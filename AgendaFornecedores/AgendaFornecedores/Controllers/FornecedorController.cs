using AgendaFornecedores.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgendaFornecedores.Controllers
{
    public class FornecedorController : Controller
    {
        public IActionResult Cadastrar(string nome, string cnpj, string contato, string email, string anotacao) {
            Console.WriteLine(nome);
            Fornecedores fornecedor = new Fornecedores(nome, cnpj, contato, email, anotacao);
            Console.WriteLine(fornecedor.Cadastrar(fornecedor));

            //aponta para um arquivo em ume pasta que sera exibido na tela do usuario
            return RedirectToAction("Formulario", "Fornecedor");
        }




        public IActionResult Formulario()
        {
            return View();
        }

    }
}
