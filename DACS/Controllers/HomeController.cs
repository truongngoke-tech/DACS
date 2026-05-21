using DACS.Data;
using DACS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DACS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel();

            // 1. Lấy danh sách đề thi (hiển thị tối đa 6 đề mới nhất cho trang chủ)
            model.DanhSachDeThi = await _context.DeThis
                .OrderByDescending(d => d.MaDeThi)
                .Take(6)
                .ToListAsync();

            // 2. Lấy kết quả thi nếu user đã đăng nhập
            if (User?.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var ketQuaList = await _context.KetQuaThis
                    .Where(k => k.UserId == userId)
                    .ToListAsync();

                if (ketQuaList.Any())
                {
                    model.SoDeDaHoanThanh = ketQuaList.Count;
                    // Tính trung bình tỉ lệ đúng (giả sử 10 câu/đề)
                    model.TiLeDungTrungBinh = ketQuaList.Average(k => (double)k.SoCauDung / 10 * 100);
                    model.PhutHocTapTuanNay = 450;
                    model.NgayLienTiep = 5;
                }
            }

            return View(model);
        }

        public IActionResult DanhSachDeThi() => View();

        public IActionResult PhanTichAI() => View();

        [Authorize]
        public IActionResult Profile()
        {
            // Lấy thông tin user hiện tại để hiển thị
            ViewBag.UserName = User.Identity?.Name;
            return View();
        }
        [Route("Home/Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 404)
                return View("~/Views/Shared/Error404.cshtml");
            return View("Error");
        }


    }
}