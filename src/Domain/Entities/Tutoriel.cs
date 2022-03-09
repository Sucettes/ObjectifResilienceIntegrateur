using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Gwenael.Domain.Entities
{
    public class Tutoriel
    {
        public int Id { get; set; }
        [MaxLength(50)] public string LienImgBanniere { get; set; }
        [Required] [MaxLength(50)] public string Name { get; set; }
        [Required] [MaxLength(50)] public string Titre { get; set; }
        [Required] [MaxLength(2)] public int Difficulte { get; set; }
        [Required] [MaxLength(7)] public double Cout { get; set; }
        [Required] [MaxLength(4)] public int Duree { get; set; }
        [Required] [MaxLength] public IEnumerable<Categorie> Categories { get; set; }
        [Required] public User AuteurUserId { get; set; }
        [Required] [MaxLength(250)] public string References { get; set; }
        [Required] [MaxLength(2000)] public string Introduction { get; set; }
        public IEnumerable<RangeeTutoriel> RangeeTutoriels { get; set; }

        public Tutoriel()
        {
            Categories = new List<Categorie>();
            RangeeTutoriels = new List<RangeeTutoriel>();
        }
    }
}