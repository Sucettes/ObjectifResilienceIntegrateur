using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gwenael.Domain.Entities
{
    [Index(nameof(FirstName), IsUnique = false)]
    [Index(nameof(LastName), IsUnique = false)]
    public class User : IdentityUser<Guid>
    {
        [Required] [MaxLength(50)] public string FirstName { get; set; }
        [Required] [MaxLength(50)] public string LastName { get; set; }
        public bool Active { get; set; }
        public virtual IEnumerable<UserRole> Roles { get; set; }
        public DateTime CreationDate { get; protected set; }
        public DateTime? LastUpdateDate { get; protected set; }

        public User()
        {
            CreationDate = DateTime.UtcNow;
        }

        public void Updated()
        {
            LastUpdateDate = DateTime.UtcNow;
        }

        public string[] Validate()
        {
            var errors = new List<string>();
            var mustNotBeNullFields = new[]
            {
                nameof(UserName),
                nameof(FirstName),
                nameof(LastName),
            };

            var mustNotExceedLength = new[]
            {
                nameof(FirstName),
                nameof(LastName),
            };

            foreach (var field in mustNotBeNullFields)
            {
                if (string.IsNullOrEmpty(GetType().GetProperty(field)?.GetValue(this)?.ToString()))
                {
                    errors.Add($"Required{field}");
                }
            }

            foreach (var field in mustNotExceedLength)
            {
                var prop = GetType().GetProperty(field);
                var maxLengthAttribute = prop?.GetCustomAttributes(typeof(MaxLengthAttribute), false)
                    .FirstOrDefault() as MaxLengthAttribute;

                if (prop?.GetValue(this)?.ToString()?.Length > maxLengthAttribute?.Length)
                {
                    errors.Add($"ExceededMaxLength{field}");
                }
            }

            return errors.ToArray();
        }
        
        public IEnumerable<string> GetRoles()
        {
            return Roles.Select(x => x.Role.Name);
        }
        
        public IEnumerable<string> GetPermissions()
        {
            return Roles.SelectMany(x => x.Role.GetClaims());
        }
    }
}