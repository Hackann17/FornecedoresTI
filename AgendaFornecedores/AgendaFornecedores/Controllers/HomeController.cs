using AgendaFornecedores.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AgendaFornecedores.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ListadeAcoes()
        {
            return View();
        }

        public IActionResult Formulario()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        public IActionResult Login(string nomeUsuario, string senhaUsuario)
      {
            Usuario u = new Usuario();
            string autentificacao = u.Logar(nomeUsuario, senhaUsuario);

            if(autentificacao == "1")
            {
                HttpContext.Session.SetString("usuario", nomeUsuario);
                return RedirectToAction("Index", "Home");
            }
            else if (autentificacao == "0")
            {
                TempData["mensagem"] = autentificacao;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["mensagem"] = autentificacao;
                return RedirectToAction("Index", "Home");
            }
        }

    }
}