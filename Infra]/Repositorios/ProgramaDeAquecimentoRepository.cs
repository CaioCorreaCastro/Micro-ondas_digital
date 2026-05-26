using Dominio.Entidades;
using Dominio.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repositorios
{
    public class ProgramaDeAquecimentoRepository : IProgramaDeAquecimentoRepository
    {
        private readonly string caminhoArquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Data", "ProgramaDeAquecimento.json");
        public List<ProgramaDeAquecimento> ObterTodosProgramasDeAquecimento()
        {
            if (!File.Exists(caminhoArquivo))
            {
                return new List<ProgramaDeAquecimento>();
            }
            string json = File.ReadAllText(caminhoArquivo);
            return JsonConvert.DeserializeObject<List<ProgramaDeAquecimento>>(json);
        }

        public void salvar(ProgramaDeAquecimento programaDeAquecimento)
        {
            string jsonAtual = File.ReadAllText(caminhoArquivo);

            List<ProgramaDeAquecimento> programas = JsonConvert.DeserializeObject<List<ProgramaDeAquecimento>>(jsonAtual) ?? new List<ProgramaDeAquecimento>();

            programas.Add(programaDeAquecimento);

            string json = JsonConvert.SerializeObject(programas, Formatting.Indented);

            File.WriteAllText(caminhoArquivo, json);
        }


        public void Inicializar()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(caminhoArquivo));

            if (!File.Exists(caminhoArquivo))
            {
                List<ProgramaDeAquecimento> programas = CriarProgramasPadrao();
                string json = JsonConvert.SerializeObject(programas, Formatting.Indented);

                File.WriteAllText(caminhoArquivo, json);
            }
        }
        private List<ProgramaDeAquecimento> CriarProgramasPadrao()
        {
            return new List<ProgramaDeAquecimento>
            {
                new ProgramaDeAquecimento
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

                new ProgramaDeAquecimento
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

                new ProgramaDeAquecimento
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

                new ProgramaDeAquecimento
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

                new ProgramaDeAquecimento
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
