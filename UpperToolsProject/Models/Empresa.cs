using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace UpperToolsProject.Models
{
    [Table("Empresa")]
    public class Empresa
    {
        [Key]
        [Required(ErrorMessage ="Este campo não pode ficar vazio.")]
        public string Cnpj { get; set; }
        public string DataSituacao { get; set; }
        public string MotivoSituacao { get; set; }
        public string Tipo { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public List<Qsa> Qsa { get; set; }
        public string Situacao { get; set; }
        public string Porte { get; set; }
        public string Abertura { get; set; }
        public string NaturezaJuridica { get; set; }
        public string UltimaAtualizacao { get; set; }
        public string Status { get; set; }
        public string Fantasia { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public string Municipio { get; set; }
        public string Uf { get; set; }
        public string Email { get; set; }
        public string Efr { get; set; }
        public string SituacaoEspecial { get; set; }
        public string DataSituacaoEspecial { get; set; }
        public Atividade[] AtividadePrincipal { get; set; }
        public Atividade[] AtividadesSecundarias { get; set; }
        public string CapitalSocial { get; set; }
    }
}

