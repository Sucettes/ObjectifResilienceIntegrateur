using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Gwenael.Domain.Entities
{
    public class RangeeTutoriel
    {
        public Guid Id { get; set; }
        public string Texte { get; set; }
        public string LienImg { get; set; }
        //[Required] [MaxLength(2)] public int Ordre { get; set; }
        [Required] public string PositionImg { get; set; }
        [Required] public Guid TutorielId { get; set; }

        public RangeeTutoriel()
        {
        }
    }
}
