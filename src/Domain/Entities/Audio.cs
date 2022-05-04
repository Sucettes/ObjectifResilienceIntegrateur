using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gwenael.Domain.Entities
{
    public class Audio
    {
        [Required] public Guid ID { get; set; }

        [Required] public string titre { get; set; }

        [Required] public string description { get; set; }

        [Required] public CategoriesTutos categorie { get; set; }

        [Required] public string urlImage { get; set; }

        [Required] public string urlAudio { get; set; }

        public bool EstPublier { get; set; }

        public int nbVue { get; set; }
    }
}
