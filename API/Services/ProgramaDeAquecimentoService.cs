using API.Exceptions;
using API.Models;
using System.Text.Json;

namespace API.Services
{
    public class ProgramaDeAquecimentoService
    {
        private readonly string caminhoArquivo =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ProgramaDeAquecimento.json");

        public ProgramaDeAquecimentoService()
        {
            Inicializar();
        }

        public List<ProgramaDeAquecimentoDTO> ObterTodos()
        {
            Inicializar();

            string json = File.ReadAllText(caminhoArquivo);

            return JsonSerializer.Deserialize<List<ProgramaDeAquecimentoDTO>>(json) ?? new List<ProgramaDeAquecimentoDTO>();
        }

        public void Cadastrar(ProgramaDeAquecimentoDTO programa)
        {
            Inicializar();

            Validar(programa);

            List<ProgramaDeAquecimentoDTO> programas = ObterTodos();

            bool caractereJaExiste = programas.Any(p => p.CaracterDeAquecimento == programa.CaracterDeAquecimento);

            if (caractereJaExiste || programa.CaracterDeAquecimento == '.')
            {
                throw new RegraDeNegocioExceptions("Não podem haver dois programas de aquecimento com o mesmo caractere de aquecimento.");
            }

            programa.Personalizado = true;

            programas.Add(programa);

            SalvarTodos(programas);
        }

        public void Remover(string nome)
        {
            Inicializar();

            List<ProgramaDeAquecimentoDTO> programas = ObterTodos();

            ProgramaDeAquecimentoDTO programa = programas.FirstOrDefault(p => p.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

            if (programa == null)
            {
                throw new RegraDeNegocioExceptions("Programa não encontrado.");
            }

            if (!programa.Personalizado)
            {
                throw new RegraDeNegocioExceptions("Programas pré-definidos não podem ser removidos.");
            }

            programas.Remove(programa);

            SalvarTodos(programas);
        }

        private void Validar(ProgramaDeAquecimentoDTO programa)
        {
            if (string.IsNullOrWhiteSpace(programa.Nome) || string.IsNullOrWhiteSpace(programa.Alimento) || programa.Tempo <= 0 ||
                programa.Potencia <= 0 || programa.CaracterDeAquecimento == '\0')
            {
                throw new RegraDeNegocioExceptions("Nome, alimento, tempo, potência e caractere de aquecimento são obrigatórios.");
            }

            if (programa.Potencia < 1 || programa.Potencia > 10)
            {
                throw new RegraDeNegocioExceptions("A potência informada deve estar entre 1 e 10.");
            }
        }

        private void Inicializar()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(caminhoArquivo)!);

            if (!File.Exists(caminhoArquivo))
            {
                SalvarTodos(CriarProgramasPadrao());
            }
        }

        private void SalvarTodos(List<ProgramaDeAquecimentoDTO> programas)
        {
            string json = JsonSerializer.Serialize(
                programas,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            File.WriteAllText(caminhoArquivo, json);
        }

        private List<ProgramaDeAquecimentoDTO> CriarProgramasPadrao()
        {
            return new List<ProgramaDeAquecimentoDTO>
            {
                new ProgramaDeAquecimentoDTO
                {
                    Nome = "Pipoca",
                    Alimento = "Pipoca (de micro-ondas)",
                    Tempo = 180,
                    Potencia = 7,
                    CaracterDeAquecimento = '+',
                    InstrucoesComplementares =
                        "Observar o barulho de estouros do milho, caso houver um intervalo de mais de 10 segundos entre um estouro e outro, interrompa o aquecimento.",
                    Personalizado = false
                },

                new ProgramaDeAquecimentoDTO
                {
                    Nome = "Leite",
                    Alimento = "Leite",
                    Tempo = 300,
                    Potencia = 5,
                    CaracterDeAquecimento = '~',
                    InstrucoesComplementares =
                        "Cuidado com aquecimento de líquidos, o choque térmico aliado ao movimento do recipiente pode causar fervura imediata causando risco de queimaduras.",
                    Personalizado = false
                },

                new ProgramaDeAquecimentoDTO
                {
                    Nome = "Carnes de boi",
                    Alimento = "Carne em pedaço ou fatias",
                    Tempo = 840,
                    Potencia = 4,
                    CaracterDeAquecimento = '*',
                    InstrucoesComplementares =
                        "Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para o descongelamento uniforme.",
                    Personalizado = false
                },

                new ProgramaDeAquecimentoDTO
                {
                    Nome = "Frango",
                    Alimento = "Frango (qualquer corte)",
                    Tempo = 480,
                    Potencia = 7,
                    CaracterDeAquecimento = '#',
                    InstrucoesComplementares =
                        "Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para o descongelamento uniforme.",
                    Personalizado = false
                },

                new ProgramaDeAquecimentoDTO
                {
                    Nome = "Feijão",
                    Alimento = "Feijão congelado",
                    Tempo = 480,
                    Potencia = 9,
                    CaracterDeAquecimento = '@',
                    InstrucoesComplementares =
                        "Deixe o recipiente destampado e em casos de plástico, cuidado ao retirar o recipiente pois o mesmo pode perder resistência em altas temperaturas.",
                    Personalizado = false
                }
            };
        }
    }
}
