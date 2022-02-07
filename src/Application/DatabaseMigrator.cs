using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Spk.Common.Helpers.Guard;
using Gwenael.Domain;
using System.Text;

namespace Gwenael.Application
{
    public class DatabaseMigrator
    {
        private readonly ILogger<DatabaseMigrator> _logger;
        private readonly GwenaelDbContext _db;

        public DatabaseMigrator(ILogger<DatabaseMigrator> logger, GwenaelDbContext db)
        {
            _logger = logger.GuardIsNotNull(nameof(logger));
            _db = db.GuardIsNotNull(nameof(db));
        }

        public string Execute()
        {
            _logger.LogInformation("---DATABASE MIGRATION---\n");
            var output = new StringBuilder("---DATABASE MIGRATION---\n");

            foreach (var migration in _db.Database.GetPendingMigrations())
            {
                _logger.LogInformation(migration);
                output.AppendLine(migration);
            }

            _db.Database.Migrate();

            _logger.LogInformation("Done!");
            return output.AppendLine("Done!").ToString();
        }
    }
}