using Hangfire;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using System.Text;
using WebApplication2.BackgroundServices;
using WebApplication2.Models;
using WebApplication2.Repository;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownProxies.Add(IPAddress.Parse("34.224.64.48"));
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddTransient<IRepo<CartItems>, CartItemsRepo>(); 
builder.Services.AddTransient<IRepo<User>, UsersRepo>();

//use in memory database instead of sql database right now 
builder.Services.AddDbContext<ToDoContext>(options => options.UseSqlServer("name=WebApp2"));

//builder.Services.AddHangfire(configuration =>
//{
//    configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
//           .UseColouredConsoleLogProvider()
//           .UseSimpleAssemblyNameTypeSerializer()
//           .UseRecommendedSerializerSettings()
//           .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"));



//}
//);
//builder.Services.AddHangfireServer(); 



//builder.Services.AddDbContext<ToDoContext>(opt =>
//   opt.UseInMemoryDatabase("TodoList"));




builder.Services.AddSwaggerGen(opt =>
{
    // opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a token ",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        //   BearerFormat = "JWT",
        //  Scheme = "bearer"
    });


    //this HAS TO be included 
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Scheme = "http",
            Name = "Bearer",
            In = ParameterLocation.Header,

        },
        new List<string>()
    }
});

});

//the authentciation schema 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddCookie(x => x.Cookie.Name = "token")
    .AddJwtBearer(
       options => {
           options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
        };
        //allows this application to access the Bearer token cookie - allwoing authorization to work 
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["token"];
                return Task.CompletedTask;
            }
        }; 

        }); 
   

builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder =>
    {
        //builder.WithOrigins("http://localhost:4200").
        //AllowAnyMethod().
        //AllowCredentials().
        //AllowAnyHeader();

        // allow all origins to work
        builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();

    });
});


builder.Services.AddHttpContextAccessor();

//the background service 
//builder.Services.AddHostedService<DatabaseBackGround>(); 


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var app = builder.Build();


app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors("EnableCORS");

//app.UseHangfireDashboard(); 

app.Run();
