using Dominio.Entidades;
using Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aplicacao.Service
{
    public class ProgramaDeAquecimentoService
    {
        private readonly IProgramaDeAquecimentoRepository programaDeAquecimentoRepository;

        public ProgramaDeAquecimentoService(IProgramaDeAquecimentoRepository programaDeAquecimentoRepository)
        {
            this.programaDeAquecimentoRepository = programaDeAquecimentoRepository;
        }

        public bool Cadastrar(ProgramaDeAquecimento programaDeAquecimento)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(programaDeAquecimento.Nome) || string.IsNullOrWhiteSpace(programaDeAquecimento.Alimento) ||
                    programaDeAquecimento.Potencia == 0 || programaDeAquecimento.CaracterDeAquecimento == '\0')
                {
                    MessageBox.Show("As informações de nome do programa, alimento, potência, caractere de aquecimento e tempo deverão obrigatoriamente ser preenchidos!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                List<ProgramaDeAquecimento> programaDeAquecimentosCadastrados = programaDeAquecimentoRepository.ObterTodosProgramasDeAquecimento();
                if (programaDeAquecimento.Potencia < 0 || programaDeAquecimento.Potencia > 10)
                {
                    MessageBox.Show("A potencia informada deve estar entre 1 e 10!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (programaDeAquecimentosCadastrados.Exists(e => e.CaracterDeAquecimento == programaDeAquecimento.CaracterDeAquecimento) || 
                   programaDeAquecimento.CaracterDeAquecimento == '.')
                {
                    MessageBox.Show("Não podem haver dois programas de aquecimento com o mesmo caracter de aquecimento!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                programaDeAquecimentoRepository.salvar(programaDeAquecimento);
                return true;
            }
            catch( Exception ex)
            {
                throw new Exception("Não foi possivel realizar o cadastro de um novo programa de aquecimento! Verifique!\n\r" + ex.Message);
            }
        }
    }
}
