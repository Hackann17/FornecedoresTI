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
            if (HttpContext.Session.GetString("usuario") != null)
            {
                if (TempData["fornecedores"] == null)
                {
                    List<Fornecedor> fornecedores = Fornecedor.listarFornecedores();

                    TempData["fornecedores"] = fornecedores;
                    TempData["Verifica faturas"] = true;

                    //teste, o ideal é esse metodo ser chamado somente no seu controller respectivo
                    Fornecedor forn = new Fornecedor();
                    forn.AnaliseVencFatura(fornecedores);
                    TempData["Verifica faturas"] = true;

                    // string forns = JsonConvert.SerializeObject(fornecedores);
                    // return RedirectToAction("AnaliseVencFatura", "Fornecedor", new { forns });

                }
                if ((bool)TempData["Verifica faturas"])
                {
                    //nesse caso a lista as datas de evncimento ja foram verificadas
                    return View(TempData["fornecedores"]);
                }
                else
                {
                    //nesse caso elas nao foram verificadas ainda
                    string forns = JsonConvert.SerializeObject(TempData["fornecedores"]);
                    return RedirectToAction("AnaliseVencFatura", "Fornecedor" , new {forns});

                }

            }
            else
            {
                return View();
            }

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
                //redirecion para que a verificação da lista de fornecedores seja revisada

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
            return View(GrupoAcesso.listarGrupos());
        }

        public IActionResult Sair()
        {
            HttpContext.Session.Remove("usuario");
            return RedirectToAction("Index", "Home");
        }
    }
}