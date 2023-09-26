using AgendaFornecedores.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgendaFornecedores.Controllers
{
    public class GrupoTrabalhoController : Controller
    {
        public IActionResult AdicionarGrupoAcesso(string nome_grupo, string fulladm)
        {
            if (fulladm==null) fulladm = "0";

            GrupoPermitido Gt = new GrupoPermitido(nome_grupo,fulladm);

            if (Gt.AdicionarGrupo(Gt))
            {
                return RedirectToAction("AdicionarGrupoAcesso","Home");
            }
                

            return View();
        }
    }
}
