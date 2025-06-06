using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReadXML.Models
{
    public partial class Xmldatum
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public double? Price { get; set; }
        public DateTime? PublishDate { get; set; }
        public string? Title { get; set; }
    }
}
