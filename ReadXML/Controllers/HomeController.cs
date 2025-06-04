using Microsoft.AspNetCore.Mvc;
using ReadXML.Models;
using System.Diagnostics;
using System.Xml.Linq;
using System.Text;
using System.Xml;

namespace ReadXML.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ReadXMLContext _context;

        public HomeController(ILogger<HomeController> logger, ReadXMLContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(new List<Xmldatum>());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult ReadXML(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    ViewBag.ErrorMessage = "Please select a file to upload.";
                    return View("Index", new List<Xmldatum>());
                }

                using var stream = file.OpenReadStream();
                var xmlDoc = XDocument.Load(stream);
                ViewBag.XmlRaw = xmlDoc.ToString();

                var bookElements = xmlDoc.Descendants("book").Select(b => new Xmldatum
                {
                    Author = b.Element("author")?.Value?.Trim(),
                    Title = b.Element("title")?.Value?.Trim(),
                    Genre = b.Element("genre")?.Value?.Trim(),
                    Price = double.TryParse(b.Element("price")?.Value?.Trim(), out var price) ? price : (double?)null,
                    PublishDate = DateTime.TryParse(b.Element("publish_date")?.Value?.Trim(), out var date) ? date : (DateTime?)null
                }).ToList();

                return View("Index", bookElements);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error" + ex.Message;
                return View("Index", new List<Xmldatum>());
            }
        }

        [HttpPost]
        public IActionResult SaveToDatabase(string xmlData)
        {
            try
            {
                if (string.IsNullOrEmpty(xmlData))
                {
                    ViewBag.ErrorMessage = "No data to save.";
                    return View("Index", new List<Xmldatum>());
                }

                var xmlDoc = XDocument.Parse(xmlData);
                var bookElements = xmlDoc.Descendants("book").Select(b => new Xmldatum
                {
                    Author = b.Element("author")?.Value?.Trim(),
                    Title = b.Element("title")?.Value?.Trim(),
                    Genre = b.Element("genre")?.Value?.Trim(),
                    Price = double.TryParse(b.Element("price")?.Value?.Trim(), out var price) ? price : (double?)null,
                    PublishDate = DateTime.TryParse(b.Element("publish_date")?.Value?.Trim(), out var date) ? date : (DateTime?)null
                }).ToList();
                _context.Xmldata.AddRange(bookElements);
                var result = _context.SaveChanges();
                ViewBag.SuccessMessage = "Insert successfull";

                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error saving to database";
                return View("Index", new List<Xmldatum>());
            }
        }
    }
}


