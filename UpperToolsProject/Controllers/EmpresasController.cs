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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarCadastro([Bind("Cnpj")] Empresa empresa)
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri("https://www.receitaws.com.br/v1/cnpj/") };

            string cnpj = empresa.Cnpj;
            var response = await client.GetAsync(cnpj);
            var content = await response.Content.ReadAsStringAsync();

            Empresa empresas = JsonConvert.DeserializeObject<Empresa>(content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            empresas.Cnpj = empresas.Cnpj.Replace("/", "").Replace(".", "").Replace("-", "");


            if (empresas != null)
            {

                _context.Add(empresas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empresas);
        }

        // GET: Empresas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresa.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            return View(empresa);
        }

        // POST: Empresas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Cnpj,DataSituacao,MotivoSituacao,Tipo,Nome,Telefone,Situacao,Porte,Abertura,NaturezaJuridica,UltimaAtualizacao,Status,Fantasia,Logradouro,Numero,Complemento,Cep,Bairro,Municipio,Uf,Email,Efr,SituacaoEspecial,DataSituacaoEspecial,CapitalSocial")] Empresa empresa)
        {
            if (id != empresa.Cnpj)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(empresa.Cnpj))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(empresa);
        }

        // GET: Empresas/Delete/5
        public async Task<IActionResult> Delete(string id)
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

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
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
