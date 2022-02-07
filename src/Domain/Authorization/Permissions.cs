using System.Collections.Generic;
using Gwenael.Domain.Entities;

namespace Gwenael.Domain.Authorization
{
    public static class Permissions
    {
        private static readonly string[] Entities =
        {
            nameof(User),
            nameof(Role)
        };

        public static IEnumerable<string> Generate()
        {
            var permissions = new List<string>();

            foreach (var entity in Entities)
            {
                permissions.AddRange(GeneratePermissionsForEntity(entity));
            }

            // Special permissions
            permissions.AddRange(new[]
            {
                "Permissions.View"
            });

            return permissions;
        }

        private static IEnumerable<string> GeneratePermissionsForEntity(string entity)
        {
            return new List<string>
            {
                $"Permissions.{entity}.Create",
                $"Permissions.{entity}.View",
                $"Permissions.{entity}.Edit",
                $"Permissions.{entity}.Delete",
            };
        }
    }
}