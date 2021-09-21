using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UpperToolsProject.Models
{
    [Table("Empresa")]
    public class Empresa
    {
        [Key]
        [Display(Name = "CNPJ")]
        [Required(ErrorMessage = "Preencha este campo.")]
        [RegularExpression("[0-9]{2}\\.?[0-9]{3}\\.?[0-9]{3}\\/?[0-9]{4}\\-?[0-9]{2}", ErrorMessage = "Digite um CNPJ válido")]
        public string Cnpj { get; set; }

        [StringLength(100)]
        [Display(Name ="Nome")]
        [Required(ErrorMessage ="Preencha este campo")]
        public string Nome { get; set; }

        [Display(Name = "Data da situacção")]
        [StringLength(20)]
        public string DataSituacao { get; set; }

        [Display(Name = "Motivo da situação")]
        [StringLength(200)]
        public string MotivoSituacao { get; set; }

        [StringLength(200)]
        public string Tipo { get; set; }

        [StringLength(20)]
        public string Telefone { get; set; }

        public List<Qsa> Qsa { get; set; }

        [Display(Name ="Situação")]
        [StringLength(200)]
        public string Situacao { get; set; }

        [StringLength(25)]
        public string Porte { get; set; }

        [StringLength(20)]
        [Display(Name = "Data de abertura")]
        public string Abertura { get; set; }

        [Display(Name = "Natureza Jurídica")]
        [StringLength(200)]
        public string NaturezaJuridica { get; set; }

        [StringLength(200)]
        public string UltimaAtualizacao { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [Display(Name = "Nome fantasia")]
        [StringLength(200)]
        public string Fantasia { get; set; }

        [StringLength(200)]
        public string Logradouro { get; set; }

        [Display(Name = "Número")]
        [StringLength(8)]
        public string Numero { get; set; }

        [StringLength(200)]
        public string Complemento { get; set; }

        [Display(Name = "CEP")]
        [StringLength(15)]
        public string Cep { get; set; }

        [StringLength(200)]
        public string Bairro { get; set; }

        [Display(Name = "Município")]
        [StringLength(100)]
        public string Municipio { get; set; }

        [Display(Name = "UF")]
        [StringLength(5)]
        public string Uf { get; set; }

        [StringLength(70)]
        public string Email { get; set; }

        [Display(Name = "Ente Federativo Responsável")]
        [StringLength(200)]
        public string Efr { get; set; }

        [Display(Name = "Situação especial")]
        [StringLength(200)]
        public string SituacaoEspecial { get; set; }

        [Display(Name = "Data da situação especial")]
        [StringLength(20)]
        public string DataSituacaoEspecial { get; set; }

        [Display(Name = "Valor do capital social")]
        [StringLength(25)]
        public string CapitalSocial { get; set; }

        [JsonProperty("atividade_principal")]
        [Display(Name = "Atividade principal")]
        [StringLength(200)]
        public List<Atividade> AtividadePrincipal { get; set; }

        [JsonProperty("atividades_secundarias")]
        [Display(Name = "Atividades secundárias")]
        [StringLength(200)]
        public List<AtividadeS> AtividadesSecundarias { get; set; }
    }
}

