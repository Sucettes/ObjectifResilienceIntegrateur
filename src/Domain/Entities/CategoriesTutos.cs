using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Gwenael.Domain.Entities
{
    public class CategoriesTutos
    {
        public Guid Id { get; set; }
        [Required] [MaxLength(50)] public string Nom { get; set; }
        [Required] [MaxLength(100)] public string Description { get; set; }

        public CategoriesTutos()
        {

        }

        public bool EstValide()
        {
            if (!String.IsNullOrEmpty(Nom) && Nom.Length <= 50)
            {
                if (!String.IsNullOrEmpty(Description) && Description.Length <= 100)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
