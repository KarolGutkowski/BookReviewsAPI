using Microsoft.Extensions;
using BookReviews.WebAPI.Consts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServivces();

var app = builder.Build();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CorsPolicyConsts.AllowLocalhostClient);

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
