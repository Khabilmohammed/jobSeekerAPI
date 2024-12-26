using jobSeeker.DataAccess.Data;
using jobSeeker.DataAccess.Data.Repository.IUserRepository;
using jobSeeker.DataAccess.Services.CloudinaryService;
using jobSeeker.DataAccess.Services.IEmailService;
using jobSeeker.DataAccess.Services.IPostService;
using jobSeeker.DataAccess.Services.ITokenBlacklistService;
using jobSeeker.DataAccess.Services.IUserRepositoryService;
using jobSeeker.DataAccess.Services.IWhetherForCastService;
using jobSeeker.DataAccess.Services.JWTBlackListService;
using jobSeeker.DataAccess.Services.OtpService;
using jobSeeker.GlobalErrorHandler;
using jobSeeker.Models;
using jobSeeker.Models.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Serilog;
using jobSeeker.DataAccess.Services.TokenService;
using jobSeeker.DataAccess.Data.Repository.IUserManagementRepo;
using jobSeeker.DataAccess.Services.IUsermanagemetService;
using jobSeeker.DataAccess.Data.Repository.IStoryRepo;
using jobSeeker.DataAccess.Services.IStoryService;
using jobSeeker.DataAccess.Services.StoryCleanupService;
using jobSeeker.DataAccess.Data.Repository.ILikeRepo;
using jobSeeker.DataAccess.Services.ILikeSerivce;
using jobSeeker.DataAccess.Data.Repository.ICommentRepo;
using jobSeeker.DataAccess.Services.ICommentService;
using jobSeeker.DataAccess.Data.Repository.ISavedRepo;
using jobSeeker.DataAccess.Services.ISavedPostService;
using jobSeeker.DataAccess.Data.Repository.IExpericeRepo;
using jobSeeker.DataAccess.Services.IExperienceService;
using jobSeeker.DataAccess.Data.Repository.CertificateRepo;
using jobSeeker.DataAccess.Services.CertificateService;
using jobSeeker.DataAccess.Data.Repository.IEducationRepo;
using jobSeeker.DataAccess.Services.IEducationService;
using jobSeeker.DataAccess.Data.Repository.ICompanyRepo;
using jobSeeker.DataAccess.Services.ICompanyService;
using jobSeeker.DataAccess.Data.Repository.IJobPostingRepo;
using jobSeeker.DataAccess.Services.IJobPostingService;
using jobSeeker.DataAccess.Services.PymentService;
using jobSeeker.DataAccess.Data.Repository.IJobApplicationRepo;
using jobSeeker.DataAccess.Services.IJobApplicationService;
using jobSeeker.DataAccess.Data.Repository.IFollowRepo;
using jobSeeker.DataAccess.Services.IFollowService;
using jobSeeker.DataAccess.Data.Repository.MessageRepo;
using jobSeeker.SingleR;
using jobSeeker.DataAccess.Data.Repository.IShareRepo;
using jobSeeker.DataAccess.Services.IShareService;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)  
    .Enrich.FromLogContext()
    .WriteTo.Console()  
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day) 
    .CreateLogger();

builder.Host.UseSerilog();
// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDBConnection")));

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 1;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
});
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<CloudinaryServices>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:3000") 
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
    });
});
builder.Services.AddControllers();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) 
            && path.StartsWithSegments("/hubs"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;

        }
    };
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "JobSeeker API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\nEnter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddScoped<IWheatherForcaset, WhetherForcaseExtended>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserManagementRepository, UserManagementRepository>();
builder.Services.AddScoped<IStoryRepository, StoryRepository>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<OTPService>();
builder.Services.AddScoped<PaymentServices>();
builder.Services.AddSingleton<PresenceTracker>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ITokenBlacklistServices, TokenBlacklistService>();
builder.Services.AddScoped<IPostServices, PostServices>();
builder.Services.AddScoped<IStoryServices, StoryServices>();
builder.Services.AddScoped<IEmailservice, Emailservice>();
builder.Services.AddScoped<ILikeRepository, LikeRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ILikeservice, LikeService>();
builder.Services.AddScoped<ICommentservice, Commentservice>();
builder.Services.AddScoped<StoryCleanupServices>();
builder.Services.AddScoped<ISavedPostRepository, SavedPostRepository>();
builder.Services.AddScoped<ISavedPostservices, SavedPostservice>();
builder.Services.AddScoped<IExperienceRepository, ExperienceRepository>();
builder.Services.AddScoped<IExperienceServices, ExperienceService>();
builder.Services.AddScoped<ICertificateRepository, CertificateRepository>();
builder.Services.AddScoped<ICertificateServices, CertificateServices>();
builder.Services.AddScoped<IEducationRepository, EducationRepository>();
builder.Services.AddScoped<IEducationServices, EducationServices>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyServices, CompanyServices>();
builder.Services.AddScoped<IJobPostingRepository, JobPostingRepository>();
builder.Services.AddScoped<IJobPostingServices, JobPostingServices>();
builder.Services.AddScoped<IJobApplicationRepository,JobApplicationRepository >();
builder.Services.AddScoped<IJobApplicationServices, JobApplicationServices>();
builder.Services.AddScoped<IFollowRepository, FollowRepository>();
builder.Services.AddScoped<IFollowServices, FollowServices>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IShareRepository, ShareRepository>();
builder.Services.AddScoped<IShareServices, ShareServices>();
builder.Services.AddHttpContextAccessor();



builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSignalR();
var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Ensure correct middleware order
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

app.Run();

Log.CloseAndFlush();

