using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DACS.Data;
using DACS.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace DACS.Controllers
{
    [Authorize]
    public class AIController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AIController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Roadmap()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var lichSu = await _context.KetQuaThis
                .Where(k => k.UserId == user.Id)
                .Include(k => k.DeThi!)
                    .ThenInclude(d => d!.ChuDe)
                .OrderByDescending(k => k.NgayThi)
                .ToListAsync();

            var thongKeChuDe = new Dictionary<string, (int Dung, int Sai)>();
            var thongKeDeThi = new List<object>();

            foreach (var kq in lichSu)
            {
                thongKeDeThi.Add(new
                {
                    TenDeThi = kq.DeThi?.TenDeThi ?? "Không rõ",
                    TenChuDe = kq.DeThi?.ChuDe?.TenChuDe ?? "Chung",
                    kq.TongDiem,
                    kq.SoCauDung,
                    NgayThi = kq.NgayThi.ToString("dd/MM/yyyy")
                });

                if (string.IsNullOrEmpty(kq.ChiTietJson)) continue;

                string chuDeThuc = kq.DeThi?.ChuDe?.TenChuDe ?? "Chung";
                try
                {
                    var chiTiet = JsonSerializer.Deserialize<List<JsonElement>>(kq.ChiTietJson);
                    if (chiTiet == null) continue;

                    int dung = 0, sai = 0;
                    foreach (var cau in chiTiet)
                    {
                        bool laDung = false;
                        if (cau.TryGetProperty("LaDung", out var laDungEl))
                        {
                            if (laDungEl.ValueKind == JsonValueKind.True)
                                laDung = true;
                            else if (laDungEl.ValueKind == JsonValueKind.String)
                                laDung = laDungEl.GetString()?.Equals("True", StringComparison.OrdinalIgnoreCase) == true;
                        }
                        if (laDung) dung++; else sai++;
                    }

                    if (!thongKeChuDe.ContainsKey(chuDeThuc))
                        thongKeChuDe[chuDeThuc] = (0, 0);
                    var cur = thongKeChuDe[chuDeThuc];
                    thongKeChuDe[chuDeThuc] = (cur.Dung + dung, cur.Sai + sai);
                }
                catch { /* Bỏ qua JSON lỗi */ }
            }

            var phanTichChuDe = thongKeChuDe
                .Select(kv => new
                {
                    ChuDe    = kv.Key,
                    SoDung   = kv.Value.Dung,
                    SoSai    = kv.Value.Sai,
                    TiLeDung = kv.Value.Dung + kv.Value.Sai > 0
                        ? (double)kv.Value.Dung / (kv.Value.Dung + kv.Value.Sai) * 100
                        : 0.0
                })
                .OrderBy(x => x.TiLeDung)
                .ToList();

            var goiY = new List<string>();
            foreach (var cd in phanTichChuDe.Where(x => x.TiLeDung < 60))
                goiY.Add($"Cần ôn tập thêm chủ đề: <b>{cd.ChuDe}</b> (tỉ lệ đúng {cd.TiLeDung:F0}%)");
            foreach (var cd in phanTichChuDe.Where(x => x.TiLeDung >= 80))
                goiY.Add($"Bạn đang làm tốt chủ đề: <b>{cd.ChuDe}</b> (tỉ lệ đúng {cd.TiLeDung:F0}%)");

            ViewBag.LichSu        = lichSu;
            ViewBag.PhanTichChuDe = phanTichChuDe;
            ViewBag.ThongKeDeThi  = thongKeDeThi;
            ViewBag.GoiY          = goiY;
            ViewBag.TongBaiBai    = lichSu.Count;
            ViewBag.DiemTrungBinh = lichSu.Count > 0 ? (double)lichSu.Average(k => k.TongDiem) : 0.0;

            return View();
        }
    }
}
