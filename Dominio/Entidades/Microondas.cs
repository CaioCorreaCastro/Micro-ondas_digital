using Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Microondas
    {
        public int Tempo { get; set; }
        public int Potencia { get; set; }
        public bool PreDefinido { get; set; }
        public char CaracterDeAquecimento { get; set; }
        public EstadosEnum Estado { get; set; }
    }
}
