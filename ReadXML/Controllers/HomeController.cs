using Microsoft.AspNetCore.Mvc;
using ReadXML.Models;
using HelpReadFile;
using System.Reflection;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace ReadXML.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ReadXMLContext _context;
        private readonly IReadFileService _readFileService;

        public HomeController(ILogger<HomeController> logger, ReadXMLContext context, HelpReadFile.IReadFileService readFileService)
        {
            _logger = logger;
            _context = context;
            _readFileService = readFileService;
        }

        public IActionResult Index()
        {
            return View(new List<Person>());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ReadXML(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    ViewBag.ErrorMessage = "Vui lòng chọn file để tải lên.";
                    return View("Index", new List<Person>());
                }

                // Đọc nội dung file
                using var reader = new StreamReader(file.OpenReadStream());
                string xmlContent = reader.ReadToEnd();
                ViewBag.XmlRaw = xmlContent;

                var xmlData = _readFileService.ConvertXmlToObject<Person>(xmlContent);
                return View("Index", xmlData);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Lỗi: " + ex.Message;
                return View("Index");
            }
        }

        [HttpPost]
        public IActionResult SaveToDatabase(string xmlData)
        {
            try
            {
               
                var xmldataList = _readFileService.ConvertXmlToObject<Xmldatum>(xmlData);
                _context.Xmldata.AddRange(xmldataList);
                _context.SaveChanges();

                ViewBag.SuccessMessage = "Lưu dữ liệu thành công";
                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Lỗi:" + ex.Message;
                return View("Index");
            }
        }

        [HttpGet]
        public IActionResult GenerateSampleXml()
        {
            try
            {
                var sampleObject = new Person();
                string sampleXml = _readFileService.GenerateSampleXml(sampleObject);
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(sampleXml);
                return File(bytes, "application/xml", "sample.xml");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Lỗi:" + ex.Message;
                return View("Index");
            }
        }
    }
}


