using AgendaFornecedores.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AgendaFornecedores.Controllers
{
    public class FornecedorController : Controller
    {
        [HttpPost]
        public IActionResult Cadastrar(string nomeFornecedor, string cnpj, string contato, string email, string anotacao,string vencimentoFatura) {

            Usuario u = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));

            Fornecedor fornecedor = new Fornecedor(0,nomeFornecedor, cnpj, contato, email, anotacao, u.GrupoTrabalho, vencimentoFatura);

            if (fornecedor.Cadastrar(fornecedor))
            {
                DateTime dataHoraAtualUtc = DateTime.Now;
                Acao ac = new Acao(0,u.NomeUsuario,"cadastrar", dataHoraAtualUtc.ToString("dd/MM/yyyy HH:mm:ss"), nomeFornecedor);

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

                Acao ac = new Acao(0, us.NomeUsuario, "deletar", dataHoraAtualUtc.ToString("dd/MM/yyyy HH:mm:ss"), nomeF);

                string objtacao = JsonConvert.SerializeObject(ac);
                return RedirectToAction("RegistrarAcao", "Acao", new {objtacao});
            }
            return RedirectToAction("Index", "Home"); ;
        }

        public IActionResult Alterar(int id, string nomeFornecedor, string cnpj, string contato, string email,string grupoT, string vencimentoFatura, string anotacao)
         {
            Fornecedor fornecedor = new Fornecedor(id, nomeFornecedor,cnpj,contato,email,anotacao, grupoT,vencimentoFatura);

            if(fornecedor.AlterarFornecedor(fornecedor)) 
            {
                Usuario u = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                DateTime dataHoraAtualUtc = DateTime.Now;

                Acao ac = new Acao(0, u.NomeUsuario, "alterar", dataHoraAtualUtc.ToString("dd/MM/yyyy HH:mm:ss"), nomeFornecedor);

                //salva o objeto de ação e atualiza a lista de ações
                string objtacao = JsonConvert.SerializeObject(ac);
                return RedirectToAction("RegistrarAcao", "Acao", new { objtacao });
            }

            return View();
        }


        public IActionResult redirecionarDados(string jfornecedor)
        {
            //trazer os dados do fornecedor
            TempData["alterfornecedor"] = jfornecedor;
            return RedirectToAction("Formulario", "Fornecedor");
        }

        public IActionResult Formulario()
        {
            return View();
        }

    }
}
