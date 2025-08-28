using Core;
using Core.Service;
using CuidaPetWebFilter;
using Microsoft.EntityFrameworkCore;
using Service;
namespace CuidaPetWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<CustomExceptionFilter>();
            });

            builder.Services.AddTransient<IProdutoService, ProdutoService>();
            builder.Services.AddTransient<IEspecieService, EspecieService>();
            builder.Services.AddTransient<IRacaService, RacaService>();
            builder.Services.AddTransient<IDoencaService, DoencaService>();
            builder.Services.AddTransient<IVacinaService, VacinaService>();
            builder.Services.AddTransient<IEstabelecimentoService, EstabelecimentoService>();
            builder.Services.AddTransient<IFuncionarioService, FuncionarioService>();
            builder.Services.AddTransient<IPessoaService, PessoaService>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddDbContext<CuidaPetContext>(
                options => options.UseMySQL(builder.Configuration.GetConnectionString("CuidaPetDatabase")!));

            var app = builder.Build();

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
