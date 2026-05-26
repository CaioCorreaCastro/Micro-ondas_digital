using Dominio.Entidades;
using Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class ProgramaDeAquecimento
    {
        public string Nome { get; set; }
        public string Alimento { get; set; }
        public int Tempo { get; set; }
        public int Potencia { get; set; }
        public char CaracterDeAquecimento { get; set; }
        public string InstrucoesComplementares { get; set; }
        public bool Personalizado{ get; set; }
    }
}
