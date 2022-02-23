using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Gwenael.Domain.Entities
{
    public class Formation
    {
        public int Id { get; set; }
        [Required][MaxLength(50)] public string Name { get; set; }
        [MaxLength(50)] public string Description { get; set; }

    }
}