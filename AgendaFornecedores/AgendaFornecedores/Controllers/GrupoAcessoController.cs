using AgendaFornecedores.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace AgendaFornecedores.Controllers
{
    public class GrupoAcessoController : Controller
    {
        public IActionResult AdicionarGrupoAcesso(string nome_grupo, int fulladm)
        {
            if (fulladm==null) fulladm = 0;

            int id = 0;
            GrupoAcesso Gt = new GrupoAcesso(id,nome_grupo, fulladm); ;

            if (Gt.AdicionarGrupo(Gt))
            {
                return RedirectToAction("AdicionarGrupoAcesso","Home");
            }
                

            return View();
        }

        public IActionResult ApagarGrupoAcesso(string nomeGrupo)
        {
            GrupoAcesso Gr = new();

            if (Gr.ApagarGrupoAcesso(nomeGrupo))
            {
                TempData["grupoAcesso"] = "Grupo apagado comm sucesso";
                return RedirectToAction("AdicionarGrupoAcesso", "Home");

            }

            TempData["grupoAcesso"] = "Houve um erro ao apagar o grupo ...";
            return RedirectToAction("AdicionarGrupoAcesso", "Home");
        }



    }
}
