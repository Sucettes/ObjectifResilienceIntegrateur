using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gwenael.Domain.Entities
{
    public class Poadcast
    {
        [Required] public string ID { get; set; }

        [Required] public string titre { get; set; }

        [Required] public string description { get; set; }

        [Required] public string categorie { get; set; }

        [Required] public string url { get; set; }
    }
}
