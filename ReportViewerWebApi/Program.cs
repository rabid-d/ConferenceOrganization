using AutoMapper;
using Microsoft.OpenApi.Models;
using ReportViewerWebApi.Services;
using ReportViewerWebApi.ViewModels;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "My API - V1",
            Version = "v1"
        }
     );

    string filePath = Path.Combine(AppContext.BaseDirectory, "ReportViewerWebApi.xml");
    c.IncludeXmlComments(filePath);
});
builder.Services.AddTransient<DAL.Services.ConferenceService>();
builder.Services.AddTransient<ÑonferenceService>();
builder.Services.AddTransient<DAL.Services.UserService>();
builder.Services.AddTransient<UserService>();
IMapper mapper = ConfigureAndCreateAutoMapper();
builder.Services.AddSingleton(mapper);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


IMapper ConfigureAndCreateAutoMapper()
{
    MapperConfiguration configuration = new(cfg =>
    {
        cfg.CreateMap<UserViewModel, DAL.Model.AppUser>();
    });

    #if DEBUG
    configuration.AssertConfigurationIsValid();
    #endif

    return configuration.CreateMapper();
}