using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpperToolsProject.Models;

namespace UpperToolsProject.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            Database.EnsureCreated(); // CRIA O BANCO DE DADOS CASO ELE NÃO EXISTA E INICIALIZA O ESQUEMA DE BANCO DE DADOS
        }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Qsa> Qsa { get; set; }
        public DbSet<Atividade> Atividade { get; set; }
        public DbSet<AtividadeS> AtividadeS { get; set; }

    }
}
