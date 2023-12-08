using AgendaFornecedores.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
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

        public IActionResult RegistrarAcaoNota(string sfornecedores, string usuario)
        {
            List<string> fornecedores = JsonConvert.DeserializeObject<List<string>>(sfornecedores);
            Usuario u = JsonConvert.DeserializeObject<Usuario>(usuario);

            try
            {
                for (int i = 0; i < fornecedores.Count; i++)
                {
                    Acao ac = new Acao(0, u.NomeUsuario, "EnviarNota", DateTime.Now, fornecedores[i]);

                    if (!ac.AdiconarAcao(ac))
                    {
                        break;
                    }
                }

                string aacao = "EnviarNota";
                return RedirectToAction("RedirecionarTela", "Acao", new { aacao });
            }catch (Exception ex)
            {
                TempData["EnvioDeNotas"] = "Não foi possivel regeitrar ação de envio!";
                return RedirectToAction("EnviarNota", "Home");
            }

        }
        public IActionResult RedirecionarTela(string aacao)
        {
            //fazer cadeia de condições para conferir qual tela será a proxima a ser exibida

            if (aacao == "cadastrar")  return RedirectToAction("Formulario", "Fornecedor");

            if (aacao == "deletar") return RedirectToAction("Index", "Home");

            if (aacao == "alterar") return RedirectToAction("Index", "Home");

            if (aacao == "EnviarNota") return RedirectToAction("EnviarNota", "Home");

            return RedirectToAction("Index", "Home");
        }
    }
}
