using Core.Service;
using CuidaPetWebFilter;
using Microsoft.EntityFrameworkCore;
using Service;
using Core.Context;
using Core;
using CuidaPetWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace CuidaPetWeb
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configurar localização para aceitar vírgula como separador decimal
            var cultureInfo = new CultureInfo("pt-BR");
            cultureInfo.NumberFormat.NumberDecimalSeparator = ",";
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ",";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

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
            builder.Services.AddTransient<IPetService, PetService>();
            builder.Services.AddTransient<IPessoaService, PessoaService>();
            builder.Services.AddTransient<IEspecialidadeService, EspecialidadeService>();
            builder.Services.AddTransient<INotificacaoService, NotificacaoService>();
            builder.Services.AddTransient<IAgendamentoService, AgendamentoService>();
            builder.Services.AddTransient<IVacinacaoService, VacinacaoService>();
            builder.Services.AddTransient<IPedidoProdutoService, PedidoProdutoService>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddDbContext<CuidaPetContext>(
                options => options.UseMySQL(
                    builder.Configuration.GetConnectionString("CuidaPetDatabase")!,
                    b => b.MigrationsAssembly("CuidaPetWeb")));

            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            builder.Services.AddDefaultIdentity<UsuarioIdentity>(options =>
            { 
                //SignIn Settings
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = false;

                //Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;

                //User settings
                options.User.RequireUniqueEmail = false;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                //Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            }).AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<CuidaPetContext>();

            var app = builder.Build();

            // Criar roles se não existirem
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Administrador", "Gerente", "Tutor", "Atendente", "Veterinário" };
                
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Configurar localização
            var supportedCultures = new[] { new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
