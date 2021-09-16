using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UpperToolsProject.Models
{
    [Table("Qsa")]
    public class Qsa
    {
        [Key]
        public string Nome { get; set; }
        public string Qual { get; set; }

    }
}