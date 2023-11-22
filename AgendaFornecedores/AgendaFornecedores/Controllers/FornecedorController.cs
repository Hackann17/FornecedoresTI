using AgendaFornecedores.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AgendaFornecedores.Controllers
{
    public class FornecedorController : Controller
    {
        [HttpPost]
        public IActionResult Cadastrar(Fornecedor fornecedor) {

            Usuario u = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
            fornecedor.Grupo_trabalho = u.GrupoTrabalho;

            if (fornecedor.Cadastrar(fornecedor))
            {
                DateTime dataHoraAtual = DateTime.Now;
                Acao ac = new Acao(0,u.NomeUsuario,"cadastrar", dataHoraAtual, fornecedor.Nome);

                //salva o objeto de ação e atualiza a lista de ações
                string objtacao = JsonConvert.SerializeObject(ac);
                return RedirectToAction("RegistrarAcao", "Acao", new {objtacao});

            }

            return RedirectToAction("Formulario", "Fornecedor");

        }
        public IActionResult Deletar(string jfornecedor)
        {
            Fornecedor fornecedor = JsonConvert.DeserializeObject<Fornecedor>(jfornecedor);
            if (fornecedor.DeletarFornecedor(fornecedor.Id))
            {
                Usuario us = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                DateTime dataHoraAtual = DateTime.Now;

                Acao ac = new Acao(0, us.NomeUsuario, "deletar", dataHoraAtual, fornecedor.Nome);

                string objtacao = JsonConvert.SerializeObject(ac);
                return RedirectToAction("RegistrarAcao", "Acao", new {objtacao});
            }
            return RedirectToAction("Index", "Home"); ;
        }
         
        public IActionResult Alterar(Fornecedor fornecedor)
        {
            //Fornecedor fornecedor = new Fornecedor(id, nomeFornecedor,cnpj,contato,email,anotacao, grupoT,  DateOnly.Parse(vencimentoFatura));

            if(fornecedor.AlterarFornecedor(fornecedor)) 
            {
                Usuario u = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                Acao ac = new Acao(0, u.NomeUsuario, "alterar", DateTime.Now, fornecedor.Nome);
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


       public IActionResult AnaliseVencFatura(string forns)
        {
            List<Fornecedor> fornes = JsonConvert.DeserializeObject<List<Fornecedor>>(forns);
            Fornecedor forn  =  new Fornecedor();

            forn.AnaliseVencFatura(fornes);
            TempData["Verifica faturas"] = true;

            return RedirectToAction("Index", "Home");
        }




    }
}
