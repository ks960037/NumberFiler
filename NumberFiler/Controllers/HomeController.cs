using Microsoft.AspNetCore.Mvc;

using NumberFiler.Models;

using System.Diagnostics;
using System.Text;

namespace NumberFiler.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 處理上傳的檔案
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // 開始讀取檔案內容
                using (var stream = new MemoryStream())
                {
                    // 讀取檔案內容到記憶體串流
                    await file.CopyToAsync(stream);

                    // 串流 轉 字串
                    var fileContent = Encoding.UTF8.GetString(stream.ToArray());

                    // 資料透過 ViewBag 傳遞
                    ViewBag.FileContent = fileContent;
                }
            }
            else
            {
                ViewBag.Message = "請選擇檔案";
            }

            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}