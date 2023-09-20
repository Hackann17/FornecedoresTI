using AgendaFornecedores.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgendaFornecedores.Controllers
{
    public class FornecedorController : Controller
    {
        public IActionResult Cadastrar(string nomeFornecedor, string cnpj, string contato, string email, string anotacao) {

            //esse metodo: 
            //realiza o cadastro dos fornecedores no banco de dados
            Fornecedores fornecedor = new Fornecedores(nomeFornecedor, cnpj, contato, email, anotacao);

            if (fornecedor.Cadastrar(fornecedor))
            {
                string nomeU = HttpContext.Session.GetString("usuario");

                DateTime dataHoraAtualUtc = DateTime.UtcNow;

                Acao ac = new Acao(nomeU, "cadastro", dataHoraAtualUtc.ToString("dd/MM/yyyy HH:mm:ss"), nomeFornecedor);

                RedirectToAction("ResgistrarAcao", "Acao", new { acao = ac });

                RedirectToAction("Formulario", "Forncedor");
            }

            //salva o objeto de ação e atualiza a lista de ações

            //atulizar a pagina das lista de fornecedores

            //aponta para um arquivo em ume pasta que sera exibido na tela do usuario
            return RedirectToAction("Formulario", "Fornecedor");
        }




        public IActionResult Formulario()
        {
            return View();
        }

    }
}
