using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DACS.Models;
using DACS.Data;
using Microsoft.EntityFrameworkCore;

namespace DACS.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public UserController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var ketQuas = await _context.KetQuaThis.Where(k => k.UserId == user.Id).ToListAsync();
            ViewBag.SoBai   = ketQuas.Count;
            ViewBag.DiemTB  = ketQuas.Count > 0 ? Math.Round(ketQuas.Average(k => (double)k.TongDiem), 1) : 0.0;
            ViewBag.DiemCao = ketQuas.Count > 0 ? (double)ketQuas.Max(k => k.TongDiem) : 0.0;

            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.Role = roles.FirstOrDefault() ?? "Student";

            ViewBag.SoBookmark = await _context.DeThiYeuThichs.CountAsync(b => b.UserId == user.Id);

            return View(user);
        }

        [HttpPost][ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(string fullName, string phoneNumber)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            if (string.IsNullOrWhiteSpace(fullName)) { TempData["Error"] = "Họ tên không được để trống."; return RedirectToAction("Profile"); }

            user.FullName = fullName.Trim();
            user.PhoneNumber = phoneNumber?.Trim();
            var result = await _userManager.UpdateAsync(user);

            TempData[result.Succeeded ? "Success" : "Error"] = result.Succeeded
                ? "Cập nhật thông tin thành công!"
                : "Lỗi: " + string.Join(", ", result.Errors.Select(e => e.Description));

            return RedirectToAction("Profile");
        }

        // Upload avatar
        [HttpPost][ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadAvatar(IFormFile avatarFile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (avatarFile == null || avatarFile.Length == 0)
            { TempData["Error"] = "Vui lòng chọn ảnh."; return RedirectToAction("Profile"); }

            // Kiểm tra định dạng
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var ext = Path.GetExtension(avatarFile.FileName).ToLower();
            if (!allowed.Contains(ext))
            { TempData["Error"] = "Chỉ chấp nhận file ảnh (jpg, png, gif, webp)."; return RedirectToAction("Profile"); }

            if (avatarFile.Length > 2 * 1024 * 1024)
            { TempData["Error"] = "Ảnh không được vượt quá 2MB."; return RedirectToAction("Profile"); }

            // Lưu file
            var uploadsDir = Path.Combine(_env.WebRootPath, "uploads", "avatars");
            Directory.CreateDirectory(uploadsDir);

            // Xóa avatar cũ nếu có
            if (!string.IsNullOrEmpty(user.AvatarUrl))
            {
                var oldFile = Path.Combine(_env.WebRootPath, user.AvatarUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldFile)) System.IO.File.Delete(oldFile);
            }

            var fileName = $"{user.Id}_{DateTime.Now:yyyyMMddHHmmss}{ext}";
            var filePath = Path.Combine(uploadsDir, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await avatarFile.CopyToAsync(stream);

            user.AvatarUrl = $"/uploads/avatars/{fileName}";
            await _userManager.UpdateAsync(user);

            TempData["Success"] = "Cập nhật ảnh đại diện thành công!";
            return RedirectToAction("Profile");
        }

        // Xóa avatar
        [HttpPost][ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaAvatar()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            if (!string.IsNullOrEmpty(user.AvatarUrl))
            {
                var oldFile = Path.Combine(_env.WebRootPath, user.AvatarUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldFile)) System.IO.File.Delete(oldFile);
                user.AvatarUrl = null;
                await _userManager.UpdateAsync(user);
            }
            TempData["Success"] = "Đã xóa ảnh đại diện.";
            return RedirectToAction("Profile");
        }
        // Trang doi mat khau
        public IActionResult DoiMatKhau() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoiMatKhau(string matKhauCu, string matKhauMoi, string xacNhanMatKhau)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (string.IsNullOrWhiteSpace(matKhauMoi) || matKhauMoi.Length < 6)
            {
                TempData["Error"] = "Mat khau moi phai co it nhat 6 ky tu.";
                return RedirectToAction("DoiMatKhau");
            }

            if (matKhauMoi != xacNhanMatKhau)
            {
                TempData["Error"] = "Mat khau xac nhan khong khop.";
                return RedirectToAction("DoiMatKhau");
            }

            var result = await _userManager.ChangePasswordAsync(user, matKhauCu, matKhauMoi);
            if (result.Succeeded)
            {
                TempData["Success"] = "Doi mat khau thanh cong! Vui long dang nhap lai.";
                return RedirectToAction("Profile");
            }

            TempData["Error"] = "Mat khau hien tai khong dung hoac co loi: " +
                string.Join(", ", result.Errors.Select(e => e.Description));
            return RedirectToAction("DoiMatKhau");
        }


    }
}
