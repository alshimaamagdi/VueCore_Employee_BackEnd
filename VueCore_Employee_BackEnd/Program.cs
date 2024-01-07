using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RepositoryPatternWithUoW.Core.IUnitOfWork;
using RepositoryPatternWithUoW.Core.Models.AutoMapper;
using RepositoryPatternWithUoW.Core.Models.Domain;
using RepositoryPatternWithUoW.Core.Models.Helper;
using RepositoryPatternWithUoW.EF.Context;
using RepositoryPatternWithUoW.EF.Repository;
using RepositoryPatternWithUoW.EF.UnitOfWork;
using System.Text;
using VueCore_Employee_BackEnd.CustomConfiguration;
using static RepositoryPatternWithUoW.Core.Interface.IGeneric;

var builder = WebApplication.CreateBuilder(args);

// Add builder.Services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(EmployeeMappingProfile));

#region Database Configuration
builder.Services.AddDbContext<ApplicationDbContext>(x => 
x.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("default"),
// Used when have a project (or assembly) that contains your DbContext and another project (or assembly) that contains your migrations.
b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
));
#endregion

#region JWT Configuration

//To Connect JWT in Json With Class JWT
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

builder.Services.AddIdentity<Users, IdentityRole>(option =>
{
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireUppercase = false;
    option.Password.RequireLowercase = false;
    option.Password.RequiredLength = 2;
}).AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 2;
});
builder.Services.AddAuthentication()
// Adding Jwt Bearer  
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});
#endregion
#region Unit Of Work Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
#endregion


#region MiddleWare
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");

        // Enable authorization in Swagger UI
        c.OAuthClientId("swagger");
        c.OAuthClientSecret("secret");
        c.OAuthRealm("your-realm");
        c.OAuthAppName("Swagger UI");
        c.OAuthUsePkce();

    });
}
// Enable CORS globally
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
//app.UseMiddleware<GlobalRoleMiddleware>();
app.MapControllers();
app.Run();
#endregion