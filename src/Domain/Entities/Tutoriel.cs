using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Gwenael.Domain.Entities
{
    public class Tutoriel
    {
        public Guid Id { get; set; }
        [Required] [MaxLength(50)] public string Titre { get; set; }
        public DateTime DateCreation { get; set; }
        [Required] [MaxLength(2)] public int Difficulte { get; set; }
        [Required] [MaxLength(7)] public double Cout { get; set; }
        [Required] [MaxLength(4)] public int Duree { get; set; }
        [Required] public Categorie Categorie { get; set; }
        // TODO : Faire en sorte que nous devons être connecter...
        [Required] public string Introduction { get; set; }
        public bool EstPublier { get; set; }
        [MaxLength(50)] public string LienImgBanniere { get; set; }
        public User AuteurUserId { get; set; }

        //public IEnumerable<RangeeTutoriel> RangeeTutoriels { get; set; }

        public Tutoriel()
        {
            DateCreation = DateTime.UtcNow;
            EstPublier = false;
            //RangeeTutoriels = new List<RangeeTutoriel>();
        }

        public bool EstValide()
        {
            if (!String.IsNullOrEmpty(Titre) && Titre.Length <= 50)
            {
                if (Difficulte is >= 0 and <= 10)
                {
                    if (Cout is >= 0 and <= 1000000)
                    {
                        if (Duree is >= 0 and < 10000)
                        {
                            if (Categorie != null && Introduction != null)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}