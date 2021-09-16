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

        // GET: Empresas/Details/5
        public async Task<IActionResult> Details(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresa
                .FirstOrDefaultAsync(m => m.Cnpj == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
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



            Empresa empresas = JsonConvert.DeserializeObject<Empresa>(content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });


            if (response.IsSuccessStatusCode && empresas.Status == "OK" && ValidaCNPJ.IsCnpj(cnpj))
            {


                empresas.Cnpj = RemovePontuacao.RmPontCnpj(empresas.Cnpj);
                if (EmpresaExists(empresas.Cnpj))
                {
                    return View("BuscarCadastro");
                }else
                _context.Add(empresas);
                await _context.SaveChangesAsync();
                bool success = true;
                ViewBag.success = success;

            }
            else
            {
                return View();
            }
            return View("AdicionarCadastro");
        }

        // GET: Empresas/BuscarCadastro
        public IActionResult BuscarCadastro()
        {
            return View();
        }

        // POST: Empresas/BuscarCadastro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuscarCadastro(Empresa emp)
        {
            emp.Cnpj = RemovePontuacao.RmPontCnpj(emp.Cnpj);
            string id = emp.Cnpj;
            string nome = emp.Nome;

            if (id == null && nome == null)
            {
                return NotFound();
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
                return NotFound();
            }
            ViewBag.emp = empresa;
            return View("Details");
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
            var empresa = await _context.Empresa.FindAsync(id);
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
