using AgendaFornecedores.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            return View(Fornecedores.listarFornecedores());
        }

        public IActionResult ListadeAcoes()
        {
            return View(Acao.listarAcoes());
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
            object autentificação = Usuario.Logar(nomeUsuario, senhaUsuario);

            if(autentificação != null)
            {
                if (autentificação is Exception)
                {
                    Exception err = (Exception)autentificação;
                    TempData["mensagem"] = err.Message;
                    return RedirectToAction("Index", "Home");
                }

                //serializa o atentificado em um sessao
                
                HttpContext.Session.SetString("usuario",JsonConvert.SerializeObject(autentificação));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["mensagem"] = "O usuario não foi encontrado...";
                return RedirectToAction("Index", "Home");
            }
           
        }

        public IActionResult AdicionarGrupoAcesso()
        {
            return View(Grupo_permitido.listarGrupos());
        }

    }
}