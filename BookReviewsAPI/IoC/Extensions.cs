using BookReviews.Domain.Models;
using BookReviews.Infrastructure.Authentication.Helpers;
using BookReviews.Infrastructure.Authentication.Policies;
using BookReviews.Infrastructure.Authentication.Schemas;
using BookReviews.Infrastructure.Cryptography;
using BookReviews.Infrastructure.Registration;
using BookReviews.WebAPI.Consts;
using BookReviewsAPI.Registration;
using System.Text.Json.Serialization;

namespace Microsoft.Extensions
{
    public static class Extensions
    {
        public static void AddServivces(this IServiceCollection services)
        {
            services.AddHelpers();

            services.AddCookieAuthenticationAndAuthorization();

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
            });

            services.AddControllers()
                .AddJsonOptions(opt=>
                {
                    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });
            services.AddSwaggerGen();
            services.AddDatabase();

            services.AddCorsPolicies();
            services.AddHealthChecks();
        }

        public static void AddHelpers(this IServiceCollection services)
        {
            services.AddTransient<IPasswordCryptographyHelper, BCryptPasswordHelper>();
            services.AddTransient<IUserAuthenticationHelper, UserAuthenticatonHelper>();
            services.AddTransient<IClaimsHelper, UserClaimsHelper>();
            services.AddTransient<IRegistrationHelper, RegistrationHelper>();
        }

        public static void AddCookieAuthenticationAndAuthorization(this IServiceCollection services)
        {
            services.AddAuthentication(AuthenticationSchemasConsts.DefaultSchema)
                .AddCookie(AuthenticationSchemasConsts.DefaultSchema, options =>
                {
                    options.Cookie.SameSite = SameSiteMode.None;
                });
            services.AddAuthorization(builder =>
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
        }

        public static void AddDatabase(this IServiceCollection services)
        {
            services.AddDbContext<BookReviewsDbContext>();
            services.AddTransient<IUsersRepository, BookReviewsDbContext>();
        }

        public static void AddCorsPolicies(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: CorsPolicyConsts.AllowLocalhostClient,
                                  policy =>
                                  {
                                      policy.WithOrigins("https://localhost:3000")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowCredentials();
                                  });
            });
        }
    }
}
