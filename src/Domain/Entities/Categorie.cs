using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Gwenael.Domain.Entities
{
    public class Categorie
    {
        [Required] public int Id { get; set; }
        [Required] [MaxLength(50)] public string Nom { get; set; }
        [Required] [MaxLength(250)] public string Description { get; set; }

        public Categorie()
        {

        }
    }
}
