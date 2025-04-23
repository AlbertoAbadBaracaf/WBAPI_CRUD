using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WBAPI_CRUD.WBAPI_CRUD_Mappers;
using WBAPI_CRUD.Data;
using WBAPI_CRUD.Repositorio;
using WBAPI_CRUD.Repositorio.IRepositorio;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql")));

builder.Services.AddScoped<ICatUserApplicationsRepositorio, CatUserApplicationsRepositorio>();
builder.Services.AddScoped<IlogWebAppWBAPI_CRUDRepositorio, logWebAppWBAPI_CRUDRepositorio>();
builder.Services.AddScoped<IUtilsRepositorio, UtilsRepositorio>();

var key = builder.Configuration.GetValue<string>("ApiSettings:Secreta");

builder.Services.AddAutoMapper(typeof(WBAPI_CRUD_Mappers));

builder.Services.AddAuthentication(
    options => {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }
).AddJwtBearer(
    options => {
        options.RequireHttpsMetadata = false;//pasar a true en prod
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = false,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }
    );


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
app.MapOpenApi();

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
