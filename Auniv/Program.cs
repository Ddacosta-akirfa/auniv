using Auniv.Data;
using Auniv.Routes;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();


builder.Services.AddDbContext<AppDbContext>(options =>
    options
        .UseMySql(builder.Configuration.GetConnectionString("auniv_cn"),
            new MySqlServerVersion(new Version(8, 4, 4))));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Documentação da API Auniv");
        options.WithTheme(ScalarTheme.Mars);
    });
}

// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     dbContext.Database.Migrate();
//     DbSeeders.SeedData(dbContext);
// }

app.Routes();

app.UseHttpsRedirection();
app.Run();