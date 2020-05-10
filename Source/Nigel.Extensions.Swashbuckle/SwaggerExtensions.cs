using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nigel.Extensions.Swashbuckle
{
    public static class SwaggerExtensions
    {
        private static void AddBaseSwagger(this SwaggerGenOptions c, SwaggerOptions options)
        {
            c.SwaggerDoc(options.Name, new OpenApiInfo { Title = options.Title, Version = options.Version });
            if (options.IncludeSecurity)
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                          "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });//Json Token认证方式，此方式为全局添加
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { 
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference(){
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, Array.Empty<string>() 
                    }
                });
            }


            var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
            for (int i = 0; i < xmlFiles.Length; i++)
            {
                c.IncludeXmlComments(xmlFiles[i]);
            }

            c.UseInlineDefinitionsForEnums();

            c.DescribeAllParametersInCamelCase();
        }

        public static IServiceCollection AddSwaggerDocs<T1>(this IServiceCollection services)
            where T1 : IDocumentFilter
        {
            var options = GetSwaggerOptions(services);

            if (!options.Enabled) return services;

            return services.AddSwaggerGen(c =>
            {
                c.AddBaseSwagger(options);

                c.DocumentFilter<T1>();
                c.OperationFilter<AuthorizationHeaderOperationFilter>();
            });
        }

        public static IServiceCollection AddSwaggerDocs(this IServiceCollection services)
        {
            var options = GetSwaggerOptions(services);

            if (!options.Enabled) return services;

            return services.AddSwaggerGen(c => { c.AddBaseSwagger(options); });
        }

        private static SwaggerOptions GetSwaggerOptions(IServiceCollection services)
        {
            SwaggerOptions options;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                services.Configure<SwaggerOptions>(configuration.GetSection("swagger"));
                options = configuration.GetOptions<SwaggerOptions>("swagger") ?? new SwaggerOptions();
            }

            return options;
        }

        public static IApplicationBuilder UseSwaggerDocs(this IApplicationBuilder builder)
        {
            var options = builder.ApplicationServices.GetService<IConfiguration>()
                .GetOptions<SwaggerOptions>("swagger");
            if (!options.Enabled) return builder;

            var routePrefix = string.IsNullOrWhiteSpace(options.RoutePrefix) ? "swagger" : options.RoutePrefix;

            builder.UseStaticFiles()
                .UseSwagger(c => { c.RouteTemplate = routePrefix + "/{documentName}/swagger.json"; });

            return options.ReDocEnabled
                ? builder.UseReDoc(c =>
                {
                    c.RoutePrefix = routePrefix;
                    c.SpecUrl = $"{options.Name}/swagger.json";
                })
                : builder.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"/{routePrefix}/{options.Name}/swagger.json", options.Title);
                    c.RoutePrefix = routePrefix;
                    c.DefaultModelExpandDepth(2);
                    c.DefaultModelRendering(ModelRendering.Model);
                    c.DefaultModelsExpandDepth(-1);
                    c.DisplayOperationId();
                    c.DisplayRequestDuration();
                    c.DocExpansion(DocExpansion.None);
                    c.EnableDeepLinking();
                    c.EnableFilter();
                    c.MaxDisplayedTags(5);
                    c.ShowExtensions();
                    c.EnableValidator();
                });
        }

        public static string Underscore(this string value)
        {
            return string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()));
        }

        public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(section).Bind(model);

            return model;
        }
    }
}