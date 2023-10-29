using Microsoft.Extensions;
using BookReviews.WebAPI.Consts;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServivces();
builder.Services.AddHealthChecks();


var app = builder.Build();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseHealthChecks("/hc");

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
