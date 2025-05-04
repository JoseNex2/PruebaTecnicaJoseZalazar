
using entities.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProyectoEncodeApi.Middleware;
using Prueba_Tecnica.Controllers;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProductsDbContext>(options =>
    options.UseSqlServer(connectionString)
);

builder.Services.AddAuthorization();
builder.Services.AddControllers();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mi API", Version = "v1" });
});


IoC.AddDependencia(builder.Services);

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
    context.Database.Migrate();
}   

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiProduct Encode");
    c.RoutePrefix = string.Empty;
});

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();

