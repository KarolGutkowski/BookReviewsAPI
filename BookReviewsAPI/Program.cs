using BookReviewsAPI.Models;
using BookReviewsAPI.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using BookReviewsAPI.Authentication.Schemas;
using BookReviewsAPI.Authentication.Policies;
using BookReviewsAPI.Cryptography;
using BookReviewsAPI.Authentication.Helpers;
using BookReviewsAPI.Registration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IPasswordCryptographyHelper, BCryptPasswordHelper>();
builder.Services.AddTransient<IUserAuthenticationHelper, UserAuthenticatonHelper>();
builder.Services.AddTransient<IClaimsHelper, UserClaimsHelper>();
builder.Services.AddTransient<IRegistrationHelper, RegistrationHelper>();

builder.Services.AddAuthentication(AuthenticationSchemasConsts.DefaultSchema)
    .AddCookie(AuthenticationSchemasConsts.DefaultSchema); //add cookie schemas
builder.Services.AddAuthorization(builder =>
{
    builder.AddPolicy(AuthenticationPoliciesConsts.DefaultUserAuth, policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser()
            .AddAuthenticationSchemes(AuthenticationSchemasConsts.DefaultSchema);
    });

    builder.AddPolicy(AuthenticationPoliciesConsts.AdminUserAuth, policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser()
            .AddAuthenticationSchemes(AuthenticationSchemasConsts.DefaultSchema)
            .RequireClaim("admin", "true");
    });
});

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
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.MapControllers();

app.Run();
