using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DACS.Models;
using DACS.Data;

namespace DACS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // ===== QUẢN LÝ USER =====
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (userId == null) return NotFound();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            // Khong cho phep xoa tai khoan Admin
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
            {
                TempData["Error"] = $"Không thể xóa tài khoản Admin \"{user.UserName}\". Hãy đổi role trước rồi mới xóa.";
                return RedirectToAction("Users");
            }

            // Khong cho xoa chinh minh
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Id == userId)
            {
                TempData["Error"] = "Bạn không thể tự xóa tài khoản của mình!";
                return RedirectToAction("Users");
            }

            await _userManager.DeleteAsync(user);
            TempData["Success"] = $"Đã xóa tài khoản \"{user.UserName}\" thành công.";
            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string userId, string newRole)
        {
            if (userId == null || newRole == null) return NotFound();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (!currentRoles.Contains(newRole))
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, newRole);
                if (User.Identity!.Name == user.UserName)
                    await _signInManager.RefreshSignInAsync(user);
            }
            return RedirectToAction("Users");
        }

        // ===== QUẢN LÝ CHỦ ĐỀ =====
        public async Task<IActionResult> ChuDe()
        {
            var chuDes = await _context.ChuDes
                .Include(c => c.ChuDeCha)
                .OrderBy(c => c.MaChuDe)
                .ToListAsync();

            // Đếm số đề thi & câu hỏi theo chủ đề
            var soDeTheoCD = await _context.DeThis
                .GroupBy(d => d.MaChuDe)
                .Select(g => new { MaChuDe = g.Key, SoDe = g.Count() })
                .ToDictionaryAsync(x => x.MaChuDe, x => x.SoDe);

            ViewBag.SoDeTheoCD = soDeTheoCD;
            return View(chuDes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemChuDe(string tenChuDe, int? maChuDeCha)
        {
            if (string.IsNullOrWhiteSpace(tenChuDe))
            {
                TempData["Error"] = "Tên chủ đề không được để trống!";
                return RedirectToAction("ChuDe");
            }

            bool trung = await _context.ChuDes.AnyAsync(c => c.TenChuDe == tenChuDe.Trim());
            if (trung)
            {
                TempData["Error"] = $"Chủ đề '{tenChuDe}' đã tồn tại!";
                return RedirectToAction("ChuDe");
            }

            _context.ChuDes.Add(new ChuDe
            {
                TenChuDe = tenChuDe.Trim(),
                MaChuDeCha = (maChuDeCha == 0) ? null : maChuDeCha
            });
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đã thêm chủ đề '{tenChuDe.Trim()}' thành công!";
            return RedirectToAction("ChuDe");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaChuDe(int id)
        {
            var chuDe = await _context.ChuDes.FindAsync(id);
            if (chuDe == null) { TempData["Error"] = "Không tìm thấy chủ đề!"; return RedirectToAction("ChuDe"); }

            bool coLienKet = await _context.DeThis.AnyAsync(d => d.MaChuDe == id)
                          || await _context.CauHois.AnyAsync(c => c.MaChuDe == id);
            if (coLienKet)
            {
                TempData["Error"] = "Không thể xóa! Chủ đề này đang được dùng bởi đề thi hoặc câu hỏi.";
                return RedirectToAction("ChuDe");
            }

            _context.ChuDes.Remove(chuDe);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Đã xóa chủ đề '{chuDe.TenChuDe}'.";
            return RedirectToAction("ChuDe");
        }

        // ===== BẢNG XẾP HẠNG =====
        public async Task<IActionResult> BangXepHang()
        {
            var users = await _userManager.Users.ToListAsync();
            var ketQuas = await _context.KetQuaThis.ToListAsync();

            var bangXepHang = users
                .Select(u => {
                    var dsKq = ketQuas.Where(k => k.UserId == u.Id).ToList();
                    return new {
                        TenHienThi = !string.IsNullOrWhiteSpace(u.FullName) ? u.FullName : u.UserName,
                        Email    = u.Email,
                        SoBai    = dsKq.Count,
                        DiemTB   = dsKq.Count > 0 ? Math.Round(dsKq.Average(k => (double)k.TongDiem), 1) : 0.0,
                        DiemCao  = dsKq.Count > 0 ? (double)dsKq.Max(k => k.TongDiem) : 0.0
                    };
                })
                .Where(x => x.SoBai > 0)
                .OrderByDescending(x => x.DiemTB)
                .ThenByDescending(x => x.SoBai)
                .ToList();

            ViewBag.BangXepHang = bangXepHang;
            return View();
        }
        // ===== DASHBOARD =====
        public async Task<IActionResult> Dashboard()
        {
            var users   = await _userManager.Users.ToListAsync();
            var deThi   = await _context.DeThis.ToListAsync();
            var cauHoi  = await _context.CauHois.ToListAsync();
            var ketQuas = await _context.KetQuaThis
                .Include(k => k.DeThi)
                .Include(k => k.User)
                .OrderByDescending(k => k.NgayThi)
                .ToListAsync();

            ViewBag.TongUser      = users.Count;
            ViewBag.TongDeThi     = deThi.Count;
            ViewBag.TongCauHoi    = cauHoi.Count;
            ViewBag.TongLuotThi   = ketQuas.Count;
            ViewBag.DiemTrungBinh = ketQuas.Count > 0
                ? Math.Round(ketQuas.Average(k => (double)k.TongDiem), 1)
                : 0.0;

            ViewBag.TopDe = ketQuas
                .GroupBy(k => k.MaDeThi)
                .Select(g => new {
                    TenDeThi = g.First().DeThi?.TenDeThi ?? "Khong ro",
                    SoLuot   = g.Count(),
                    DiemTB   = Math.Round(g.Average(x => (double)x.TongDiem), 1)
                })
                .OrderByDescending(x => x.SoLuot)
                .Take(5)
                .ToList();

            ViewBag.HoatDongGanDay = ketQuas.Take(10).Select(k => new {
                HoTen    = !string.IsNullOrEmpty(k.User!.FullName) ? k.User.FullName : k.User.UserName ?? "An danh",
                Email    = k.User.Email ?? "",
                TenDe    = k.DeThi?.TenDeThi ?? "Khong ro",
                Diem     = k.TongDiem,
                ThoiGian = k.NgayThi.ToString("dd/MM HH:mm")
            }).ToList();

            return View();
        }


    }
}
