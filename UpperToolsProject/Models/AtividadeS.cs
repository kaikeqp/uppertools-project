using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UpperToolsProject.Models
{
   [Table("AtividadeS")]
    public class AtividadeS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "Texto")]
        [JsonProperty("text")]
        public string Text { get; set; }
        [Display(Name = "Código")]
        [JsonProperty("code")]
        public string Code { get; set; }
        [ForeignKey("EmpresaCnpj")]
        public string EmpresaCnpj { get; set; }
    }

}
