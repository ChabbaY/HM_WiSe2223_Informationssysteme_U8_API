using System;
using System.IO;
using System.Reflection;
using API.Store;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();
            services.AddMvc();

            services.AddDbContext<Context>(options => {
                options.UseInMemoryDatabase("InMemoryDb");
            });

            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v0", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My API", Version = "v0" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Context context) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/v0/swagger.json", "My API V0");
            });

            //Initialize Data
            context.AddressInformation.Add(new DataObject.AddressInformation() {//1
                Title = "Mr",
                Name = "Bean"
            });
            context.AddressInformation.Add(new DataObject.AddressInformation() {//2
                Title = "Mr",
                Name = "Torsten Zimmer",
                Email = "torsten.zimmer@hm.edu"
            });
            context.Addresses.Add(new DataObject.Address() {//1
                AdderssInformationId = 1,
                Street = "Main Avenue",
                House_Nr = "1",
                Postcode = "12345",
                City = "City",
                Country = "Germany",
                Region  ="Bavaria",
                Timezone = "UTC+1"
            });
            context.Addresses.Add(new DataObject.Address() {//2
                AdderssInformationId = 2,
                Street = "Main Avenue",
                House_Nr = "1",
                Postcode = "67890",
                City = "London",
                Country = "England"
            });
            context.Customers.Add(new DataObject.Customer() {//1
                AddressInformationId=1
            });
            context.Contacts.Add(new DataObject.Contact() {//1
                AddressInformationId=2
            });
            context.Relations.Add(new DataObject.Relation() {//1
                CustomerId=1,
                ContactId=1,
                Comment="nothing to comment on"
            });
            context.Requests.Add(new DataObject.Request() {//1
                CustomerId=1,
                FromDate="2022/11/10",
                UntilDate="2022/12/10"
            });
            context.Materials.Add(new DataObject.Material() {//1
                Name = "Iron",
                Price = 5
            });
            context.Positions.Add(new DataObject.Position() {//1
                RequestId = 1,
                MaterialId = 1,
                Pos = 1,
                Count = 20
            });
            context.SaveChanges();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
