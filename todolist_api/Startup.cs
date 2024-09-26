using System.Text;
using System.Text.Unicode;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using todolist_api.Database;
using todolist_api.Models;
using todolist_api.Repositories;
using Task = todolist_api.Models.Task;

namespace todolist_api
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationContext>(options => {
                options.UseNpgsql(connection);
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new () {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["JWT:Issuer"],
                        ValidAudience = Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(UTF8Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
                    };
                });

            services.AddTransient<IBaseRepository<User>, BaseRepository<User>>();
            services.AddTransient<IBaseRepository<Board>, BaseRepository<Board>>();
            services.AddTransient<IBaseRepository<List>, BaseRepository<List>>();
            services.AddTransient<IBaseRepository<Task>, BaseRepository<Task>>();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endPoint => 
            {
                endPoint.MapControllers();
            });
        }
    }
}