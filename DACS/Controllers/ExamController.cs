using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DACS.Data;
using DACS.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;
using System.Text;

namespace DACS.Controllers
{
    [Authorize]
    public class ExamController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public ExamController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
        }

        // ==========================================
        // 1. QUẢN LÝ & TẠO ĐỀ THI
        // ==========================================

        public async Task<IActionResult> Index(string? mucDo)
        {
            var query = _context.DeThis.Include(d => d.ChuDe).AsQueryable();

            if (!string.IsNullOrEmpty(mucDo))
                query = query.Where(d => d.MucDoKho == mucDo);

            var dsDeThi = await query.ToListAsync();

            var user = await _userManager.GetUserAsync(User);

            var bookmarks = user != null
                ? await _context.DeThiYeuThichs
                    .Where(b => b.UserId == user.Id)
                    .Select(b => b.MaDeThi)
                    .ToListAsync()
                : new List<int>();

            ViewBag.MucDoFilter = mucDo;
            ViewBag.DanhSachMucDo = new List<string> { "Dễ", "Trung bình", "Khó" };
            ViewBag.Bookmarks = bookmarks;

            return View(dsDeThi);
        }

        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create()
        {
            ViewBag.MaChuDe = new SelectList(
                await _context.ChuDes.AsNoTracking().ToListAsync(),
                "MaChuDe",
                "TenChuDe");

            ViewBag.DanhSachMucDo = new SelectList(new[] { "Dễ", "Trung bình", "Khó" });

            ViewBag.DanhSachDeThi = new SelectList(
                await _context.DeThis.AsNoTracking().ToListAsync(),
                "MaDeThi",
                "TenDeThi");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create(DeThi deThi)
        {
            if (ModelState.IsValid)
            {
                _context.DeThis.Add(deThi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.MaChuDe = new SelectList(
                await _context.ChuDes.AsNoTracking().ToListAsync(),
                "MaChuDe",
                "TenChuDe",
                deThi.MaChuDe);

            ViewBag.DanhSachMucDo = new SelectList(
                new[] { "Dễ", "Trung bình", "Khó" },
                deThi.MucDoKho);

            ViewBag.DanhSachDeThi = new SelectList(
                await _context.DeThis.AsNoTracking().ToListAsync(),
                "MaDeThi",
                "TenDeThi");

            return View(deThi);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Json(new { success = false, message = "Tên chủ đề không được để trống." });

            var existing = await _context.ChuDes.AnyAsync(c => c.TenChuDe == name.Trim());

            if (existing)
                return Json(new { success = false, message = "Chủ đề này đã tồn tại trong hệ thống." });

            var newCategory = new ChuDe
            {
                TenChuDe = name.Trim()
            };

            _context.ChuDes.Add(newCategory);
            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                id = newCategory.MaChuDe,
                name = newCategory.TenChuDe
            });
        }

        [Authorize(Roles = "Admin,Teacher")]
        public IActionResult Edit(int id)
        {
            var deThi = _context.DeThis.Find(id);

            if (deThi == null)
                return NotFound();

            ViewBag.MaChuDe = new SelectList(
                _context.ChuDes,
                "MaChuDe",
                "TenChuDe",
                deThi.MaChuDe);

            ViewBag.DanhSachMucDo = new SelectList(
                new[] { "Dễ", "Trung bình", "Khó" },
                deThi.MucDoKho);

            return View(deThi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Edit(int id, DeThi deThi)
        {
            if (id != deThi.MaDeThi)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(deThi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.MaChuDe = new SelectList(
                _context.ChuDes,
                "MaChuDe",
                "TenChuDe",
                deThi.MaChuDe);

            ViewBag.DanhSachMucDo = new SelectList(
                new[] { "Dễ", "Trung bình", "Khó" },
                deThi.MucDoKho);

            return View(deThi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Delete(int id)
        {
            var deThi = await _context.DeThis.FindAsync(id);

            if (deThi != null)
            {
                _context.CauHois.RemoveRange(_context.CauHois.Where(c => c.MaDeThi == id));
                _context.DeThis.Remove(deThi);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // ==========================================
        // 2. KHÔNG GIAN THI CỬ & LUYỆN TẬP
        // ==========================================

        public async Task<IActionResult> DetailsByTopic(string topic)
        {
            if (string.IsNullOrEmpty(topic))
                return RedirectToAction(nameof(Index));

            var dsDeThi = await _context.DeThis
                .Include(d => d.ChuDe)
                .Where(d => d.ChuDe.TenChuDe == topic)
                .ToListAsync();

            ViewBag.TopicName = topic;

            return View(dsDeThi);
        }

        public async Task<IActionResult> Details(int id)
        {
            var deThi = await _context.DeThis
                .Include(d => d.ChuDe)
                .FirstOrDefaultAsync(d => d.MaDeThi == id);

            if (deThi == null)
                return NotFound();

            var cauHois = await _context.CauHois
                .Where(c => c.MaDeThi == id)
                .OrderBy(x => Guid.NewGuid())
                .Take(deThi.SoCauHoi ?? 40)
                .ToListAsync();

            ViewBag.DeThi = deThi;
            ViewBag.IsPractice = false;

            return View(cauHois);
        }

        public async Task<IActionResult> Practice(int id)
        {
            var deThi = await _context.DeThis
                .Include(d => d.ChuDe)
                .FirstOrDefaultAsync(d => d.MaDeThi == id);

            if (deThi == null)
                return NotFound();

            var cauHois = await _context.CauHois
                .Where(c => c.MaDeThi == id)
                .OrderBy(x => Guid.NewGuid())
                .Take(deThi.SoCauHoi ?? 40)
                .ToListAsync();

            ViewBag.DeThi = deThi;
            ViewBag.IsPractice = true;

            return View("Details", cauHois);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(int maDethi, IFormCollection form)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            var deThi = await _context.DeThis.FindAsync(maDethi);

            if (deThi == null)
                return NotFound();

            var cauHois = await _context.CauHois
                .Where(c => c.MaDeThi == maDethi)
                .ToListAsync();

            int soCauDung = 0;
            var chiTietList = new List<object>();

            foreach (var cauHoi in cauHois)
            {
                var dapAnChon = form[$"cauHoi_{cauHoi.MaCauHoi}"].ToString();

                bool laDung =
                    !string.IsNullOrEmpty(dapAnChon) &&
                    dapAnChon.Equals(cauHoi.DapAnDung, StringComparison.OrdinalIgnoreCase);

                if (laDung)
                    soCauDung++;

                chiTietList.Add(new
                {
                    cauHoi.MaCauHoi,
                    cauHoi.NoiDung,
                    cauHoi.PhuongAnA,
                    cauHoi.PhuongAnB,
                    cauHoi.PhuongAnC,
                    cauHoi.PhuongAnD,
                    DapAnChon = dapAnChon,
                    cauHoi.DapAnDung,
                    LaDung = laDung
                });

                if (!laDung)
                {
                    var existing = await _context.CauHoiSais
                        .FirstOrDefaultAsync(c =>
                            c.UserId == user.Id &&
                            c.MaCauHoi == cauHoi.MaCauHoi);

                    if (existing != null)
                    {
                        existing.LanSai++;
                        existing.LanCuoiSai = DateTime.Now;
                        existing.DapAnDaChon = dapAnChon;
                        _context.CauHoiSais.Update(existing);
                    }
                    else
                    {
                        _context.CauHoiSais.Add(new CauHoiSai
                        {
                            UserId = user.Id,
                            MaCauHoi = cauHoi.MaCauHoi,
                            DapAnDaChon = dapAnChon,
                            LanSai = 1,
                            LanCuoiSai = DateTime.Now
                        });
                    }
                }
            }

            float tongDiem = cauHois.Count > 0
                ? (float)soCauDung / cauHois.Count * 10
                : 0;

            var ketQua = new KetQuaThi
            {
                UserId = user.Id,
                MaDeThi = maDethi,
                TongDiem = (float)Math.Round(tongDiem, 1),
                SoCauDung = soCauDung,
                NgayThi = DateTime.Now,
                ChiTietJson = JsonSerializer.Serialize(chiTietList)
            };

            _context.KetQuaThis.Add(ketQua);
            await _context.SaveChangesAsync();

            return RedirectToAction("Result", new { id = ketQua.MaKetQua });
        }

        public async Task<IActionResult> Result(int id)
        {
            var ketQua = await _context.KetQuaThis
                .Include(k => k.DeThi)
                .Include(k => k.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.MaKetQua == id);

            if (ketQua == null)
                return NotFound();

            var topRankRaw = await _context.KetQuaThis
                .Where(k => k.MaDeThi == ketQua.MaDeThi)
                .GroupBy(k => k.UserId)
                .Select(g => new
                {
                    Name = g.Select(x => x.User!.FullName).FirstOrDefault(),
                    MaxScore = g.Max(x => x.TongDiem)
                })
                .OrderByDescending(x => x.MaxScore)
                .Take(5)
                .ToListAsync();

            ViewBag.TopList = topRankRaw
                .Select(x => new KeyValuePair<string, float>(
                    !string.IsNullOrWhiteSpace(x.Name) ? x.Name : "Ẩn danh",
                    x.MaxScore))
                .ToList();

            ViewBag.ChiTiet = JsonSerializer.Deserialize<List<JsonElement>>(ketQua.ChiTietJson ?? "[]");

            return View(ketQua);
        }

        public async Task<IActionResult> History()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            var lichSu = await _context.KetQuaThis
                .Where(k => k.UserId == user.Id)
                .Include(k => k.DeThi)
                .OrderByDescending(k => k.NgayThi)
                .ToListAsync();

            return View(lichSu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteHistory(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account", new { area = "Identity" });

            var ketQua = await _context.KetQuaThis
                .FirstOrDefaultAsync(x =>
                    x.MaKetQua == id &&
                    x.UserId == userId);

            if (ketQua == null)
            {
                TempData["Error"] = "Không tìm thấy lịch sử làm bài.";
                return RedirectToAction("History");
            }

            _context.KetQuaThis.Remove(ketQua);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã xóa lịch sử làm bài.";

            return RedirectToAction("History");
        }

        // ==========================================
        // 3. SỔ TAY CÂU SAI & BOOKMARK
        // ==========================================

        public async Task<IActionResult> SoTayCauSai()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            var cauSais = await _context.CauHoiSais
                .Where(c => c.UserId == user.Id)
                .Include(c => c.CauHoi)
                .ThenInclude(c => c!.DeThi)
                .OrderByDescending(c => c.LanSai)
                .ToListAsync();

            return View(cauSais);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaCauSai(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var item = await _context.CauHoiSais.FindAsync(id);

            if (item != null && user != null && item.UserId == user.Id)
            {
                _context.CauHoiSais.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("SoTayCauSai");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleBookmark(int maDeThi)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Json(new { success = false });

            var existing = await _context.DeThiYeuThichs
                .FirstOrDefaultAsync(b =>
                    b.UserId == user.Id &&
                    b.MaDeThi == maDeThi);

            bool isBookmarked;

            if (existing != null)
            {
                _context.DeThiYeuThichs.Remove(existing);
                isBookmarked = false;
            }
            else
            {
                _context.DeThiYeuThichs.Add(new DeThiYeuThich
                {
                    UserId = user.Id,
                    MaDeThi = maDeThi
                });

                isBookmarked = true;
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, isBookmarked });
        }

        public async Task<IActionResult> YeuThich()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            var dsYeuThich = await _context.DeThiYeuThichs
                .Where(b => b.UserId == user.Id)
                .Include(b => b.DeThi)
                .ThenInclude(d => d!.ChuDe)
                .OrderByDescending(b => b.NgayLuu)
                .ToListAsync();

            return View(dsYeuThich);
        }

        // ==========================================
        // 4. GIẢNG VIÊN AI GIẢI THÍCH LỖI SAI
        // ==========================================

        [HttpPost]
        public async Task<IActionResult> GiaiThichAI(int maCauHoi, string dapAnChon, string dapAnDung)
        {
            var cauHoi = await _context.CauHois
                .Include(c => c.DeThi)
                .ThenInclude(d => d!.ChuDe)
                .FirstOrDefaultAsync(c => c.MaCauHoi == maCauHoi);

            if (cauHoi == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Không tìm thấy câu hỏi."
                });
            }

            var result = await GetAiExplanation(cauHoi, dapAnChon, dapAnDung);

            return Json(result);
        }

        private async Task<object> GetAiExplanation(CauHoi cauHoi, string dapAnChon, string dapAnDung)
        {
            string GetDapAnText(string key) => key?.Trim().ToUpper() switch
            {
                "A" => cauHoi.PhuongAnA ?? "",
                "B" => cauHoi.PhuongAnB ?? "",
                "C" => cauHoi.PhuongAnC ?? "",
                "D" => cauHoi.PhuongAnD ?? "",
                _ => ""
            };

            dapAnChon = dapAnChon?.Trim().ToUpper() ?? "";
            dapAnDung = dapAnDung?.Trim().ToUpper() ?? "";

            var tenDeThi = cauHoi.DeThi?.TenDeThi ?? "Không xác định";
            var tenChuDe = cauHoi.DeThi?.ChuDe?.TenChuDe ?? "Không xác định";

            var pA = cauHoi.PhuongAnA ?? "";
            var pB = cauHoi.PhuongAnB ?? "";
            var pC = cauHoi.PhuongAnC ?? "";
            var pD = cauHoi.PhuongAnD ?? "";
            var nd = cauHoi.NoiDung ?? "";

            var dapAnDungText = GetDapAnText(dapAnDung);
            var dapAnChonText = GetDapAnText(dapAnChon);

            var prompt = $@"
Bạn là một giảng viên đại học có kinh nghiệm giảng dạy nhiều môn học.

Hãy giải thích chi tiết lỗi sai cho sinh viên dựa trên câu hỏi trắc nghiệm sau.

Thông tin bài thi:
- Tên đề thi: {tenDeThi}
- Chủ đề/môn học: {tenChuDe}

Câu hỏi:
{nd}

Các đáp án:
A. {pA}
B. {pB}
C. {pC}
D. {pD}

Sinh viên chọn: {dapAnChon} — {dapAnChonText}
Đáp án đúng: {dapAnDung} — {dapAnDungText}

Yêu cầu trả lời:
- Trả lời bằng tiếng Việt.
- Trình bày như một giảng viên đang sửa bài cho sinh viên.
- Giải thích phù hợp với môn học/chủ đề, không mặc định là tiếng Anh.
- Nếu là môn tiếng Anh thì giải thích từ vựng, ngữ pháp, synonym, antonym nếu có.
- Nếu là môn kỹ thuật, cơ sở dữ liệu, mạng máy tính, lập trình, tin học thì giải thích theo kiến thức chuyên ngành.
- Nếu là môn lý thuyết thì giải thích khái niệm, bản chất và cách phân biệt.
- Phân tích rõ vì sao đáp án đúng là đúng.
- Phân tích rõ vì sao đáp án sinh viên chọn là sai.
- Phân tích từng đáp án A, B, C, D.
- Có mẹo ghi nhớ hoặc cách làm bài.
- Có ví dụ minh họa nếu phù hợp.
- Không bịa kiến thức ngoài câu hỏi nếu không đủ dữ kiện.

Yêu cầu định dạng:
- Chỉ trả về HTML.
- Không dùng markdown.
- Không dùng ```html.
- Không cần thẻ html, head, body.
- Toàn bộ nội dung nằm trong một div class='ai-detail'.

Bố cục HTML bắt buộc:

<div class='ai-detail'>
    <h5>🤖 Giảng viên AI giải thích chi tiết</h5>

    <div class='mb-3'>
        <b>📘 Phân tích câu hỏi:</b>
        <p>Giải thích câu hỏi đang hỏi kiến thức gì.</p>
    </div>

    <div class='mb-3'>
        <b>✅ Vì sao đáp án đúng là {dapAnDung} — {dapAnDungText}?</b>
        <p>Giải thích rõ lý do đáp án đúng.</p>
    </div>

    <div class='mb-3'>
        <b>❌ Vì sao đáp án bạn chọn là {dapAnChon} — {dapAnChonText} chưa chính xác?</b>
        <p>Giải thích lỗi sai của sinh viên.</p>
    </div>

    <div class='mb-3'>
        <b>🔎 Phân tích từng đáp án:</b>
        <ul>
            <li><b>A.</b> Nhận xét đáp án A.</li>
            <li><b>B.</b> Nhận xét đáp án B.</li>
            <li><b>C.</b> Nhận xét đáp án C.</li>
            <li><b>D.</b> Nhận xét đáp án D.</li>
        </ul>
    </div>

    <div class='mb-3'>
        <b>🧠 Mẹo ghi nhớ / cách làm bài:</b>
        <p>Đưa ra mẹo học hoặc cách suy luận để lần sau làm đúng.</p>
    </div>

    <div class='mb-2'>
        <b>📝 Ví dụ minh họa:</b>
        <p>Cho ví dụ phù hợp với nội dung câu hỏi.</p>
    </div>
</div>
";

            var geminiKey =
                _config["GeminiApiKey"] ??
                _config["Gemini:ApiKey"] ??
                "";

            if (!string.IsNullOrWhiteSpace(geminiKey) &&
                geminiKey != "YOUR_GEMINI_API_KEY_HERE")
            {
                try
                {
                    var res = await CallGemini(geminiKey, prompt);

                    if (!string.IsNullOrWhiteSpace(res))
                        return new { success = true, message = res };
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Gemini error: " + ex.Message);
                }
            }

            var openRouterKey =
                _config["OpenRouterApiKey"] ??
                _config["OpenRouter:ApiKey"] ??
                "";

            if (!string.IsNullOrWhiteSpace(openRouterKey) &&
                openRouterKey != "YOUR_OPENROUTER_API_KEY_HERE")
            {
                try
                {
                    var res = await CallOpenRouter(openRouterKey, prompt);

                    if (!string.IsNullOrWhiteSpace(res))
                        return new { success = true, message = res };
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("OpenRouter error: " + ex.Message);
                }
            }

            return new
            {
                success = true,
                message = BuildBasicExplanation(cauHoi, dapAnChon, dapAnDung)
            };
        }

        private async Task<string?> CallGemini(string apiKey, string prompt)
        {
            using var http = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(25)
            };

            http.DefaultRequestHeaders.Add("Referer", "https://localhost:7016/");
            http.DefaultRequestHeaders.Add("Origin", "https://localhost:7016");

            var model =
                _config["Gemini:Model"] ??
                _config["GeminiModel"] ??
                "gemini-2.0-flash";

            var url =
                $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            var body = JsonSerializer.Serialize(new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new
                            {
                                text = prompt
                            }
                        }
                    }
                }
            });

            var resp = await http.PostAsync(
                url,
                new StringContent(body, Encoding.UTF8, "application/json"));

            if (!resp.IsSuccessStatusCode)
                return null;

            var json = await resp.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);

            if (doc.RootElement.TryGetProperty("error", out _))
                return null;

            if (!doc.RootElement.TryGetProperty("candidates", out var candidates) ||
                candidates.GetArrayLength() == 0)
                return null;

            if (!candidates[0].TryGetProperty("content", out var content))
                return null;

            if (!content.TryGetProperty("parts", out var parts) ||
                parts.GetArrayLength() == 0)
                return null;

            if (!parts[0].TryGetProperty("text", out var text))
                return null;

            return text.GetString();
        }

        private async Task<string?> CallOpenRouter(string apiKey, string prompt)
        {
            using var http = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(25)
            };

            http.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);
            http.DefaultRequestHeaders.Add("HTTP-Referer", "https://localhost");
            http.DefaultRequestHeaders.Add("X-Title", "EDUTECH AI");

            var model =
                _config["OpenRouter:Model"] ??
                _config["OpenRouterModel"] ??
                "google/gemma-3-12b-it:free";

            var body = JsonSerializer.Serialize(new
            {
                model = model,
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                },
                max_tokens = 1500
            });

            var resp = await http.PostAsync(
                "https://openrouter.ai/api/v1/chat/completions",
                new StringContent(body, Encoding.UTF8, "application/json"));

            if (!resp.IsSuccessStatusCode)
                return null;

            var json = await resp.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("choices", out var choices) ||
                choices.GetArrayLength() == 0)
                return null;

            if (!choices[0].TryGetProperty("message", out var message))
                return null;

            if (!message.TryGetProperty("content", out var content))
                return null;

            return content.GetString();
        }

        private string BuildBasicExplanation(CauHoi cauHoi, string dapAnChon, string dapAnDung)
        {
            string GetDapAn(string key) => key?.Trim().ToUpper() switch
            {
                "A" => cauHoi.PhuongAnA ?? "",
                "B" => cauHoi.PhuongAnB ?? "",
                "C" => cauHoi.PhuongAnC ?? "",
                "D" => cauHoi.PhuongAnD ?? "",
                _ => ""
            };

            dapAnChon = dapAnChon?.Trim().ToUpper() ?? "";
            dapAnDung = dapAnDung?.Trim().ToUpper() ?? "";

            var dapAnDungText = GetDapAn(dapAnDung);
            var dapAnChonText = GetDapAn(dapAnChon);

            var sb = new StringBuilder();

            sb.Append("<div class='ai-detail'>");
            sb.Append("<h5>🤖 Giảng viên AI giải thích chi tiết</h5>");

            sb.Append("<div class='mb-3'>");
            sb.Append("<b>📘 Phân tích câu hỏi:</b>");
            sb.Append("<p>Câu hỏi yêu cầu bạn xác định đáp án đúng nhất dựa trên kiến thức của môn học hoặc chủ đề đang làm. Hãy đọc kỹ nội dung câu hỏi, xác định từ khóa chính rồi so sánh từng phương án.</p>");
            sb.Append("</div>");

            if (!string.IsNullOrEmpty(dapAnChon) && dapAnChon == dapAnDung)
            {
                sb.Append("<div class='alert alert-success p-2 mb-3 rounded-3'>");
                sb.Append("<i class='fas fa-check-circle me-1'></i>");
                sb.Append(" <b>Chính xác!</b> Bạn đã chọn đúng đáp án ");
                sb.Append($"<b>{dapAnDung} — {dapAnDungText}</b>.");
                sb.Append("</div>");
            }
            else if (!string.IsNullOrEmpty(dapAnChon))
            {
                sb.Append("<div class='alert alert-danger p-2 mb-3 rounded-3'>");
                sb.Append("<i class='fas fa-times-circle me-1'></i>");
                sb.Append(" Bạn chọn ");
                sb.Append($"<b>{dapAnChon} — {dapAnChonText}</b>");
                sb.Append(" nhưng chưa chính xác.");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class='alert alert-warning p-2 mb-3 rounded-3'>");
                sb.Append("<i class='fas fa-exclamation-circle me-1'></i>");
                sb.Append(" Bạn đã bỏ qua câu này.");
                sb.Append("</div>");
            }

            sb.Append("<div class='mb-3'>");
            sb.Append($"<b>✅ Vì sao đáp án đúng là {dapAnDung} — {dapAnDungText}?</b>");
            sb.Append("<p>Đáp án này phù hợp nhất với yêu cầu của câu hỏi. Đây là phương án khớp với kiến thức trọng tâm mà câu hỏi đang kiểm tra.</p>");
            sb.Append("</div>");

            if (!string.IsNullOrEmpty(dapAnChon) && dapAnChon != dapAnDung)
            {
                sb.Append("<div class='mb-3'>");
                sb.Append($"<b>❌ Vì sao đáp án bạn chọn là {dapAnChon} — {dapAnChonText} chưa chính xác?</b>");
                sb.Append("<p>Đáp án bạn chọn chưa đúng vì không khớp hoàn toàn với yêu cầu của câu hỏi hoặc dễ gây nhầm lẫn với một khái niệm khác. Khi làm bài, cần so sánh bản chất của từng đáp án thay vì chỉ nhìn từ khóa quen thuộc.</p>");
                sb.Append("</div>");
            }

            sb.Append("<div class='mb-3'>");
            sb.Append("<b>🔎 Phân tích từng đáp án:</b>");
            sb.Append("<ul>");

            foreach (var opt in new[] { "A", "B", "C", "D" })
            {
                var txt = GetDapAn(opt);

                if (string.IsNullOrWhiteSpace(txt))
                    continue;

                if (opt == dapAnDung)
                {
                    sb.Append($"<li><b>{opt}. {txt}</b>: Đây là đáp án đúng vì phù hợp nhất với nội dung câu hỏi.</li>");
                }
                else if (opt == dapAnChon)
                {
                    sb.Append($"<li><b>{opt}. {txt}</b>: Đây là đáp án bạn chọn, nhưng chưa chính xác so với yêu cầu của câu hỏi.</li>");
                }
                else
                {
                    sb.Append($"<li><b>{opt}. {txt}</b>: Đây không phải là phương án phù hợp nhất.</li>");
                }
            }

            sb.Append("</ul>");
            sb.Append("</div>");

            sb.Append("<div class='mb-3'>");
            sb.Append("<b>🧠 Mẹo ghi nhớ / cách làm bài:</b>");
            sb.Append("<p>Hãy đọc kỹ câu hỏi, gạch chân từ khóa chính, loại trước các đáp án chắc chắn sai, sau đó so sánh hai đáp án còn lại theo bản chất kiến thức. Với câu hỏi lý thuyết, nên học theo khái niệm và ví dụ thay vì học thuộc máy móc.</p>");
            sb.Append("</div>");

            sb.Append("<div class='mb-2'>");
            sb.Append("<b>📝 Ví dụ minh họa:</b>");
            sb.Append("<p>Khi gặp câu hỏi trắc nghiệm, không nên chọn đáp án chỉ vì thấy quen. Hãy kiểm tra xem đáp án đó có trả lời đúng trọng tâm câu hỏi hay không.</p>");
            sb.Append("</div>");

            sb.Append("<hr class='my-2'>");
            sb.Append("<div class='text-muted small'>");
            sb.Append("<i class='fas fa-robot me-1'></i>");
            sb.Append(" Đây là phần giải thích cơ bản. Để có giải thích chuyên sâu theo từng môn học, hãy thêm GeminiApiKey hoặc OpenRouterApiKey vào appsettings.json.");
            sb.Append("</div>");

            sb.Append("</div>");

            return sb.ToString();
        }

        // ==========================================
        // 5. QUẢN LÝ CÂU HỎI & IMPORT EXCEL
        // ==========================================

        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> ManageCauHoi(int id)
        {
            var deThi = await _context.DeThis
                .Include(d => d.ChuDe)
                .FirstOrDefaultAsync(d => d.MaDeThi == id);

            if (deThi == null)
                return NotFound();

            var cauHois = await _context.CauHois
                .Where(c => c.MaDeThi == id)
                .OrderBy(c => c.MaCauHoi)
                .ToListAsync();

            ViewBag.DeThi = deThi;

            return View(cauHois);
        }

        [Authorize(Roles = "Admin,Teacher")]
        public IActionResult AddCauHoi(int maDeThi)
        {
            var deThi = _context.DeThis.Find(maDeThi);

            if (deThi == null)
                return NotFound();

            ViewBag.DeThi = deThi;
            ViewBag.DanhSachMucDo = new SelectList(new[] { "Dễ", "Trung bình", "Khó" });

            return View(new CauHoi
            {
                MaDeThi = maDeThi,
                MaChuDe = deThi.MaChuDe
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> AddCauHoi(CauHoi cauHoi)
        {
            if (ModelState.IsValid)
            {
                _context.CauHois.Add(cauHoi);
                await _context.SaveChangesAsync();

                return RedirectToAction("ManageCauHoi", new { id = cauHoi.MaDeThi });
            }

            ViewBag.DeThi = _context.DeThis.Find(cauHoi.MaDeThi);
            ViewBag.DanhSachMucDo = new SelectList(
                new[] { "Dễ", "Trung bình", "Khó" },
                cauHoi.MucDoKho);

            return View(cauHoi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> DeleteCauHoi(int id, int maDeThi)
        {
            var c = await _context.CauHois.FindAsync(id);

            if (c != null)
            {
                _context.CauHois.Remove(c);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ManageCauHoi", new { id = maDeThi });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> ImportExcel(IFormFile file, int maDeThi)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "File không hợp lệ.";
                return RedirectToAction(nameof(Create));
            }

            var deThi = await _context.DeThis.FindAsync(maDeThi);

            if (deThi == null)
            {
                TempData["Error"] = "Mã đề thi không tồn tại trong hệ thống.";
                return RedirectToAction(nameof(Create));
            }

            int count = 0;

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            using var package = new ExcelPackage(stream);
            var ws = package.Workbook.Worksheets[0];

            if (ws.Dimension == null)
            {
                TempData["Error"] = "File Excel trống.";
                return RedirectToAction(nameof(Create));
            }

            for (int row = 2; row <= ws.Dimension.Rows; row++)
            {
                var nd = ws.Cells[row, 1].Value?.ToString();

                if (string.IsNullOrWhiteSpace(nd))
                    continue;

                _context.CauHois.Add(new CauHoi
                {
                    MaDeThi = maDeThi,
                    MaChuDe = deThi.MaChuDe,
                    NoiDung = nd,
                    PhuongAnA = ws.Cells[row, 2].Value?.ToString(),
                    PhuongAnB = ws.Cells[row, 3].Value?.ToString(),
                    PhuongAnC = ws.Cells[row, 4].Value?.ToString(),
                    PhuongAnD = ws.Cells[row, 5].Value?.ToString(),
                    DapAnDung = ws.Cells[row, 6].Value?.ToString()?.Trim().ToUpper(),
                    MucDoKho = ws.Cells[row, 7].Value?.ToString() ?? "Trung bình"
                });

                count++;
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Import thành công {count} câu hỏi vào đề '{deThi.TenDeThi}'!";

            return RedirectToAction("ManageCauHoi", new { id = maDeThi });
        }
    }
}