using BookReviewsAPI.Models;
using BookReviewsAPI.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IBookRepository, BookRepository>();
builder.Services.AddApiVersioning(options =>
{
    //options.AssumeDefaultVersionWhenUnspecified = true;
    //options.DefaultApiVersion = ApiVersion.Default;
    options.ReportApiVersions = true;
});
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BookReviewsDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
