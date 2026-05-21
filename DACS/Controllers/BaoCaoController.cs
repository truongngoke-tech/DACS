using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DACS.Data;
using DACS.Models;

namespace DACS.Controllers
{
    [Authorize]
    public class BaoCaoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BaoCaoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ===== USER: Gửi báo cáo =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuiBaoCao(int maDeThi, int? maCauHoi,
            string loaiBaoCao, string moTa)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false, message = "Vui lòng đăng nhập." });

            if (string.IsNullOrWhiteSpace(moTa))
                return Json(new { success = false, message = "Vui lòng mô tả lỗi cụ thể." });

            // Kiem tra da bao cao chua (tranh spam)
            var daBC = await _context.BaoCaoLois.AnyAsync(b =>
                b.UserId == user.Id &&
                b.MaDeThi == maDeThi &&
                b.MaCauHoi == maCauHoi &&
                b.TrangThai != "Đã sửa");

            if (daBC)
                return Json(new { success = false, message = "Bạn đã gửi báo cáo cho mục này rồi. Vui lòng chờ Admin xử lý." });

            _context.BaoCaoLois.Add(new BaoCaoLoi
            {
                UserId      = user.Id,
                MaDeThi     = maDeThi,
                MaCauHoi    = maCauHoi,
                LoaiBaoCao  = loaiBaoCao ?? "Khác",
                MoTa        = moTa.Trim(),
                TrangThai   = "Chờ xử lý",
                NgayTao     = DateTime.Now
            });
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Cảm ơn bạn! Báo cáo đã được gửi đến Admin." });
        }

        // ===== USER: Lịch sử báo cáo của mình =====
        public async Task<IActionResult> CuaToi()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var ds = await _context.BaoCaoLois
                .Where(b => b.UserId == user.Id)
                .Include(b => b.DeThi)
                .Include(b => b.CauHoi)
                .OrderByDescending(b => b.NgayTao)
                .ToListAsync();

            return View(ds);
        }

        // ===== ADMIN: Quản lý báo cáo =====
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> QuanLy(string? trangThai)
        {
            var query = _context.BaoCaoLois
                .Include(b => b.User)
                .Include(b => b.DeThi)
                .Include(b => b.CauHoi)
                .AsQueryable();

            if (!string.IsNullOrEmpty(trangThai))
                query = query.Where(b => b.TrangThai == trangThai);

            var ds = await query.OrderByDescending(b => b.NgayTao).ToListAsync();

            ViewBag.TrangThaiFilter = trangThai;
            ViewBag.SoChoXuLy = await _context.BaoCaoLois.CountAsync(b => b.TrangThai == "Chờ xử lý");
            return View(ds);
        }

        // ADMIN: Cập nhật trạng thái + ghi chú
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> CapNhatTrangThai(int id, string trangThai, string? ghiChuAdmin)
        {
            var bc = await _context.BaoCaoLois.Include(b => b.User).FirstOrDefaultAsync(b => b.Id == id);
            if (bc == null) return NotFound();

            var oldStatus = bc.TrangThai;
            bc.TrangThai   = trangThai;
            bc.GhiChuAdmin = ghiChuAdmin?.Trim();
            if (trangThai != "Chờ xử lý") bc.NgayXuLy = DateTime.Now;

            // Neu da sua -> gui thong bao cho nguoi dung
            if (trangThai == "Đã sửa" && oldStatus != "Đã sửa" && bc.UserId != null)
            {
                var deThi = await _context.DeThis.FindAsync(bc.MaDeThi);
                var noiDungTB = $"Báo cáo lỗi của bạn về đề thi <b>\"{deThi?.TenDeThi ?? "N/A"}\"</b> đã được xử lý. " +
                               (string.IsNullOrEmpty(ghiChuAdmin) ? "Cảm ơn bạn đã đóng góp!" : $"Ghi chú: {ghiChuAdmin}");

                _context.ThongBaos.Add(new ThongBao
                {
                    UserId    = bc.UserId,
                    NoiDung   = noiDungTB,
                    DuongDan  = $"/Exam/Details/{bc.MaDeThi}",
                    LoaiIcon  = "success",
                    DaDoc     = false,
                    ThoiGian  = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Đã cập nhật trạng thái báo cáo!";
            return RedirectToAction("QuanLy");
        }
    }
}
