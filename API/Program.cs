
using API.Data.SeedData;
using API.Helper;
using API.Middleware;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {            
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v2", new OpenApiInfo { Title = "My Demo", Version = "v2" }));
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Demo", Version = "v1" });

                // Configure Swagger to use JWT Bearer authentication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
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
                                }
                            },
                            Array.Empty<string>()
                        }
                });

                // If you need to include XML comments in Swagger
                // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // c.IncludeXmlComments(xmlPath);
            });
            builder.Services.AddCors();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IPhotoService, PhotoService>();
            builder.Services.AddScoped<ILikesRepository, LikesRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
           
            builder.Services.AddIdentityCore<AppUser>().AddRoles<AppRole>()
                .AddRoleManager<RoleManager<AppRole>>().AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddScoped<LogUserActivity>();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

            builder.Services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefualtConnection"));
            });
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.SaveToken = true;
                option.RequireHttpsMetadata = false; // make true if you need Https
                option.TokenValidationParameters = new TokenValidationParameters()
                {

                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience= builder.Configuration["JWT:ValidAduiance"],
                    IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
                    
                    
                    
                   
                };
            });

            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                opt.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin","Moderator"));


            });
            var app = builder.Build();
            if (builder.Environment.IsDevelopment())
            { 
                app.UseDeveloperExceptionPage();
            }
        // Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionMiddleware>();
            if (app.Environment.IsDevelopment())
            {
               
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            using var scope = app.Services.CreateScope();
            var Services = scope.ServiceProvider;
            var context = Services.GetRequiredService<AppDbContext>();
            var Logger = Services.GetRequiredService<ILogger<Program>>();
            try
            {
                var userManger = Services.GetRequiredService<UserManager<AppUser>>();
                var roleManger = Services.GetRequiredService<RoleManager<AppRole>>();

                await context.Database.MigrateAsync();
                await Seed.SeedUsers(userManger,roleManger);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occur while proccess");
            }

            app.Run();
        }
    }
}