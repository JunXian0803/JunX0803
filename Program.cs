using Logic;
using Microsoft.Extensions.FileProviders;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connection_string = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddSingleton(new Logic.Account(connection_string));
builder.Services.AddSingleton(new Logic.AccountProfile(connection_string));
builder.Services.AddSingleton(new Logic.AccountVerification(connection_string));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();


app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();


app.Run();
