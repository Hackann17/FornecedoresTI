using AgendaFornecedores.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AgendaFornecedores.Controllers
{
    public class FornecedorController : Controller
    {
        public IActionResult Cadastrar(string nomeFornecedor, string cnpj, string contato, string email, string anotacao,string vencimentoFatura) {

            Usuario u = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));

            Fornecedor fornecedor = new Fornecedor(nomeFornecedor, cnpj, contato, email, anotacao, u.GrupoTrabalho, vencimentoFatura);

            if (fornecedor.Cadastrar(fornecedor))
            {
                DateTime dataHoraAtualUtc = DateTime.UtcNow;

                Acao ac = new Acao(u.NomeUsuario,"cadastrar", dataHoraAtualUtc.ToString("dd/MM/yyyy HH:mm:ss"), nomeFornecedor);

                //salva o objeto de ação e atualiza a lista de ações
                string objtacao = JsonConvert.SerializeObject(ac);
                return RedirectToAction("RegistrarAcao", "Acao", new {objtacao});

            }
            return RedirectToAction("Formulario", "Fornecedor");

        }

        public IActionResult Deletar(string cnpj, string nomeF)
        {
            Fornecedor fornecedor = new();
            if (fornecedor.DeletarFornecedor(cnpj))
            {
                Usuario us = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                DateTime dataHoraAtualUtc = DateTime.Now;

                Acao ac = new Acao(us.NomeUsuario, "deletar", dataHoraAtualUtc.ToString("dd/MM/yyyy HH:mm:ss"), nomeF);

                string objtacao = JsonConvert.SerializeObject(ac);
                return RedirectToAction("RegistrarAcao", "Acao", new {objtacao});
            }
            return RedirectToAction("Index", "Home"); ;
        }

        public IActionResult Alterar(string cnpj, string nomeF)
        {
            Fornecedor fornecedor = new();
            if(fornecedor.AlterarFornecedor(cnpj, nomeF))
            {
                return View();
            }


            return View();
        }







        public IActionResult Formulario()
        {
            return View();
        }

    }
}
