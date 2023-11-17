using Microsoft.AspNetCore.Mvc;

using NumberFiler.Models;

using PhoneNumbers;

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

                    var result = numbers
                        .Distinct() // 去除重複
                        .Select(n => parseNumber(n)) // 正則化
                        .GroupBy(n => getClassifyByNumber(n))
                        .ToArray();

                    // 分類結果
                    // List<ClassifiedNumber> results = new List<ClassifiedNumber>();

                    // 開始分類
                    //foreach (var number in uniqueNumbers)
                    //{
                    //    var result = getClassifyByNumber(number);
                    //    results.Add(new ClassifiedNumber { T = result, Number = number });
                    //}
                    // 將結果分組
                    //var groupResult = results.OrderBy(r => r.Number).GroupBy(r => r.T).ToList();

                    // 資料透過 ViewBag 傳遞
                    ViewBag.Results = result;
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
            var tests = input.Split(' ');
            var first5 = tests[1];
            switch (first5)
            {
                case "907":
                case "908":
                case "909":
                case "910":
                case "912":
                case "918":
                case "919":
                case "920":
                case "921":
                case "922":
                case "923":
                case "924":
                case "925":
                case "928":
                case "929":
                case "930":
                case "931":
                case "932":
                case "933":
                case "938":
                case "939":
                case "942":
                case "943":
                case "946":
                case "947":
                case "948":
                case "950":
                case "951":
                case "960":
                case "961":
                case "962":
                case "963":
                case "964":
                case "968":
                case "969":
                case "970":
                case "973":
                case "974":
                case "981":
                case "988":
                case "998":
                case "999":
                    return MyType.SMART;
                case "904":
                case "905":
                case "906":
                case "915":
                case "916":
                case "917":
                case "926":
                case "927":
                case "935":
                case "936":
                case "945":
                case "952":
                case "953":
                case "954":
                case "955":
                case "956":
                case "965":
                case "966":
                case "967":
                case "975":
                case "977":
                case "983":
                case "986":
                case "987":
                case "989":
                case "995":
                case "997":
                    return MyType.GLOBE;
                case "991":
                case "992":
                case "993":
                case "994":
                    return MyType.DITO;
                case "996":
                    return MyType.CHERRY;
                default:
                    return MyType.NONE;
            }
        }

        private String? parseNumber(string input)
        {
            // 使用 libphonenumber
            PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();
            // 菲律賓國家代碼
            string countryCode = "PH";
            // 是不是合法電話號碼
            bool isValidNumber = false;

            // 輸出格式化後的號碼
            String formattedNumber = "";
            try
            {
                PhoneNumber phoneNumber = phoneUtil.Parse(input, countryCode);
                isValidNumber = phoneUtil.IsValidNumber(phoneNumber); // returns true for valid number
                formattedNumber = phoneUtil.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL);
            }
            catch (NumberParseException ex)
            {
                String errorMessage = "NumberParseException was thrown: " + ex.Message.ToString();
            }

            if (isValidNumber) return formattedNumber;


            else return null;
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