using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UpperToolsProject.Tools.RemovePontuacao
{
    public class RemovePontuacao
    {
        public static string RmPontCnpj(string cnpj)
        {
            return cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
        }
    }
}
