using Microsoft.AspNetCore.Identity;

namespace Core;

public  class UsuarioIdentity : IdentityUser
{
    public virtual Pessoa? Pessoa { get; set; }
}
