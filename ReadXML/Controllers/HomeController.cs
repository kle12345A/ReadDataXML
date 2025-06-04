using Microsoft.AspNetCore.Mvc;
using ReadXML.Models;
using System.Diagnostics;
using System.Xml.Linq;
using System.Text;

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
            return View();
        }

       
        [HttpPost]
        public IActionResult ReadXML(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return View("Index", new List<string>());
                }

                using var stream = file.OpenReadStream();
                var xmlDoc = XDocument.Load(stream);


                var xmlString = xmlDoc.ToString();
                ViewBag.XmlRaw = Convert.ToBase64String(Encoding.UTF8.GetBytes(xmlString));

                var xmlElements = xmlDoc.Descendants()
                    .Where(e => !e.HasElements)
                    .Select(e => $"{e.Name}: {e.Value.Trim()}")
                    .Where(s => !string.IsNullOrEmpty(s.Split(':')[1].Trim()))
                    .ToList();

                if (!xmlElements.Any())
                {
                    ViewBag.Error = "No data";
                    return View("Index", new List<string>());
                }

                ViewBag.XmlData = xmlElements;
                return View("Index", xmlElements);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading XML file");
                return View("Index", new List<string>());
            }
        }

        [HttpPost]
        public IActionResult SaveToDatabase(string xmlData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(xmlData))
                {
                    ViewBag.Error = "No data to save.";
                    return View("Index");
                }

                var xmlBytes = Convert.FromBase64String(xmlData);
                var xmlString = Encoding.UTF8.GetString(xmlBytes);

                var xmlDoc = XDocument.Parse(xmlString);
                var bookElements = xmlDoc.Descendants("book");

                if (!bookElements.Any())
                {
                    ViewBag.Error = "Not found in XML.";
                    return View("Index");
                }

                var xmlRecords = new List<Xmldatum>();

                foreach (var bookElement in bookElements)
                {
                    try
                    {
                        var record = new Xmldatum
                        {
                            Author = bookElement.Element("author").Value.Trim(),
                            Title = bookElement.Element("title").Value.Trim(),
                            Genre = bookElement.Element("genre").Value.Trim(),
                            Price = double.TryParse(bookElement.Element("price").Value.Trim(), out var price) ? price : null,
                            PublishDate = DateTime.TryParse(bookElement.Element("publish_date").Value.Trim(), out var date) ? date : null
                        };

                        xmlRecords.Add(record);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Error = "Error: " + ex.Message;
                        return View("Index");
                    }
                }

                _context.Xmldata.AddRange(xmlRecords);
                var saveResult = _context.SaveChanges();
                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveToDatabase: {Message}", ex.Message);
                ViewBag.Error = "Error processing XML data: " + ex.Message;
                return View("Index");
            }
        }
    }
}

