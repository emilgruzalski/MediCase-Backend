using MediCase.API.Entities.Admin;
using Quartz;

namespace MediCase.API.Jobs
{
    public class DeleteOutdatedEntitiesJob : IJob
    {
        private readonly MediCaseAdminContext _dbContext;

        public DeleteOutdatedEntitiesJob(MediCaseAdminContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            var outdatedGroups = _dbContext.Groups.Where(e => e.ExpirationDate < currentDate).ToList();
            _dbContext.Groups.RemoveRange(outdatedGroups);

            _dbContext.SaveChanges();

            return Task.CompletedTask;
        }
    }
}
