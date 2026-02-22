using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Core;
using Core.Context;

namespace CuidaPetWeb.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<UsuarioIdentity> _signInManager;
        private readonly UserManager<UsuarioIdentity> _userManager;
        private readonly IUserStore<UsuarioIdentity> _userStore;
        private readonly IUserEmailStore<UsuarioIdentity> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly CuidaPetContext _context;

        public RegisterModel(
            UserManager<UsuarioIdentity> userManager,
            IUserStore<UsuarioIdentity> userStore,
            SignInManager<UsuarioIdentity> signInManager,
            ILogger<RegisterModel> logger,
            CuidaPetContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; } = null!;

        public string ReturnUrl { get; set; } = null!;

        public IList<AuthenticationScheme> ExternalLogins { get; set; } = null!;

        public class InputModel
        {
            [Required(ErrorMessage = "O campo Email é obrigatório.")]
            [EmailAddress(ErrorMessage = "Email inválido.")]
            [Display(Name = "Email")]
            public string Email { get; set; } = null!;

            [Required(ErrorMessage = "O campo Nome é obrigatório.")]
            [StringLength(100, ErrorMessage = "O {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 3)]
            [Display(Name = "Usuário")]
            public string Nome { get; set; } = null!;

            [Required(ErrorMessage = "O campo Telefone é obrigatório.")]
            [StringLength(11, ErrorMessage = "O Telefone deve ter 11 dígitos (DDD + número).", MinimumLength = 11)]
            [Display(Name = "Telefone")]
            [RegularExpression(@"^\d{11}$", ErrorMessage = "Telefone deve conter apenas números (11 dígitos).")]
            public string Telefone { get; set; } = null!;

            [Required(ErrorMessage = "O campo CPF é obrigatório.")]
            [StringLength(11, ErrorMessage = "O CPF deve ter 11 dígitos.", MinimumLength = 11)]
            [Display(Name = "CPF")]
            [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter apenas números (11 dígitos).")]
            public string Cpf { get; set; } = null!;

            [Required(ErrorMessage = "O campo Senha é obrigatório.")]
            [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Senha")]
            public string Password { get; set; } = null!;

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar Senha")]
            [Compare("Password", ErrorMessage = "A senha e a confirmação não coincidem.")]
            public string ConfirmPassword { get; set; } = null!;

            // Campos de Endereço
            [Required(ErrorMessage = "O campo Logradouro é obrigatório.")]
            [StringLength(100, ErrorMessage = "O Logradouro deve ter no máximo 100 caracteres.")]
            [Display(Name = "Logradouro")]
            public string Logradouro { get; set; } = null!;

            [Required(ErrorMessage = "O campo Número é obrigatório.")]
            [StringLength(10, ErrorMessage = "O Número deve ter no máximo 10 caracteres.")]
            [Display(Name = "Número")]
            public string Numero { get; set; } = null!;

            [StringLength(50, ErrorMessage = "O Complemento deve ter no máximo 50 caracteres.")]
            [Display(Name = "Complemento")]
            public string? Complemento { get; set; }

            [Required(ErrorMessage = "O campo Bairro é obrigatório.")]
            [StringLength(50, ErrorMessage = "O Bairro deve ter no máximo 50 caracteres.")]
            [Display(Name = "Bairro")]
            public string Bairro { get; set; } = null!;

            [Required(ErrorMessage = "O campo Cidade é obrigatório.")]
            [StringLength(100, ErrorMessage = "A Cidade deve ter no máximo 100 caracteres.")]
            [Display(Name = "Cidade")]
            public string Cidade { get; set; } = null!;

            [Required(ErrorMessage = "O campo Estado é obrigatório.")]
            [StringLength(2, ErrorMessage = "O Estado deve ter 2 caracteres.", MinimumLength = 2)]
            [Display(Name = "Estado (UF)")]
            [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "Estado deve ser a sigla com 2 letras maiúsculas (ex: SP, RJ).")]
            public string Estado { get; set; } = null!;
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? string.Empty;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            
            if (ModelState.IsValid)
            {
                // Verificar se o CPF já está cadastrado
                var pessoaExistente = _context.Pessoas.FirstOrDefault(p => p.Cpf == Input.Cpf);
                if (pessoaExistente != null)
                {
                    ModelState.AddModelError(string.Empty, "CPF já cadastrado.");
                    return Page();
                }

                // Criar o usuário Identity
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Nome, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                user.PhoneNumber = Input.Telefone;
                user.NormalizedUserName = Input.Nome.ToUpper();

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Usuário criou uma nova conta com senha.");

                    // Atribuir role padrão de "Tutor" para novos usuários
                    await _userManager.AddToRoleAsync(user, "Tutor");

                    // Criar a Pessoa associada ao usuário
                    var pessoa = new Pessoa
                    {
                        Cpf = Input.Cpf,
                        IdUsuario = user.Id,
                        Status = "A", // Ativo
                        Logradouro = Input.Logradouro,
                        Numero = Input.Numero,
                        Complemento = Input.Complemento,
                        Bairro = Input.Bairro,
                        Cidade = Input.Cidade,
                        Estado = Input.Estado
                    };

                    _context.Pessoas.Add(pessoa);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Pessoa criada e associada ao usuário.");

                    // Fazer login automático após o registro
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Se chegou aqui, algo falhou, reexibir o formulário
            return Page();
        }

        private UsuarioIdentity CreateUser()
        {
            try
            {
                return Activator.CreateInstance<UsuarioIdentity>();
            }
            catch
            {
                throw new InvalidOperationException($"Não foi possível criar uma instância de '{nameof(UsuarioIdentity)}'. " +
                    $"Certifique-se de que '{nameof(UsuarioIdentity)}' não é uma classe abstrata e possui um construtor sem parâmetros, ou alternativamente " +
                    $"substitua a página de registro em /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<UsuarioIdentity> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("A UI padrão requer um user store com suporte a email.");
            }
            return (IUserEmailStore<UsuarioIdentity>)_userStore;
        }
    }
}
