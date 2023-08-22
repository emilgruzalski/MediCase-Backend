using MediCase.WebAPI.Entities.Admin;
using MediCase.WebAPI.Entities.Content;
using MediCase.WebAPI.Entities.Moderator;
using System.Data;
using System.Drawing.Printing;

namespace MediCase.WebAPI
{
    public class Seeder
    {
        private readonly MediCaseAdminContext _dbAdminContext;
        private readonly MediCaseModeratorContext _dbModeratorContext;
        private readonly MediCaseContentContext _dbContentContext;

        public Seeder(MediCaseAdminContext dbAdminContext, MediCaseModeratorContext dbModeratorContext, MediCaseContentContext dbContentContext)
        {
            _dbAdminContext = dbAdminContext;
            _dbModeratorContext = dbModeratorContext;
            _dbContentContext = dbContentContext;
        }

        public void Seed()
        {
            _dbAdminContext.Database.EnsureCreated();
            _dbContentContext.Database.EnsureCreated();
            _dbModeratorContext.Database.EnsureCreated();
        }
    }
}
