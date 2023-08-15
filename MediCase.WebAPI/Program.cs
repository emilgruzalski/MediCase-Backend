using MediCase.WebAPI.Entities.Admin;
using MediCase.WebAPI.Entities.Content;
using MediCase.WebAPI.Entities.Moderator;
using MediCase.WebAPI.Jobs;
using MediCase.WebAPI.Middleware;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Quartz;
using System.Reflection;

// Early init of NLog to allow startup and exception logging, before host is built
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddDbContext<MediCaseAdminContext>(options =>
        options.UseMySql(builder.Configuration.GetConnectionString("Admin"), ServerVersion.Parse("10.6.14-mariadb")));

    builder.Services.AddDbContext<MediCaseModeratorContext>(options => 
        options.UseMySql(builder.Configuration.GetConnectionString("Moderator"), ServerVersion.Parse("10.6.14-mariadb")));

    builder.Services.AddDbContext<MediCaseContentContext>(options =>
        options.UseMySql(builder.Configuration.GetConnectionString("Content"), ServerVersion.Parse("10.6.14-mariadb")));

    // Add Quartz services
    builder.Services.AddQuartz(q =>
    {
        q.UseMicrosoftDependencyInjectionJobFactory();
        var JobKey = new JobKey("DeleteOutdatedEntitiesJob");
        q.AddJob<DeleteOutdatedEntitiesJob>(opts => opts.WithIdentity(JobKey));

        q.AddTrigger(opts => opts
            .ForJob(JobKey)
            .WithIdentity("DeleteOutdatedEntitiesJob-trigger")
            // Fire at 00:00:00 every day
            .WithCronSchedule("0 0 0 * * ?")
        );

        q.AddTrigger(opts => opts
            .ForJob(JobKey)
            .WithIdentity("DeleteOutdatedEntitiesJob-trigger2")
            .StartNow()
        );
    });
    builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

    builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddDateOnlyTimeOnlyStringConverters();

    // Add AutoMapper to the container.
    builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

    // Add middleware
    builder.Services.AddScoped<ErrorHandlingMiddleware>();
    builder.Services.AddScoped<RequestTimeMiddleware>();

    builder.Services.AddScoped<MediCase.WebAPI.Repositories.Moderator.Interfaces.IEntityTypeRepository, MediCase.WebAPI.Repositories.Moderator.EntityTypeRepository>();
    builder.Services.AddScoped<MediCase.WebAPI.Repositories.Moderator.Interfaces.IEntityLanguageRepository, MediCase.WebAPI.Repositories.Moderator.EntityLanguageRepository>();
    builder.Services.AddScoped<MediCase.WebAPI.Repositories.Moderator.Interfaces.IEntityRepository, MediCase.WebAPI.Repositories.Moderator.EntityRepository>();
    builder.Services.AddScoped<MediCase.WebAPI.Repositories.Moderator.Interfaces.IEntityGraphDataRepository, MediCase.WebAPI.Repositories.Moderator.EntityGraphDataRepository>();
    builder.Services.AddScoped<MediCase.WebAPI.Repositories.Moderator.Interfaces.IEntityTranslationsRepository, MediCase.WebAPI.Repositories.Moderator.EntityTranslationsRepository>();
    builder.Services.AddScoped<MediCase.WebAPI.Repositories.Moderator.Interfaces.IEntityTranslationFilesRepository, MediCase.WebAPI.Repositories.Moderator.EntityTranslationFilesRepository>();
    builder.Services.AddScoped<MediCase.WebAPI.Repositories.Moderator.Interfaces.IModeratorQueryBucketRepository, MediCase.WebAPI.Repositories.Moderator.ModeratorQueryBucketRepository>();
    builder.Services.AddScoped<MediCase.WebAPI.Repositories.Moderator.Interfaces.IMediCaseModTransactionRepository, MediCase.WebAPI.Repositories.Moderator.MediCaseModTransactionRepository>();

    builder.Services.AddScoped<MediCase.WebAPI.Repositories.Content.Interfaces.IEntityTypeRepository, MediCase.WebAPI.Repositories.Content.EntityTypeRepository>();
    builder.Services.AddScoped<MediCase.WebAPI.Repositories.Content.Interfaces.IEntityLanguageRepository, MediCase.WebAPI.Repositories.Content.EntityLanguageRepository>();
    builder.Services.AddScoped<MediCase.WebAPI.Repositories.Content.Interfaces.IEntityRepository, MediCase.WebAPI.Repositories.Content.EntityRepository>();
    builder.Services.AddScoped<MediCase.WebAPI.Repositories.Content.Interfaces.IEntityGraphDataRepository, MediCase.WebAPI.Repositories.Content.EntityGraphDataRepository>();
    builder.Services.AddScoped<MediCase.WebAPI.Repositories.Content.Interfaces.IEntityTranslationsRepository, MediCase.WebAPI.Repositories.Content.EntityTranslationsRepository>();
    builder.Services.AddScoped<MediCase.WebAPI.Repositories.Content.Interfaces.IEntityTranslationFilesRepository, MediCase.WebAPI.Repositories.Content.EntityTranslationFilesRepository>();
    builder.Services.AddScoped<MediCase.WebAPI.Repositories.Content.Interfaces.ISynchronizationRepository, MediCase.WebAPI.Repositories.Content.SynchronizationRepository>();
    builder.Services.AddScoped<MediCase.WebAPI.Repositories.Content.Interfaces.IMediCaseTransactionRepository, MediCase.WebAPI.Repositories.Content.MediCaseTransactionRepository>();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c => c.UseDateOnlyTimeOnlyStringConverters());

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<RequestTimeMiddleware>();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    //NLog: catch setup errors
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}