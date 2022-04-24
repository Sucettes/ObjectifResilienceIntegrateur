using System;
using System.ComponentModel.DataAnnotations;

namespace Gwenael.Domain.Entities
{
    public class Tutos
    {
        public Guid Id { get; set; }
        [Required][MaxLength(50)] public string Titre { get; set; }
        public DateTime DateCreation { get; set; }
        [Required][MaxLength(2)] public int Difficulte { get; set; }
        [Required][MaxLength(7)] public double Cout { get; set; }
        [Required][MaxLength(4)] public int Duree { get; set; }
        [Required] public CategoriesTutos Categorie { get; set; }
        [Required] public string Introduction { get; set; }
        public string Materiels { get; set; }
        public bool EstPublier { get; set; }
        public string LienImgBanniere { get; set; }
        public User AuteurUserId { get; set; }


        public Tutos()
        {
            DateCreation = DateTime.UtcNow;
            EstPublier = false;
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