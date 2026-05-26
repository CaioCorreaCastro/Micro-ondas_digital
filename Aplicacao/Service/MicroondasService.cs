using Dominio.Entidades;
using Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aplicacao.Service
{
    public class MicroondasService
    {
        public async Task<bool> IniciarAquecimentoAsync(Microondas microondas, Action<string> atualizaDisplay)
        {
            try
            {
                string contador = "";
                if ((microondas.Tempo < 1 || microondas.Tempo > 120) && !microondas.PreDefinido)
                {
                    MessageBox.Show("O tempo informado deve estar entre 1 e 120 segundos!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (microondas.Potencia < 0 || microondas.Potencia > 10)
                {
                    MessageBox.Show("A potencia informada deve estar entre 1 e 10!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                microondas.Estado = EstadosEnum.INICIADO;

                var tarefaBeep = Task.Run(() => TocarBeepEnquantoAquece(microondas));

                while (microondas.Tempo > 0 && microondas.Estado != EstadosEnum.PARADO)
                {
                    if(microondas.Estado == EstadosEnum.PAUSADO)
                    {
                        await Task.Delay(200);
                        continue;
                    }
                    contador += new string(microondas.CaracterDeAquecimento, microondas.Potencia) + " ";
                    atualizaDisplay(contador);
                    microondas.Tempo--;
                    await Task.Delay(1000);
                }
                atualizaDisplay(contador + " Aquecimento concluído");                
                microondas.Estado = EstadosEnum.PARADO;
                Console.Beep(1800, 800);
                Console.Beep(1800, 800);
                Console.Beep(1800, 800);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao tentar iniciar o aquecimento! \n\r" + ex.Message);
            }
        }

        private async Task TocarBeepEnquantoAquece(Microondas microondas)
        {
            while (microondas.Estado == EstadosEnum.INICIADO)
            {
                Console.Beep(200, microondas.Tempo * 1000);
            }
        }
    }
}
