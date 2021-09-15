using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UpperToolsProject.Models
{
   
    public class Atividade
    {
        [Key]
        public string Code { get; set; }
        public string Text { get; set; }
    }

}
