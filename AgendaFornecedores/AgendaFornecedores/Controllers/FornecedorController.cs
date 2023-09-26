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

                Acao ac = new Acao(u.NomeUsuario,"cadastro", dataHoraAtualUtc.ToString("dd/MM/yyyy HH:mm:ss"), nomeFornecedor);

                //salva o objeto de ação e atualiza a lista de ações
                string objtacao = JsonConvert.SerializeObject(ac);
                return RedirectToAction("ResgistrarAcao", "Acao", new {objtacao});

            }

            TempData["cadastro"] = "não foi possivel realizar o cadastro desse fornecedor..."; 
            return RedirectToAction("Formulario", "Fornecedor");

        }

        public IActionResult DeletarFornecedor()
        {
            return View();
        }


        public IActionResult Formulario()
        
       {
            return View();
        }

    }
}
