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

        // ESTE MÉTODO FAZ COM QUE UMA PÁGINA MOSTRE APENAS OS SÓCIOS DO CNPJ RECEBIDO
        // POST: Empresas/DSocios
        [HttpPost]
        public async Task<IActionResult> Dsocios(Empresa emp)
        {
            var qsatolist = await _context.Qsa.ToListAsync();

            List<Qsa> qsa = new List<Qsa>();

            // SE O CNPJ ENVIADO PELA PÁGINA 'Details' FOR IGUAL À CNPJ's DO BANCO DE DADOS 'Qsa', ENTÃO ESSES CNPJS SÃO ADICIONADOS À UMA LISTA
            foreach (var item in qsatolist)
            {
                if (emp.Cnpj == item.EmpresaCnpj)
                {               
                    qsa.Add(item);
                }
            }
            ViewBag.qsa = qsa;

            // VALIDAÇÃO PARA NOTIFICAR ERRO CASO A EMPRESA NÃO TENHA SÓCIOS
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


        // POST: Empresas/AtividadePrimaria
        [HttpPost]
        public async Task<IActionResult> AtividadePrimaria(Empresa emp)
        {
            var ativpr = await _context.Atividade.ToListAsync();

            List<Atividade> atvp = new List<Atividade>();

            // SE O CNPJ ENVIADO PELA PÁGINA 'Details' FOR IGUAL À CNPJ's DO BANCO DE DADOS 'Atividade', ENTÃO ESSES CNPJS SÃO ADICIONADOS À UMA LISTA
            foreach (var item in ativpr)
            {
                if (emp.Cnpj == item.EmpresaCnpj)
                {

                    atvp.Add(item);
                }
            }
         
            ViewBag.nome = "Principal";

            return View("Atividade",atvp);
        }

        // POST: Empresas/AtividadeSecundaria
        [HttpPost]
        public async Task<IActionResult> AtividadeSecundaria(Empresa emp)
        {
            var ativse = await _context.AtividadeS.ToListAsync();

            List<AtividadeS> atvs = new List<AtividadeS>();

            // SE O CNPJ ENVIADO PELA PÁGINA 'Details' FOR IGUAL À CNPJ's DO BANCO DE DADOS 'AtividadeS', ENTÃO ESSES CNPJS SÃO ADICIONADOS À UMA LISTA
            foreach (var item in ativse)
            {
                if (emp.Cnpj == item.EmpresaCnpj)
                {

                    atvs.Add(item);
                }
            }

            ViewBag.nome = "Secundárias";

            return View("AtividadeS", atvs);
        }

        // GET: Empresas/AdicionarCadastro
        public IActionResult AdicionarCadastro()
        {
            return View();
        }

        // ESTE MÉTODO RECEBE UM CNPJ E BUSCA INFORMAÇÕES DA EMPRESA E Á ADICIONA AO BANCO DE DADOS
        // POST: Empresas/AdicionarCadastro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarCadastro([Bind("Cnpj")] Empresa empresa)
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri("https://www.receitaws.com.br/v1/cnpj/") };

            string cnpj = empresa.Cnpj;
            cnpj = RemovePontuacao.RmPontCnpj(cnpj); // REMOVE MASCARA/PONTUAÇÕES DO CNPJ RECEBIDO
            var response = await client.GetAsync(cnpj);
            var content = await response.Content.ReadAsStringAsync();

            //CASO HAJA MAIS DE 3 REQUISIÇÕES DE CNPJ EM 1 MINUTO, FEITA PARA A API DA RECEITA FEDERAL, ELA RETORNA STATUS DE ERROR,
            //NESTE CASO, A APLICAÇÃO NOTIFICA QUE O USUÁRIO DEVE ESPERAR UM MOMENTO ATÉ A PROXIMA REQUISIÇÃO
            if (!response.IsSuccessStatusCode)
            {
                TempData["msg"] = "Espere um momento para adicionar um novo CNPJ";
                return RedirectToAction("AdicionarCadastro");
            }
            
            // DESERIALIZA O ARQUIVO JSON RECEBIDO PELA API
            Empresa empresas = JsonConvert.DeserializeObject<Empresa>(content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });


            // VALIDAÇÕES DE CNPJ RECEBIDO
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

        // ESTE MÉTODO APÓS RECEBER UM CNPJ, BUSCA TODAS AS INFORMAÇÕES DA EMPRESA CORRESPONDENTE NO BANCO DE DADOS
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
        }

        // GET: Empresas/Delete/
        public IActionResult Delete()
        {
            return View();
        }

        // ESTE METODO RECEBE UM CNPJ E DELETA TODAS AS INFORMAÇÕES DA EMPRESA CORRESPONDENTE
        // POST: Empresas/Delete/cnpj
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Empresa emp)
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
