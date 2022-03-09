using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Gwenael.Domain.Entities
{
    public class RangeeTutoriel
    {
        public int Id { get; set; }
        public string Texte { get; set; }
        [MaxLength(50)] public string LienImg { get; set; }
        [Required] [MaxLength(2)] public int Ordre { get; set; }

        public RangeeTutoriel()
        {

        }
    }
}
