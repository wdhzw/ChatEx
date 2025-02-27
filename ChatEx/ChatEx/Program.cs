using System.Net;
using System.Net.Http.Headers;
using ChatEx.Components;
using ChatEx.Interfaces;
using ChatEx.Models;
using ChatEx.Services;
using ChatEx.ViewModels;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MudBlazor.Services;
using Refit;

namespace ChatEx
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddMudServices();

            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddControllers();
            builder.Services.AddControllersWithViews();
            builder.Services.AddSwaggerGen();

            AddServices(builder.Services, builder.Configuration, builder.Environment);

            builder.WebHost.ConfigureKestrel(options => {
                options.Listen(IPAddress.Any, 9000);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAntiforgery();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.MapControllers();

            app.Run();
        }

        private static void AddServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            var myHostUri = new Uri("https://api.siliconflow.cn/");
            Action<HttpClient> config = (client) =>
            {
                var jsonMediaType = new MediaTypeWithQualityHeaderValue("application/json");
                client.BaseAddress = myHostUri;
                client.Timeout = TimeSpan.FromSeconds(1200);
                // 添加默认请求头
                client.DefaultRequestHeaders.Add("Authorization", "Bearer sk-bsjtdimuhfcbyyqhxkqlandpkjbmrdkluycjfygbjgjccqpf");
                client.DefaultRequestHeaders.Accept.Add(jsonMediaType);
            };
            services.AddRefitClient<IDeepSeekApi>()
                    .ConfigureHttpClient(config);
            services.AddRefitClient<IChatApi>()
                    .ConfigureHttpClient(config);
            services.AddRefitClient<IImageGenerationApi>()
                    .ConfigureHttpClient(config);
            

            
            services.TryAddScoped<ChatExViewModel>();
            services.TryAddScoped<ImageViewModel>();
            services.TryAddScoped<TranslateViewModel>();

            services.TryAddScoped<StreamParserService>();
            services.TryAddScoped<DeepSeekService>();

            //services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);
            //services.Configure<IISServerOptions>(options => options.AllowSynchronousIO = true);

            services.Configure<ApiInfo>(configuration.GetSection("ApiInfo"));
        }
    }
}
