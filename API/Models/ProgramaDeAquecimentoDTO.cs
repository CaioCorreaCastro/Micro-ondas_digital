namespace API.Models
{
    public class ProgramaDeAquecimentoDTO
    {
        public string Nome { get; set; }
        public string Alimento { get; set; }
        public int Tempo { get; set; }
        public int Potencia { get; set; }
        public char CaracterDeAquecimento { get; set; }
        public string InstrucoesComplementares { get; set; }
        public bool Personalizado { get; set; }
    }
}
