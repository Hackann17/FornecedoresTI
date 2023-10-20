using AgendaFornecedores.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace AgendaFornecedores.Controllers
{
    public class GrupoAcessoController : Controller
    {
        public IActionResult AdicionarGrupoAcesso(string nome_grupo, bool fulladm)
        {
            if (fulladm == null) fulladm = false ;

            int id = 0;
            GrupoAcesso Gt = new GrupoAcesso(id,nome_grupo, fulladm); ;

            if (Gt.AdicionarGrupo(Gt))
            {
                return RedirectToAction("AdicionarGrupoAcesso","Home");
            }          
            return View();
        }

        public IActionResult ApagarGrupoAcesso(int idGrupo)
        {
            GrupoAcesso Gr = new();

            if (Gr.Apagar(idGrupo))
            {
                TempData["grupoAcesso"] = "Grupo apagado comm sucesso";
                return RedirectToAction("AdicionarGrupoAcesso", "Home");

            }
            TempData["grupoAcesso"] = "Houve um erro ao apagar o grupo ...";
            return RedirectToAction("AdicionarGrupoAcesso", "Home");
        }
    }
}
