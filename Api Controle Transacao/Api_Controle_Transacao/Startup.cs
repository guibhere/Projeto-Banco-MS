using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Api_Controle_Transacao.Service.Interface;
using Api_Controle_Transacao.Helper.Interface;
using Api_Controle_Transacao.Helper;
using static Api_Controle_Transacao.Helper.SplunkLogger;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Confluent.Kafka.DependencyInjection;
using StackExchange.Redis;

namespace Api_Controle_Transacao
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Fluent Validator
            services.AddRazorPages();
            services.AddControllers().AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<Startup>();
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api_Controle_Transacao", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter ‘Bearer’ [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                    },
                        new string[] {}
                    }
                });
                // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // c.IncludeXmlComments(xmlPath);
            });
            // For Identity
            // services.AddIdentity<ApplicationUser, IdentityRole>()
            //     .AddEntityFrameworkStores<ApplicationDbContext>()
            //     .AddDefaultTokenProviders();
            // Adding Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecretKey"]))
                };
            });
            // Databaset

            // DynamoDB
            if (Configuration.GetSection("DynamoDBConfig").GetValue<bool>("Local"))
            {
                var dynamodbconfig = new AmazonDynamoDBConfig { ServiceURL = (Configuration.GetSection("DynamoDBConfig").GetValue<String>("Server")) };
                var credentials = new BasicAWSCredentials("xxx", "xxx");
                services.AddSingleton<IAmazonDynamoDB>(new AmazonDynamoDBClient(credentials, dynamodbconfig));
            }
            else
                services.AddAWSService<IAmazonDynamoDB>();

            services.AddSingleton<IDynamoDBContext, DynamoDBContext>();

            // Services
            services.AddScoped<ITrasacaoService, TrasacaoService>();
            services.AddScoped<ITrasacaoRepository, TrasacaoRepository>();

            // Controllers
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            //Splunk
            var setting = Configuration.GetSection("SplunkConfig");
            services.Configure<SplunkConfig>(setting);
            services.AddScoped<ISplunkLogger, SplunkLogger>();

            //Kafka
            Dictionary<string, string> KafkaConfig = Configuration.GetSection("KafkaConfig").GetChildren().ToDictionary(c => c.Key, c => c.Value);
            services.AddKafkaClient(KafkaConfig);

            //Redis
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis")));

            //Helpers
            setting = Configuration.GetSection("ContaClienteConfig");
            services.Configure<ContaClienteConfig>(setting);
            services.AddScoped<IContaClienteConector, ContaClienteConector>();
            services.AddScoped<IHashMaker, HashMaker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api_Controle_Transacao v1"); c.RoutePrefix = string.Empty; });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
            //Permite Acesso a Arquivos na API, porem apenas com a autenticação
            /*
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    if (ctx.Context.User.Identity.IsAuthenticated)
                    {
                        ctx.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        ctx.Context.Response.ContentLength = 0;
                        ctx.Context.Response.Body = Stream.Null;
                        ctx.Context.Response.Headers.Add("Cache-Control", "no-store");
                    }
                    else
                        ctx.Context.Response.Redirect("/Swagger/");
                },
                FileProvider = new PhysicalFileProvider(
                 Path.Combine(env.ContentRootPath, "Arquivos")),
                RequestPath = "/Arquivos"
            });
            */
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
