using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DACS.Data;
using DACS.Models;

namespace DACS.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Lấy thông báo cho user (API)
        public async Task<IActionResult> GetAll()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { count = 0, items = new List<object>() });

            var tbs = await _context.ThongBaos
                .Where(t => t.UserId == null || t.UserId == user.Id)
                .OrderByDescending(t => t.ThoiGian)
                .Take(20)
                .Select(t => new {
                    t.Id, t.NoiDung, t.DuongDan, t.LoaiIcon, t.DaDoc,
                    ThoiGian = t.ThoiGian.ToString("dd/MM HH:mm")
                })
                .ToListAsync();

            int chuaDoc = await _context.ThongBaos
                .CountAsync(t => (t.UserId == null || t.UserId == user.Id) && !t.DaDoc);

            return Json(new { count = chuaDoc, items = tbs });
        }

        // Đánh dấu đã đọc
        [HttpPost][ValidateAntiForgeryToken]
        public async Task<IActionResult> DanhDauDaDoc(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var tb = await _context.ThongBaos.FindAsync(id);
            if (tb != null && (tb.UserId == null || tb.UserId == user!.Id))
            {
                tb.DaDoc = true;
                await _context.SaveChangesAsync();
            }
            return Json(new { success = true });
        }

        // Đánh dấu tất cả đã đọc
        [HttpPost][ValidateAntiForgeryToken]
        public async Task<IActionResult> DocTatCa()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false });

            var tbs = await _context.ThongBaos
                .Where(t => (t.UserId == null || t.UserId == user.Id) && !t.DaDoc)
                .ToListAsync();
            tbs.ForEach(t => t.DaDoc = true);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        // Admin gửi thông báo
        [Authorize(Roles = "Admin")]
        [HttpPost][ValidateAntiForgeryToken]
        public async Task<IActionResult> GuiThongBao(string noiDung, string? userId, string loaiIcon = "info")
        {
            if (string.IsNullOrWhiteSpace(noiDung)) return Json(new { success = false });
            _context.ThongBaos.Add(new ThongBao
            {
                NoiDung = noiDung,
                UserId = string.IsNullOrEmpty(userId) ? null : userId,
                LoaiIcon = loaiIcon,
                ThoiGian = DateTime.Now
            });
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}
