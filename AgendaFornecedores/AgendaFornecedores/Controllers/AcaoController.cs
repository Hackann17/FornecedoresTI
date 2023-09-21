using AgendaFornecedores.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AgendaFornecedores.Controllers
{
    public class AcaoController : Controller
    {
        public  IActionResult ResgistrarAcao(string objtacao)
        {
            Acao acao = JsonConvert.DeserializeObject<Acao>(objtacao);

            if (acao.AdiconarAcao(acao))
            {
                TempData["cadastro"] = "A ação de cadastro foi realizada com sucesso !";
                return RedirectToAction("Formulario", "Fornecedor");

            }

            TempData["cadastro"] = "Houve um erro no cadastro...";
            return RedirectToAction("Formulario", "Fornecedor");

        }
    }
}
