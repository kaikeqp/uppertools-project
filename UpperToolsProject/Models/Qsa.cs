using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UpperToolsProject.Models
{
    [Table("Qsa")]
    public class Qsa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("qual")]
        [Display(Name ="Qualificação")]
        public string Qual { get; set; }

        [ForeignKey("EmpresaCnpj")]
        [Display(Name = "CNPJ")]
        public string EmpresaCnpj { get; set; }

    }
}