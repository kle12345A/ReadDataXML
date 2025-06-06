using System.Xml.Serialization;

namespace ReadXML.Models
{
    public class Person
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public decimal Salary { get; set; }
    }
}
