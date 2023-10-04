using BookReviews.WebAPI.Consts;
using BookReviewsAPI.Authentication.Helpers;
using BookReviewsAPI.Authentication.Policies;
using BookReviewsAPI.Authentication.Schemas;
using BookReviewsAPI.Cryptography;
using BookReviewsAPI.Models;
using BookReviewsAPI.Registration;


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

            services.AddControllers();
            services.AddSwaggerGen();
            services.AddDatabase();

            services.AddCorsPolicies();
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
                .AddCookie(AuthenticationSchemasConsts.DefaultSchema); //add cookie schemas
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
        }

        public static void AddCorsPolicies(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: CorsPolicyConsts.AllowLocalhostClient,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:3000");
                                  });
            });
        }
    }
}
