using kairosApp.Domain.Persistence.Contexts;
using kairosApp.Domain.Persistence.Repositories;
using kairosApp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using kairosApp.Domain.Services;
using kairosApp.Services;
using kairosApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddJWTTokenServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    
});

//SERVICES
builder.Services.AddScoped<IPersonaService, PersonaService>();
builder.Services.AddScoped<ICuentaUsuarioService, CuentaUsuarioService>();
builder.Services.AddScoped<IGrupoService, GrupoService>();
builder.Services.AddScoped<ISolicitudService, SolicitudService>();
builder.Services.AddScoped<IUsuarioGrupoService, UsuarioGrupoService>();
builder.Services.AddScoped<IActiveDirectoryService, ActiveDirectoryService>();

//REPOSITORIES
builder.Services.AddScoped<IPersonaRepository, PersonaRepository>();
builder.Services.AddScoped<ICuentaUsuarioRepository, CuentaUsuarioRepository>();
builder.Services.AddScoped<IGrupoRepository, GrupoRepository>();
builder.Services.AddScoped<IUsuarioGrupoRepository, UsuarioGrupoRepository>();
builder.Services.AddScoped<ISolicitudRepository, SolicitudRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(options =>
{
    options.WithOrigins("http://localhost:3000");
    options.AllowAnyHeader();
    options.AllowAnyMethod();
});
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
