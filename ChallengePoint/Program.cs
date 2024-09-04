using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using ChallengePoint.API.Swagger;
using ChallengePoint.Application.Mapper;
using ChallengePoint.Domain.Interface;
using ChallengePoint.Infra.Data.Context;
using ChallengePoint.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

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

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<SwaggerDefaultValues>();
});

builder.Services.AddAutoMapper(typeof(CollaboratorDomainToDTO));
builder.Services.AddAutoMapper(typeof(TimekeepingDomainToDTO));

builder.Services.AddTransient<IPointRepository, PointRepository>();
builder.Services.AddTransient<ICollaboratorRepository, CollaboratorRepository>();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

var versionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in versionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                $"ChallengePoint APi - {description.GroupName.ToUpper()}");
        }
    });
}

app.UseHttpsRedirection();

// Adiciona o middleware CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
