using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReportViewerWebApi.Services;
using ReportViewerWebApi.ViewModels;
using ReportViewerWebApi.ViewModels.ConferenceEditor;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "My API - V1",
            Version = "v1"
        }
     );

    string filePath = Path.Combine(AppContext.BaseDirectory, "ReportViewerWebApi.xml");
    options.IncludeXmlComments(filePath);

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
});
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddTransient<DAL.Services.ConferenceService>();
builder.Services.AddTransient<ÑonferenceService>();
builder.Services.AddTransient<DAL.Services.UserService>();
builder.Services.AddTransient<UserService>();
IMapper mapper = ConfigureAndCreateAutoMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddSingleton(builder.Configuration);
byte[] key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWT:Key"));
builder
    .Services
    .AddAuthentication(auth =>
    {
        auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(token =>
    {
        token.RequireHttpsMetadata = false;
        token.SaveToken = true;
        token.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration.GetValue<string>("JWT:Issuer"),
            ValidateAudience = true,
            ValidAudience = builder.Configuration.GetValue<string>("JWT:Audience"),
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.EnableTryItOutByDefault();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


IMapper ConfigureAndCreateAutoMapper()
{
    MapperConfiguration configuration = new(cfg =>
    {
        cfg
            .CreateMap<UserViewModel, DAL.Model.AppUser>()
            .ForMember(u => u.AppUserId, opt => opt.Ignore())
            .ForMember(u => u.ModifiedBy, opt => opt.Ignore())
            .ForMember(u => u.ModifiedDate, opt => opt.Ignore())
            .ForMember(u => u.CreatedBy, opt => opt.Ignore())
            .ForMember(u => u.CreatedDate, opt => opt.Ignore());
        cfg.CreateMap<DAL.Model.Conference, ConferenceEditorViewModel>();
        cfg.CreateMap<ConferenceEditorViewModel, DAL.Model.Conference>()
            .ForMember(u => u.Sections, opt => opt.Ignore())
            .ForMember(u => u.ModifiedBy, opt => opt.Ignore())
            .ForMember(u => u.ModifiedDate, opt => opt.Ignore())
            .ForMember(u => u.CreatedBy, opt => opt.Ignore())
            .ForMember(u => u.CreatedDate, opt => opt.Ignore());
        cfg.CreateMap<ConferenceViewModel, DAL.Model.Conference>()
            .ForMember(u => u.ConferenceId, opt => opt.Ignore())
            .ForMember(u => u.Sections, opt => opt.Ignore())
            .ForMember(u => u.ModifiedBy, opt => opt.Ignore())
            .ForMember(u => u.ModifiedDate, opt => opt.Ignore())
            .ForMember(u => u.CreatedBy, opt => opt.Ignore())
            .ForMember(u => u.CreatedDate, opt => opt.Ignore());
    });

    #if DEBUG
    configuration.AssertConfigurationIsValid();
    #endif

    return configuration.CreateMapper();
}