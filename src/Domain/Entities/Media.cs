using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Gwenael.Domain.Entities
{
    public class Media
    {
        [MaxLength(50)] public string Id { get; set; }
        [Required] public int Idreference { get; set; }
        [Required] [MaxLength(50)] public string LinkS3 { get; set; }
        [Required] public int OrderInThePage { get; set; }
    }
}