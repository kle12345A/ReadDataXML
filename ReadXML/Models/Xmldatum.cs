using System;
using System.Collections.Generic;

namespace ReadXML.Models
{
    public partial class Xmldatum
    {
        public int Id { get; set; }
        public string? Author { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public double? Price { get; set; }
        public DateTime? PublishDate { get; set; }
    }
}
