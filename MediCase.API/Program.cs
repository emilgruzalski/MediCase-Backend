using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Quartz;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediCase.API.Services.Interfaces;
using MediCase.API.Repositories.Moderator.Interfaces;
using MediCase.API.Models.User.Validators;
using MediCase.API.Services;
using MediCase.API.Repositories.Content;
using MediCase.API.Repositories.Content.Interfaces;
using MediCase.API.Repositories.Interfaces;
using MediCase.API.Repositories.Moderator;
using MediCase.API.Middleware;
using MediCase.API.Models.Account;
using MediCase.API.Models.Group;
using MediCase.API.Models.User;
using MediCase.API;
using MediCase.API.Entities.Moderator;
using MediCase.API.Models.Group.Validators;
using MediCase.API.Models.Account.Validators;
using MediCase.API.Entities.Content;
using MediCase.API.Jobs;
using MediCase.API.Repositories;
using MediCase.API.Entities.Admin;

// Early init of NLog to allow startup and exception logging, before host is built
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    var authenticationSettings = new AuthenticationSettings();

    builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

    builder.Services.AddSingleton(authenticationSettings);
    builder.Services.AddAuthentication(option =>
    {
        option.DefaultAuthenticateScheme = "Bearer";
        option.DefaultScheme = "Bearer";
        option.DefaultChallengeScheme = "Bearer";
    }).AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = authenticationSettings.JwtIssuer,
            ValidAudience = authenticationSettings.JwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
        };
    });

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

    builder.Services.AddFluentValidationAutoValidation();

    builder.Services.AddHttpContextAccessor();

    // Add middleware
    builder.Services.AddScoped<ErrorHandlingMiddleware>();
    builder.Services.AddScoped<RequestTimeMiddleware>();

    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IGroupService, GroupService>();
    builder.Services.AddScoped<IAccountService, AccountService>();

    builder.Services.AddScoped<IFileService, FileService>();
    builder.Services.AddScoped<IModEntityService, ModEntityService>();
    builder.Services.AddScoped<IEntityService, EntityService>();
    builder.Services.AddScoped<ISynchronizationService, SynchronizationService>();
    builder.Services.AddScoped<ITranslationGeneratorService, TranslationGeneratorService>();
    builder.Services.AddScoped<IImageGeneratorService, ImageGeneratorService>();
    builder.Services.AddScoped<IVoiceGeneratorService, VoiceGeneratorService>();

    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IGroupRepository, GroupRepository>();

    builder.Services.AddScoped<MediCase.API.Repositories.Moderator.Interfaces.IEntityTypeRepository, MediCase.API.Repositories.Moderator.EntityTypeRepository>();
    builder.Services.AddScoped<MediCase.API.Repositories.Moderator.Interfaces.IEntityLanguageRepository, MediCase.API.Repositories.Moderator.EntityLanguageRepository>();
    builder.Services.AddScoped<MediCase.API.Repositories.Moderator.Interfaces.IEntityRepository, MediCase.API.Repositories.Moderator.EntityRepository>();
    builder.Services.AddScoped<MediCase.API.Repositories.Moderator.Interfaces.IEntityGraphDataRepository, MediCase.API.Repositories.Moderator.EntityGraphDataRepository>();
    builder.Services.AddScoped<MediCase.API.Repositories.Moderator.Interfaces.IEntityTranslationsRepository, MediCase.API.Repositories.Moderator.EntityTranslationsRepository>();
    builder.Services.AddScoped<MediCase.API.Repositories.Moderator.Interfaces.IEntityTranslationFilesRepository, MediCase.API.Repositories.Moderator.EntityTranslationFilesRepository>();
    builder.Services.AddScoped<IModeratorQueryBucketRepository, ModeratorQueryBucketRepository>();
    builder.Services.AddScoped<IMediCaseModTransactionRepository, MediCaseModTransactionRepository>();

    builder.Services.AddScoped<MediCase.API.Repositories.Content.Interfaces.IEntityTypeRepository, MediCase.API.Repositories.Content.EntityTypeRepository>();
    builder.Services.AddScoped<MediCase.API.Repositories.Content.Interfaces.IEntityLanguageRepository, MediCase.API.Repositories.Content.EntityLanguageRepository>();
    builder.Services.AddScoped<MediCase.API.Repositories.Content.Interfaces.IEntityRepository, MediCase.API.Repositories.Content.EntityRepository>();
    builder.Services.AddScoped<MediCase.API.Repositories.Content.Interfaces.IEntityGraphDataRepository, MediCase.API.Repositories.Content.EntityGraphDataRepository>();
    builder.Services.AddScoped<MediCase.API.Repositories.Content.Interfaces.IEntityTranslationsRepository, MediCase.API.Repositories.Content.EntityTranslationsRepository>();
    builder.Services.AddScoped<MediCase.API.Repositories.Content.Interfaces.IEntityTranslationFilesRepository, MediCase.API.Repositories.Content.EntityTranslationFilesRepository>();
    builder.Services.AddScoped<ISynchronizationRepository, SynchronizationRepository>();
    builder.Services.AddScoped<IMediCaseTransactionRepository, MediCaseTransactionRepository>();

    builder.Services.AddScoped<IValidator<UserDto>, UserDtoValidator>();
    builder.Services.AddScoped<IValidator<UserNameDto>, UserNameDtoValidator>();
    builder.Services.AddScoped<IValidator<UserPasswordDto>, UserPasswordDtoValidator>();
    builder.Services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
    builder.Services.AddScoped<IValidator<UpdateEmailDto>, UpdateEmailDtoValidator>();
    builder.Services.AddScoped<IValidator<UpdatePasswordDto>, UpdatePasswordDtoValidator>();
    builder.Services.AddScoped<IValidator<UpdateNameDto>, UpdateNameDtoValidator>();
    builder.Services.AddScoped<IValidator<UserQuery>, UserQueryValidator>();
    builder.Services.AddScoped<IValidator<GroupDto>, GroupDtoValidator>();
    builder.Services.AddScoped<IValidator<GroupDateDto>, GroupDateDtoValidator>();
    builder.Services.AddScoped<IValidator<GroupNameDto>, GroupNameDtoValidator>();
    builder.Services.AddScoped<IValidator<GroupDescDto>, GroupDescDtoValidator>();
    builder.Services.AddScoped<IValidator<GroupQuery>, GroupQueryValidator>();

    builder.Services.AddScoped<Seeder>();

    builder.Services.AddCors(opts =>
    {
        opts.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
        });
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c => c.UseDateOnlyTimeOnlyStringConverters());

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
        seeder.Seed();
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<RequestTimeMiddleware>();

    app.UseAuthentication();

    app.UseHttpsRedirection();

    app.UseCors();

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