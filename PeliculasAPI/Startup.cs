using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Servicios;

namespace PeliculasAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            

            services.AddControllers()
                        .AddNewtonsoftJson();

            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //Alamcenamiento de imagenes en AzureStorage
            //services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivoAzure>();
            services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivoLocal>();
            services.AddHttpContextAccessor();

            services.AddEndpointsApiExplorer();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();
            //app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
