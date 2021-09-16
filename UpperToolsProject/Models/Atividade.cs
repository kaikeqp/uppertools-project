using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UpperToolsProject.Models
{
   [Table("Atividade")]
    public class Atividade
    {
        [Key]
        public string Code { get; set; }
        public string Text { get; set; }
    }

}
