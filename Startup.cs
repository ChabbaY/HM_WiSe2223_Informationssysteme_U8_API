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
                options.SchemaFilter<SwaggerSchemaFilter>();
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
            context.Suppliers.Add(new DataObject.Supplier() {//1
                Title = "Mr",
                Name = "Bean"
            });
            context.Suppliers.Add(new DataObject.Supplier() {//2
                Title = "Mr",
                Name = "Torsten Zimmer",
                Email = "torsten.zimmer@hm.edu"
            });
            context.Addresses.Add(new DataObject.Address() {//1
                SupplierId = 2,
                Street = "Lothstraße",
                HouseNumber = "64",
                Postcode = "12345",
                City = "City",
                Country = "Germany",
                Region  ="Bavaria",
                Timezone = "UTC+1"
            });
            context.Addresses.Add(new DataObject.Address() {//2
                SupplierId = 1,
                Street = "Main Avenue",
                HouseNumber = "1",
                Postcode = "67890",
                City = "London",
                Country = "England"
            });
            context.Materials.Add(new DataObject.Material() {//1
                Name = "Iron Ore",
                Type = "Raw Materials"
            });
            context.PurchaseRequisitions.Add(new DataObject.PurchaseRequisition() {//1
                NeededUntil="2022/12/24",
                Comment="Xmas-Present"
            });
            context.Positions.Add(new DataObject.Position() {//1
                PurchaseRequisitionId = 1,
                MaterialId = 1,
                Pos = 1,
                Count = 20,
                Unit = "kg"
            });
            context.Requests.Add(new DataObject.Request() {//1
                PurchaseRequisitionId = 1,
                Date = "2022/12/21",
                Deadline = "2022/12/24",
                Comment = "Xmas-Present!!!"
            });
            context.Offers.Add(new DataObject.Offer() {//1
                SupplierId = 2,
                RequestId = 1,
                Date = "2022/12/22",
                Deadline = "2022/12/24",
                Currency = "EUR",
                Comment = "can be delivered in time",
                PricesSum = 500d//don't change, hidden field will only be updated on data change
                                //and thus needs to be instantiated correctly
            });
            context.PriceInformation.Add(new DataObject.PriceInformation() {//1
                OfferId = 1,
                PositionId = 1,
                UnitPrice = 25,
                Count = 20//don't change, hidden field will only be updated on data change
                          //and thus needs to be instantiated correctly
            });
            context.SaveChanges();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
