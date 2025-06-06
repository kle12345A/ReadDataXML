using System.Xml.Serialization;

namespace ReadXML.Models
{
    public class Book
    {
        [XmlElement(IsNullable = true)]
        public string? Tuitle { get; set; }

        [XmlElement(IsNullable = true)]
        public string? Author { get; set; }

        [XmlElement(IsNullable = true)]
        public DateTime? PublishedDate { get; set; }

        [XmlElement(IsNullable = true)]
        public string? ISBN { get; set; }

        [XmlElement]
        public decimal Price { get; set; }

        [XmlElement(IsNullable = true)]
        public string? Genre { get; set; }

        [XmlElement]
        public int Pages { get; set; }

        [XmlElement(IsNullable = true)]
        public string? Publisher { get; set; }

        [XmlElement(IsNullable = true)]
        public string? Language { get; set; }

        [XmlElement(IsNullable = true)]
        public string? Description { get; set; }
    }
}
