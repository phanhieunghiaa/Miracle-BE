
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Micracle.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repositories;
using Repositories.Data;
using Repositories.Interface;
using Services;
using Services.Helpers;
using Services.Interface;
using System.Text;
using Net.payOS;

namespace Micracle
{
    public class Program
    {
        public static void Main(string[] args)
        {



            //PayOS============================================================PayOS

            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            PayOS payOS = new PayOS(configuration["Environment:PAYOS_CLIENT_ID"] ?? throw new Exception("Cannot find environment"),
                                configuration["Environment:PAYOS_API_KEY"] ?? throw new Exception("Cannot find environment"),
                                configuration["Environment:PAYOS_CHECKSUM_KEY"] ?? throw new Exception("Cannot find environment"));

            //PayOS============================================================PayOS




            var builder = WebApplication.CreateBuilder(args);


            // Lấy đường dẫn tương đối đến file JSON từ thư mục gốc của project
            var pathToKey = Path.Combine(Directory.GetCurrentDirectory(), "miracles-ef238-firebase-adminsdk-mm7s5-0c76f3bec8.json");

            // Khởi tạo Firebase Admin SDK
            /*FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(pathToKey)
            });*/
            //Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        Array.Empty<string>()
                    }
                });
            });


            //This if for front end connection
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:5000", "https://miracle-ashen.vercel.app")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            //PayOS Services
            builder.Services.AddSingleton(payOS);
            //PayOS Services

            builder.Services.AddScoped<IOrderProductServices, OrderProductServices>();
            builder.Services.AddScoped<IOrderProductRepository, OrderProductRepository>();

            builder.Services.AddScoped<ICardServices, CardServices>();
            builder.Services.AddScoped<ICardRepositories, CardRepository>();

            builder.Services.AddScoped<ICategoryServices, CategoryServices>();
            builder.Services.AddScoped<ICategoryRepositories, CategoryRepository>();

            builder.Services.AddScoped<ISubCategoryServices, SubCategoriesServices>();
            builder.Services.AddScoped<ISubCategoriesRepository, SubCategoryRepository>();

            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<ICartProductService, CartProductService>();
            builder.Services.AddScoped<ICartProductRepository, CartProductRepository>();

            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<ICartService, CartService>();

            builder.Services.AddScoped<IImagesServices, ImageServices>();
            builder.Services.AddScoped<IImagesRepository, ImagesRepository>();


            builder.Services.AddScoped<IProductImagesRepository, ProductImagesRepository>();
            builder.Services.AddScoped<IProductImagesServices, ProductImageServices>();

            builder.Services.AddScoped<IOrderServices, OrderServices>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();

            builder.Services.AddScoped<IPaymentServices, PaymentServices>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();


            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<VerificationCodeManager>();
            builder.Services.AddSingleton<JwtTokenHelper>();

            builder.Services.AddScoped<IEmailServices, EmailServices>();
            builder.Services.AddScoped<EmailServices>();

            //SQL
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        RoleClaimType = "Role"  // Đảm bảo claim "Role" được nhận diện đúng

                    };
                });

            builder.Services.AddAuthorization(options =>
            {
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI(c=>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dispatch API V1");
                });
            //}

            app.UseHttpsRedirection();
            app.UseCors("AllowSpecificOrigins");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
