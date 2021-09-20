using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using UpperToolsProject.Data;
using UpperToolsProject.Models;
using UpperToolsProject.Tools.RemovePontuacao;
using UpperToolsProject.Tools.ValidaCnpj;

namespace UpperToolsProject.Controllers
{
    public class EmpresasController : Controller
    {
        private readonly MyDbContext _context;

        public EmpresasController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Empresas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Empresa.ToListAsync());
        }


        // Post: Empresas/DSocios
        [HttpPost]
        public async Task<IActionResult> Dsocios(Empresa emp)
        {
            var qsatolist = await _context.Qsa.ToListAsync();

            List<Qsa> qsa = new List<Qsa>();
            foreach (var item in qsatolist)
            {
                if (emp.Cnpj == item.EmpresaCnpj)
                {
                    
                    qsa.Add(item);
                }
            }
            ViewBag.qsa = qsa;

            if (qsa == null || qsa.Count == 0)
            {

             var empresa = await _context.Empresa
                    .FirstOrDefaultAsync(m => m.Cnpj == emp.Cnpj);

                ViewBag.emp = empresa;

                ViewBag.msg = "Este CNPJ não possui sócios";
                return View("Details", empresa);
            }
            return View(qsa);
        }

        [HttpPost]
        public async Task<IActionResult> AtividadePrimaria(Empresa emp)
        {
            var ativpr = await _context.Atividade.ToListAsync();

            List<Atividade> atvp = new List<Atividade>();
            foreach (var item in ativpr)
            {
                if (emp.Cnpj == item.EmpresaCnpj)
                {

                    atvp.Add(item);
                }
            }
          

            if (atvp == null)
            {
                ViewData["msg"] = "Este CNPJ não possui sócios";
                return RedirectToAction("Dsocios");
            }
            ViewBag.nome = "Principal";

            return View("Atividade",atvp);
        }

        [HttpPost]
        public async Task<IActionResult> AtividadeSecundaria(Empresa emp)
        {
            var ativse = await _context.AtividadeS.ToListAsync();

            List<AtividadeS> atvs = new List<AtividadeS>();
            foreach (var item in ativse)
            {
                if (emp.Cnpj == item.EmpresaCnpj)
                {

                    atvs.Add(item);
                }
            }

            if (atvs == null)
            {
                ViewData["msg"] = "Este CNPJ não possui sócios";
                return RedirectToAction("Dsocios");
            }
            ViewBag.nome = "Secundárias";


            return View("AtividadeS", atvs);
        }

        // GET: Empresas/AdicionarCadastro
        public IActionResult AdicionarCadastro()
        {
            return View();
        }

        // POST: Empresas/AdicionarCadastro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarCadastro([Bind("Cnpj")] Empresa empresa)
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri("https://www.receitaws.com.br/v1/cnpj/") };

            string cnpj = empresa.Cnpj;
            cnpj = RemovePontuacao.RmPontCnpj(cnpj);
            var response = await client.GetAsync(cnpj);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                TempData["msg"] = "Espere um momento para adicionar um novo CNPJ";
                return RedirectToAction("AdicionarCadastro");
            }
            
            Empresa empresas = JsonConvert.DeserializeObject<Empresa>(content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });



            if (response.IsSuccessStatusCode && empresas.Status == "OK" && ValidaCNPJ.IsCnpj(cnpj))
                {


                    empresas.Cnpj = RemovePontuacao.RmPontCnpj(empresas.Cnpj);
                    if (EmpresaExists(empresas.Cnpj))
                    {
                        TempData["msg"] = "Este cadastro já existe no sistema";
                    }
                    else
                    {
                        _context.Add(empresas);
                        await _context.SaveChangesAsync();
                        TempData["msgSuccess"] = "Cadastro realizado com sucesso!";
                    }
                }
            
            else
            {
                TempData["msg"] = "O CNPJ informado é inválido!";
            }
            return RedirectToAction("AdicionarCadastro");
        }

        // GET: Empresas/BuscarCadastro
        public IActionResult BuscarCadastro()
        {
            return View();
        }

        // GET: Empresas/Details
        public IActionResult Details ()
        {
            return View();
        }

        // POST: Empresas/Details/cnpj
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(Empresa emp)
        {
            emp.Cnpj = RemovePontuacao.RmPontCnpj(emp.Cnpj);
            string id = emp.Cnpj;
            string nome = emp.Nome;

            if (id == null && nome == null)
            {
                TempData["msg"] = "O CNPJ informado é inválido!";
                return RedirectToAction("BuscarCadastro");
            }

            Empresa empresa = null;
            if (id != null)
            {
                empresa = await _context.Empresa
                    .FirstOrDefaultAsync(m => m.Cnpj == id);
            }

            if (nome != null)
            {
                empresa = await _context.Empresa
                    .FirstOrDefaultAsync(m => m.Nome == nome);
            }

            if (empresa == null)
            {
                TempData["msg"] = "O dado informado não está no banco de dados!";
                return RedirectToAction("BuscarCadastro");
            }
            ViewBag.emp = empresa;

            return View(empresa);
            //return RedirectToAction("Details");
        }

        // GET: Empresas/Delete/
        public IActionResult Delete()
        {
            return View();
        }

        // POST: Empresas/Delete/cnpj
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Empresa emp)
        {
            string id = emp.Cnpj;

            id = RemovePontuacao.RmPontCnpj(id);
            var empresa = await _context.Empresa.FindAsync(id);

            var qsatolist = await _context.Qsa.ToListAsync();
            var atptolist = await _context.Atividade.ToListAsync();
            var atstolist = await _context.AtividadeS.ToListAsync();

            foreach (var item in qsatolist)
            {
                if (id == item.EmpresaCnpj)
                _context.Qsa.Remove(item);

            }
            foreach (var item in atptolist)
            {
                if (id == item.EmpresaCnpj)
                    _context.Atividade.Remove(item);

            }
            foreach (var item in atstolist)
            {
                if (id == item.EmpresaCnpj)
                    _context.AtividadeS.Remove(item);

            }


            if (empresa == null)
            {
                TempData["msg"] = "Este CNPJ não está cadastrado";
                return RedirectToAction("Delete");
            }
            
            _context.Empresa.Remove(empresa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpresaExists(string id)
        {
            return _context.Empresa.Any(e => e.Cnpj == id);
        }
    }
}
