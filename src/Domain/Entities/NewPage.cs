using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Gwenael.Domain.Entities
{
    public class NewPage
    {
        public int Id { get; set; }
        [Required] [MaxLength(50)] public string Titre { get; set; }
        [MaxLength(20000)] public string InerText { get; set; }
        public string LienImg { get; set; }
        public bool EstPublier { get; set; }

    }
}