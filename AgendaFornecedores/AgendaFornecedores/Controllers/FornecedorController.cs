using AgendaFornecedores.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AgendaFornecedores.Controllers
{
    public class FornecedorController : Controller
    {
        [HttpPost]
        public IActionResult Cadastrar(Fornecedor fornecedor)
        {

            Usuario u = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
            fornecedor.Grupo_trabalho = u.GrupoTrabalho;

            if (fornecedor.Cadastrar(fornecedor))
            {
                DateTime dataHoraAtual = DateTime.Now;
                Acao ac = new Acao(0, u.NomeUsuario, "cadastrar", dataHoraAtual, fornecedor.Nome);

                //salva o objeto de ação e atualiza a lista de ações
                string objtacao = JsonConvert.SerializeObject(ac);
                return RedirectToAction("RegistrarAcao", "Acao", new { objtacao });

            }

            return RedirectToAction("Formulario", "Fornecedor");
        }
        public IActionResult Deletar(string jfornecedor)
        {
            Fornecedor fornecedor = JsonConvert.DeserializeObject<Fornecedor>(jfornecedor);
            if (fornecedor.DeletarFornecedor(fornecedor))
            {
                Fornecedor f = new Fornecedor();

                Usuario us = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                DateTime dataHoraAtual = DateTime.Now;

                Acao ac = new Acao(0, us.NomeUsuario, "deletar", dataHoraAtual, fornecedor.Nome);

                string objtacao = JsonConvert.SerializeObject(ac);
                return RedirectToAction("RegistrarAcao", "Acao", new { objtacao });
            }
            return RedirectToAction("Index", "Home"); ;
        }
        public IActionResult Alterar(Fornecedor fornecedor)
        {
            //Fornecedor fornecedor = new Fornecedor(id, nomeFornecedor,cnpj,contato,email,anotacao, grupoT,  DateOnly.Parse(vencimentoFatura));

            if (fornecedor.AlterarFornecedor(fornecedor))
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
            Usuario us = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
            Fornecedor forn = new Fornecedor();

           if(forn.AnaliseVencFatura(fornes, us))
            {
                TempData["AnaliseVencFatura"] = "Email de analise enviado com sucesso.";

            }
            else
            {
                TempData["AnaliseVencFatura"] = "E-mail não enviado.";
            }
            return RedirectToAction("Index", "Home");
        }


        public IActionResult EnviarNota( string destinatario, string mensagem) 
        {
            Fornecedor forn = new Fornecedor();
            List<string> fornecedors = forn.EnviarNota(destinatario, mensagem);
            if (!(fornecedors  == null || forn.EnviarNota(destinatario, mensagem).Count <= 0))
            {

                string usuario = HttpContext.Session.GetString("usuario");
                string fornecedores = JsonConvert.SerializeObject(fornecedors);
                RedirectToAction("RegistrarAcao", "Acao", new { fornecedores, usuario });

                TempData["EnvioDeNotas"] = "As notas foram enviadas com sucesso !";

                return RedirectToAction("EnviarNota", "Home");

            }
            else
            {
                TempData["EnvioDeNotas"] = "Não foi possivel enviar as notas!";
            }
            return RedirectToAction("EnviarNota", "Home");
        }
        

        public IActionResult CriaPastas(string forns)
        {
            List<Fornecedor> fornecedores = JsonConvert.DeserializeObject<List<Fornecedor>>(forns);
            Fornecedor fornecedor1 = new Fornecedor();

            if (fornecedor1.CriaPastas(fornecedores))
            {
                TempData["VerificaCriaDiretorio"] = "Pastas criadas no diretorio";   
            }
            else{
                TempData["VerificaCriaDiretorio"] = "Não foi possivel criar as Pastas";
            }
            return RedirectToAction("Index", "Home");
        }

    }
}

