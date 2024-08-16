using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DefenceSimulator3.Data;
using DefenceSimulator3.Service;
using Microsoft.Extensions.Options;
using DefenceSimulator3.Sockets;

namespace DefenceSimulator3
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<DefenceSimulator3Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefenceSimulator3Context") ?? throw new InvalidOperationException("Connection string 'DefenceSimulator3Context' not found.")
                ));

            
            

            builder.Services.AddScoped<AttackHandlerService>();
            builder.Services.AddSignalR(); // <-- Add this line
            builder.Services.AddScoped(typeof(IronDomeHub));


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();
            app.MapHub<IronDomeHub>("/IronDomeHub");
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthorization();
            using (var scope = app.Services.CreateScope())
            {
                var attackHandlerService = scope.ServiceProvider.GetRequiredService<AttackHandlerService>();
                await attackHandlerService.InitialThreats();
            }

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
            
        }
    }
}
