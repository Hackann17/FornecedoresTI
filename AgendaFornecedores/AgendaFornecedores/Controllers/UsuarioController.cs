using AgendaFornecedores.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgendaFornecedores.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Login(string nomeUsuario, string senhaUsuario)
        {
            //chamar o metodo de verificação 
            Usuario usuario = new Usuario();
            
            string autentificacao = usuario.Logar(nomeUsuario, senhaUsuario);

            if(!autentificacao.Equals("1"))
            {
                //criar a sessao do usuario






                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


            



            



           
        }
    }
}
