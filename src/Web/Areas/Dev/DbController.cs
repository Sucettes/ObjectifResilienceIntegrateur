using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spk.Common.Helpers.Guard;
using Gwenael.Domain;
using Gwenael.Domain.Authorization;
using Gwenael.Domain.Entities;
using Gwenael.Domain.Fakers;
using Gwenael.Domain.Stores;
using Gwenael.Web.Extensions;
using System.Text;
using System.Threading.Tasks;

namespace Gwenael.Web.Areas.Dev
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Area(AreaNames.Dev)]
    public class DbController : Controller
    {
        private readonly GwenaelDbContext _db;
        private readonly PersistedGrantDbContext _persistedGrantDbContext;
        private readonly GwenaelRoleManager _roleManager;
        private readonly GwenaelUserManager _userManager;

        private StringBuilder _output;

        public DbController(GwenaelDbContext db, PersistedGrantDbContext persistedGrantDbContext,
            GwenaelUserManager userManager, GwenaelRoleManager roleManager)
        {
            _roleManager = roleManager;
            _db = db.GuardIsNotNull(nameof(db));
            _persistedGrantDbContext = persistedGrantDbContext;
            _userManager = userManager.GuardIsNotNull(nameof(userManager));
            _roleManager = roleManager.GuardIsNotNull(nameof(roleManager));
        }

        public IActionResult Migrate()
        {
            _output = new StringBuilder("---PersistedGrantDbContext MIGRATE---\n");
            _persistedGrantDbContext.Database.Migrate();
            _output.AppendLine("---PersistedGrantDbContext MIGRATE COMPLETED-- -\n\n\n");


            _output = _output.AppendLine("---GwenaelDbContext MIGRATE---\n");
            _db.Database.Migrate();
            _output.AppendLine("---DATABASE MIGRATE COMPLETED-- -\n");

            return Ok(_output.ToString());
        }

        public async Task<IActionResult> Seed(bool reset = false)
        {
            _output = new StringBuilder("---DATABASE SEED---\n");

            if (reset)
                await DeleteAllDatabaseObjects();

            await _persistedGrantDbContext.Database.MigrateAsync();
            await _db.Database.MigrateAsync();

            _output.AppendLine("Ensured database is created and applied all pending migrations");


            foreach (var role in Roles.GetAllRoles())
                await CreateRole(role);


            foreach (var superAdmin in SuperAdmins.GetAllSuperAdmins())
            {
                await CreateUser(superAdmin, Constants.Authorization.Roles.SUPER_ADMIN);
                await _roleManager.SeedClaimsForSuperAdmin();
            }

            var userFakerInstance = UserFaker.DefaultInstance();
            foreach (var fakeUser in userFakerInstance.Generate(20))
                await CreateUser(fakeUser, Constants.Authorization.Roles.ADMIN);

            foreach (var fakeUser in userFakerInstance.Generate(80))
                await CreateUser(fakeUser, Constants.Authorization.Roles.USER);

            await _db.SaveChangesAsync();

            _output.AppendLine("Done!");

            return Ok(_output.ToString());
        }

        private async Task CreateRole(Role role)
        {
            await _roleManager.CreateAsync(role);
            _output.AppendLine($"Created role '{role}'");
        }

        private async Task CreateUser(User superAdmin, string roleName, string password = "123qwe")
        {
            await _userManager.CreateAsync(superAdmin);
            await _userManager.AddPasswordAsync(superAdmin, password);
            await _userManager.AddToRoleAsync(superAdmin, roleName);
            _output.AppendLine(
                $"Created user '{superAdmin.UserName}' with password '{password}' and role '{roleName}'");
        }

        private async Task DeleteAllDatabaseObjects()
        {
            await _db.Database.ExecuteSqlRawAsync(@"
                declare @str varchar(max)
                declare cur cursor for

                SELECT 'ALTER TABLE ' + '[' + s.[NAME] + '].[' + t.name + '] DROP CONSTRAINT ['+ c.name + ']'
                FROM sys.objects c, sys.objects t, sys.schemas s
                WHERE c.type IN ('C', 'F', 'PK', 'UQ', 'D')
                 AND c.parent_object_id=t.object_id and t.type='U' AND t.SCHEMA_ID = s.schema_id
                ORDER BY c.type

                open cur
                FETCH NEXT FROM cur INTO @str
                WHILE (@@fetch_status = 0) BEGIN
                 PRINT @str
                 EXEC (@str)
                 FETCH NEXT FROM cur INTO @str
                END

                close cur
                deallocate cur

                EXEC sp_MSforeachtable @command1 = ""DROP TABLE ? ""
            ");
        }
    }
}