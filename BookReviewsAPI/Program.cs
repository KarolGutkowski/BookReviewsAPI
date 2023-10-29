using Microsoft.Extensions;
using BookReviews.WebAPI.Consts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServivces();
builder.Services.AddHealthChecks();

var app = builder.Build();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseHealthChecks("/hc");


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseCors(CorsPolicyConsts.AllowLocalhostClient);
app.UseCors("azure test policy");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
