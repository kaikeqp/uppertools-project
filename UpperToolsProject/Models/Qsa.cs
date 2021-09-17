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
        [JsonProperty("nome")]
        public string Nome { get; set; }
        [JsonProperty("qual")]
        public string Qual { get; set; }

    }
}