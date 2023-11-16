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
                    // 字串 逐行分割
                    string[] numbers = fileContent.Split('\n');
                    // 去除重複
                    var uniqueNumbers = numbers.Distinct().ToArray();
                    
                    // 分類結果
                    List<ClassifiedNumber> results = new List<ClassifiedNumber>();                    

                    // 開始分類
                    foreach (var number in uniqueNumbers)
                    {
                        var result = getClassifyByNumber(number);
                        results.Add(new ClassifiedNumber { T = result, Number = number });
                    }
                    // 將結果分組
                    var groupResult = results.OrderBy(r => r.Number).GroupBy(r => r.T).ToList();

                    // 資料透過 ViewBag 傳遞
                    ViewBag.Results = groupResult;
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

        private MyType getClassifyByNumber(string input)
        {
            var first5 = input.Substring(0, 5);
            switch (first5)
            {
                case "63907":
                case "63908":
                case "63909":
                case "63910":
                case "63912":
                case "63918":
                case "63919":
                case "63920":
                case "63921":
                case "63922":
                case "63923":
                case "63924":
                case "63925":
                case "63928":
                case "63929":
                case "63930":
                case "63931":
                case "63932":
                case "63933":
                case "63938":
                case "63939":
                case "63942":
                case "63943":
                case "63946":
                case "63947":
                case "63948":
                case "63950":
                case "63951":
                case "63960":
                case "63961":
                case "63962":
                case "63963":
                case "63964":
                case "63968":
                case "63969":
                case "63970":
                case "63973":
                case "63974":
                case "63981":
                case "63988":
                case "63998":
                case "63999":
                    return MyType.SMART;
                case "63904":
                case "63905":
                case "63906":
                case "63915":
                case "63916":
                case "63917":
                case "63926":
                case "63927":
                case "63935":
                case "63936":
                case "63945":
                case "63952":
                case "63953":
                case "63954":
                case "63955":
                case "63956":
                case "63965":
                case "63966":
                case "63967":
                case "63975":
                case "63977":
                case "63983":
                case "63986":
                case "63987":
                case "63989":
                case "63995":
                case "63997":
                    return MyType.GLOBE;
                case "63991":
                case "63992":
                case "63993":
                case "63994":
                    return MyType.DITO;
                case "63996":
                    return MyType.CHERRY;
                default:
                    return MyType.NONE;
            }
        }
    }
    public enum MyType
    {
        SMART = 0,
        GLOBE = 1,
        DITO = 2,
        CHERRY = 3,
        NONE = 4
    }

    public class ClassifiedNumber
    {

        public MyType T { get; set; }
        public string Number { get; set; }
    }
}