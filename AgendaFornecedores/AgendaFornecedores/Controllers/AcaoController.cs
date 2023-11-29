using AgendaFornecedores.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace AgendaFornecedores.Controllers
{
    public class AcaoController : Controller
    {
        public  IActionResult RegistrarAcao(string objtacao)
        {
            Acao acao = JsonConvert.DeserializeObject<Acao>(objtacao);

            if (acao.AdiconarAcao(acao))
            {
                TempData["acao"] = $"A ação de {acao.Action} foi realizada com sucesso";
                string aacao = acao.Action;
                return RedirectToAction("RedirecionarTela", "Acao", new { aacao });

            }
            TempData["acao"] = $"Houve um erro ao {acao.Action}...";
            return RedirectToAction("RedirecionarTela", "Acao");
        }

        public IActionResult RedirecionarTela(string aacao)
        {
            //fazer cadeia de condições para conferir qual tela será a proxima a ser exibida

            if (aacao == "cadastrar")  return RedirectToAction("Formulario", "Fornecedor");

            if (aacao == "deletar") return RedirectToAction("Index", "Home");

            if (aacao == "alterar") return RedirectToAction("Index", "Home");

            return RedirectToAction("Index", "Home");
        }
    }
}
