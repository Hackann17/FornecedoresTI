using AgendaFornecedores.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AgendaFornecedores.Controllers
{
    public class FornecedorController : Controller
    {
        public IActionResult Cadastrar(string nomeFornecedor, string cnpj, string contato, string email, string anotacao) {

            Fornecedores fornecedor = new Fornecedores(nomeFornecedor, cnpj, contato, email, anotacao);

            if (fornecedor.Cadastrar(fornecedor))
            {
                Usuario u = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));

                DateTime dataHoraAtualUtc = DateTime.UtcNow;

                Acao ac = new Acao(u.NomeUsuario,"cadastro", dataHoraAtualUtc.ToString("dd/MM/yyyy HH:mm:ss"), nomeFornecedor);
                //salva o objeto de ação e atualiza a lista de ações
                string objtacao = JsonConvert.SerializeObject(ac);
                return RedirectToAction("ResgistrarAcao", "Acao", new {objtacao});

            }

            TempData["mesagemCadastros"] = "não foi possivel realizar o cadastro dese fornecedor..."; 
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
