using Aplicacao.Service;
using Dominio.Entidades;
using Dominio.Enums;
using Dominio.Interfaces;
using Infra.Repositorios;
using Interface.ApiServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interface
{
    public partial class Principal : Form
    {
        #region Variaveis globais
        // Variavel criada para controle dos programas personalizados 
        private int programaPersonalizadoSelecionado = 0;

        // Variaveis responsáveis pela autenticação da API
        private string tokenApi = string.Empty;
        private string urlApi = string.Empty;
        private bool apiAutenticada = false;

        // Declaração dos services da API
        private ProgramaDeAquecimentoApiService programaDeAquecimentoApiService;
        private MicroondasApiService microondasApiService;

        // Declaração de objetos dos programas de aquecimento de do funcionamento geral do microondas
        private Microondas microondas;
        private MicroondasService microondasService;
        private ProgramaDeAquecimento programaDeAquecimento;
        private List<ProgramaDeAquecimento> programasPreDefinidos;
        private List<ProgramaDeAquecimento> programasPersonalizados;

        // Variaveis anteriores a mudança para API (Deixadas para visualização)
        IProgramaDeAquecimentoRepository repository;
        private ProgramaDeAquecimentoService programaDeAquecimentoService;
        #endregion

        #region Construtor
        public Principal()
        {
            InitializeComponent();
            microondasService = new MicroondasService();
            microondasApiService = new MicroondasApiService();
            programaDeAquecimento = new ProgramaDeAquecimento();
            programasPreDefinidos = new List<ProgramaDeAquecimento>();
            programasPersonalizados = new List<ProgramaDeAquecimento>();
            programaDeAquecimentoApiService = new ProgramaDeAquecimentoApiService();
         
            microondas = new Microondas
            {
                Tempo = 0,
                Potencia = 0,
                PreDefinido = false,
                CaracterDeAquecimento = '.',
                Estado = EstadosEnum.PARADO
            };
            apiAutenticada = false;
            ControlarFuncoesPelaApi();

            // Variaveis anteriores a mudança para API (Deixadas para visualização)
            repository = new ProgramaDeAquecimentoRepository();
            programaDeAquecimentoService = new ProgramaDeAquecimentoService(repository);
        }

        #endregion

        #region Funções de Click
        // Limpa os campos de tempo e potencia
        private void botaoLimpar_Click(object sender, EventArgs e)
        {
            maskedTextBoxTempo.Text = string.Empty;
            maskedTextBoxPotencia.Text = string.Empty; ;
        }
        // Função de click generica para todos os botões numericos
        private void botaoNumerico_Click(object sender, EventArgs e)
        {
            Button botaoNumerico = (Button)sender;
            if (!microondas.PreDefinido)
            {
                Console.Beep(1000, 50);
                AdicionarNumero(botaoNumerico);
            }
        }
        // Função de click generica para todos os botões de programas pré definidos
        private void botaoPreDefinido_Click(object sender, EventArgs e)
        {
            Button botaoPreDefinido = (Button)sender;
            Console.Beep(1000, 50);
            SelecaoPreDefinido(botaoPreDefinido);
        }
        // Botão responsável por iniciar o contador
        private async void botaoIniciar_Click(object sender, EventArgs e)
        {
            if (!VerificaAutenticação())
                return;

            switch (microondas.Estado)
            {
                // Caso esteja parado ele realizará o processo de iniciar o funcionamento do microondas
                case EstadosEnum.PARADO:
                    int tempo = 30;
                    int potencia = 10;

                    if (!string.IsNullOrWhiteSpace(maskedTextBoxTempo.Text))
                    {
                        if (!int.TryParse(maskedTextBoxTempo.Text, out tempo))
                        {
                            MessageBox.Show("Tempo Inválido!");
                            return;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(maskedTextBoxPotencia.Text))
                    {
                        if (!int.TryParse(maskedTextBoxPotencia.Text, out potencia))
                        {
                            MessageBox.Show("Potencia Inválida!");
                            return;
                        }
                    }

                    microondas.Tempo = tempo;
                    microondas.Potencia = potencia;

                    try
                    {
                        Console.Beep(1000, 500);
                        // Esta função realiza a chamada na api para que possa ser feito o inicio do funcionamento
                        await microondasApiService.IniciarAsync(urlApi, tokenApi, tempo, potencia);
                        // Esta função é responsável por fazer o funcionamento apenas visual da tela
                        await microondasService.IniciarAquecimentoAsync(microondas, AtualizaDisplay);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    break;

                // Caso o microondas esteja funcionando ele verifica se está com um programa pré definido,
                // caso não esteja permite adicionar 30 segundos ao tempo
                case EstadosEnum.INICIADO:
                    if (!microondas.PreDefinido)
                    {
                        try
                        {
                            await microondasApiService.AdicionarTempoAsync(urlApi, tokenApi);

                            if (microondas.Tempo + 30 > 120)
                            {
                                microondas.Tempo = 120;
                            }
                            else
                            {
                                microondas.Tempo += 30;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Erro ao adicionar tempo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    break;
                // Se estiver pausado o mesmo continua
                case EstadosEnum.PAUSADO:
                    try
                    {
                        await microondasApiService.ContinuarAsync(urlApi, tokenApi);
                        microondas.Estado = EstadosEnum.INICIADO;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Erro ao continuar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    break;
            }
        }

        private async void botaoParar_Click(object sender, EventArgs e)
        {
            if (!VerificaAutenticação())
                return; ;
            // Caso o microondas esteja funcionando o botao de paurar pausa o funcionamento
            // se ele já estiver pausado o mesmo irá finalizar o aquecimento parando o funcionamento e voltando a tela para o status padrão
            if (microondas.Estado == EstadosEnum.INICIADO)
            {
                try
                {
                    await microondasApiService.PausarAsync(urlApi, tokenApi);
                    microondas.Estado = EstadosEnum.PAUSADO;
                    Console.Beep(1000, 50);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro ao pausar", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
            }
            else
            {
                try
                {
                    await microondasApiService.CancelarAsync(urlApi, tokenApi);

                    textBoxCronometro.Text = string.Empty;
                    maskedTextBoxTempo.Text = string.Empty;
                    maskedTextBoxPotencia.Text = string.Empty;

                    microondas.Tempo = 0;
                    microondas.Potencia = 0;
                    microondas.PreDefinido = false;
                    microondas.CaracterDeAquecimento = '.';
                    microondas.Estado = EstadosEnum.PARADO;

                    ControleDigitacao(microondas.PreDefinido);
                    Console.Beep(1000, 50);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro ao cancelar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        // Botão responsável por controlar qual dos displays os botões da tela irão alterar
        private void botaoTempoPotencia_Click(object sender, EventArgs e)
        {
            botaoTempoPotencia.Text = botaoTempoPotencia.Text == "Tempo" ? "Potencia" : "Tempo";
        }
        // Botão responsável por chamar o cadastro de um novo programa de aquecimento
        private void botaoNovaFuncao_Click(object sender, EventArgs e)
        {
            InvertePanels();
        }
        // Cancelar o cadastro de um nono programa de aquecimento
        private void botaoCancelarNovaFuncao_Click(object sender, EventArgs e)
        {
            LimparCadastroPrograma();
            InvertePanels();
        }
        // Salvar o novo programa de aquecimento
        private async void botaoSalvarProgramaDeAquecimento_Click(object sender, EventArgs e)
        {
            if (!VerificaAutenticação())
                return;

            int tempo = 0;
            int potencia = 0;
            if (!string.IsNullOrWhiteSpace(maskedTextBoxTempoCadastro.Text))
            {
                if (!int.TryParse(maskedTextBoxTempoCadastro.Text, out tempo))
                {
                    MessageBox.Show("Tempo Inválido!");
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(maskedTextBoxPotenciaCadastro.Text))
            {
                if (!int.TryParse(maskedTextBoxPotenciaCadastro.Text, out potencia))
                {
                    MessageBox.Show("Potencia Inválida!");
                    return;
                }
            }
            // Instancia o objeto com os campos preenchidos em tela para a gravação do mesmo no JSON
            programaDeAquecimento = new ProgramaDeAquecimento
            {
                Nome = textBoxNomePrograma.Text,
                Alimento = textBoxAlimento.Text,
                Tempo = tempo,
                Potencia = potencia,
                CaracterDeAquecimento = string.IsNullOrWhiteSpace(maskedTextBoxStringDeAquecimento.Text) ? '\0' : maskedTextBoxStringDeAquecimento.Text[0],
                InstrucoesComplementares = textBoxInstrucoes.Text,
                Personalizado = true
            };
            try
            {
                // Realiza o cadastro no JSON via api
                await programaDeAquecimentoApiService.CadastrarAsync(urlApi, tokenApi, programaDeAquecimento);
                // Faz o carregamento dos programas cadastrados para que possam ser utilizados
                await CarregarProgramasPelaApiAsync();
                // Volta para o estado padrão para um novo cadastro
                programaDeAquecimento = null;
                LimparCadastroPrograma();
                InvertePanels();

                MessageBox.Show("Programa cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao cadastrar programa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        // Seleciona a função personalizada anterior pois optei por mostrar todas em apenas um botão
        private void botaoFuncaoAnterior_Click(object sender, EventArgs e)
        {
            if (programasPersonalizados.Count > 0)
            {
                if (programaPersonalizadoSelecionado != 0)
                    programaPersonalizadoSelecionado--;
                programaDeAquecimento = programasPersonalizados[programaPersonalizadoSelecionado];
                botaoFuncaoPersonalisada.Text = programaDeAquecimento.Nome.ToString();
            }
            else
            {
                MessageBox.Show("Não existem programas personalizados cadastrados no sistema atualmente!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        // Seleciona a próxima função personalizada pois optei por mostrar todas em apenas um botão
        private void botaoProximaFuncao_Click(object sender, EventArgs e)
        {
            if (programasPersonalizados.Count > 0)
            {
                if (programaPersonalizadoSelecionado < programasPersonalizados.Count - 1)
                    programaPersonalizadoSelecionado++;
                programaDeAquecimento = programasPersonalizados[programaPersonalizadoSelecionado];
                botaoFuncaoPersonalisada.Text = programaDeAquecimento.Nome.ToString();
            }
            else
            {
                MessageBox.Show("Não existem programas personalizados cadastrados no sistema atualmente!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        // Botão onde todas as funções personalizadas se encontram
        private void botaoFuncaoPersonalisada_Click(object sender, EventArgs e)
        {
            if (programasPersonalizados == null || programasPersonalizados.Count == 0)
            {
                MessageBox.Show("Não existem programas personalizados cadastrados no sistema atualmente!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }
            programaDeAquecimento = programasPersonalizados[programaPersonalizadoSelecionado];
            SelecaoPersonalizado(programaDeAquecimento);
        }      
        // Chama a rotina para realizar a configuração da API e estabelecer a comunicação com a mesma
        private async void botaoConfigurarApi_Click(object sender, EventArgs e)
        {
            using (ConfiguracaoAPI configuracaoApi = new ConfiguracaoAPI())
            {
                if (configuracaoApi.ShowDialog() == DialogResult.OK)
                {
                    tokenApi = configuracaoApi.TokenApi;
                    urlApi = configuracaoApi.UrlApi;
                    apiAutenticada = configuracaoApi.Autenticado;
                    // habilita os campos de utilizaçaõ após a autenticação da API
                    ControlarFuncoesPelaApi();
                    try
                    {
                        // Carrega os programas de aquecimento
                        await CarregarProgramasPelaApiAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Erro ao carregar programas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
        #endregion

        #region Funções Gerais
        // Verifica se está autenticado ao tentar utilizar alguma função do sistema
        private bool VerificaAutenticação()
        {
            if (!apiAutenticada)
            {
                MessageBox.Show("Autentique-se na API antes de utilizar o micro-ondas.", "API não autenticada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        // Habilita campos para utilização após a comunicação com a API
        private void ControlarFuncoesPelaApi()
        {
            botaoIniciar.Enabled = apiAutenticada;
            botaoParar.Enabled = apiAutenticada;
            botaoNovaFuncao.Enabled = apiAutenticada;
            botaoFuncaoPersonalisada.Enabled = apiAutenticada;
            botaoFuncaoAnterior.Enabled = apiAutenticada;
            botaoProximaFuncao.Enabled = apiAutenticada;
            labelStatusApi.Text = apiAutenticada ? "API: Autenticada" : "API: Não autenticada";
            labelStatusApi.ForeColor = apiAutenticada ? Color.Green : Color.Red;
        }
        // Carrega as duas listas de programas de aquecimentos, os personalizados e os pré definidos
        private async Task CarregarProgramasPelaApiAsync()
        {
            List<ProgramaDeAquecimento> programas = await programaDeAquecimentoApiService.ObterTodosAsync(urlApi, tokenApi);

            programasPreDefinidos = programas.Where(e => e.Personalizado == false).ToList();

            programasPersonalizados = programas.Where(e => e.Personalizado == true).ToList();

            if (programasPersonalizados.Count > 0)
            {
                programaPersonalizadoSelecionado = 0;
                botaoFuncaoPersonalisada.Text = programasPersonalizados[0].Nome;
            }
            else
            {
                botaoFuncaoPersonalisada.Text = string.Empty;
            }
        }
        // Função responsável pela seleção de um programa de aqucimento personalizado
        private void SelecaoPersonalizado(ProgramaDeAquecimento selecionado)
        {
            if (botaoFuncaoPersonalisada.Text != string.Empty)
            {
                microondas.PreDefinido = true;
                if (microondas.Estado == EstadosEnum.INICIADO)
                {
                    microondas.Estado = EstadosEnum.PARADO;
                }
                maskedTextBoxPotencia.Text = selecionado.Potencia.ToString();
                maskedTextBoxTempo.Text = selecionado.Tempo.ToString();
                microondas.CaracterDeAquecimento = selecionado.CaracterDeAquecimento;
                ControleDigitacao(microondas.PreDefinido);
            }
            else
            {
                MessageBox.Show("Não existem programas personalizados cadastrados no sistema atualmente!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        // Alterna entre tela de cadastro e tela de utilização
        private void InvertePanels()
        {
            botaoIniciar.Enabled = !botaoIniciar.Enabled;
            botaoParar.Enabled = !botaoParar.Enabled;
            panelBotoes.Visible = !panelBotoes.Visible;
            panelCadastroNovaFuncao.Visible = !panelCadastroNovaFuncao.Visible;
        }
        // Limpa o cadastro no fim da utilização
        private void LimparCadastroPrograma()
        {
            textBoxAlimento.Text = string.Empty;
            textBoxInstrucoes.Text = string.Empty;
            maskedTextBoxStringDeAquecimento.Text = string.Empty;
            textBoxNomePrograma.Text = string.Empty;
            maskedTextBoxPotenciaCadastro.Text = string.Empty;
            maskedTextBoxTempoCadastro.Text = string.Empty;
            microondas.PreDefinido = false;
            microondas.CaracterDeAquecimento = '.';
            ControleDigitacao(microondas.PreDefinido);
        }
        // Digita o numero nos text box de potencia e tempo
        private void AdicionarNumero(Button botaoPrecionado)
        {
            if (botaoTempoPotencia.Text == "Tempo")
            {
                maskedTextBoxTempo.Text += botaoPrecionado.Text;
            }
            else
            {
                maskedTextBoxPotencia.Text += botaoPrecionado.Text;
            }
        }
        // Responsável por selecionar os programas pré definidos
        private void SelecaoPreDefinido(Button botaoPrecionado)
        {
            if (!VerificaAutenticação())
                return;

            programaDeAquecimento = programasPreDefinidos.FirstOrDefault(e => e.Nome == botaoPrecionado.Text);

            if (programaDeAquecimento == null)
            {
                MessageBox.Show("Programa pré-definido não encontrado. Verifique se os programas foram carregados pela API.", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            microondas.PreDefinido = true;

            if (microondas.Estado == EstadosEnum.INICIADO)
            {
                textBoxCronometro.Text = string.Empty;
                maskedTextBoxTempo.Text = string.Empty;
                maskedTextBoxPotencia.Text = string.Empty;
                microondas.Estado = EstadosEnum.PARADO;
            }

            maskedTextBoxPotencia.Text = programaDeAquecimento.Potencia.ToString();
            maskedTextBoxTempo.Text = programaDeAquecimento.Tempo.ToString();
            microondas.CaracterDeAquecimento = programaDeAquecimento.CaracterDeAquecimento;
            ControleDigitacao(microondas.PreDefinido);
        }
        // Permite digitação ou não de valores quando está com um programa pré definido selecionado
        private void ControleDigitacao(bool permissao)
        {
            maskedTextBoxTempo.Enabled = !permissao;
            maskedTextBoxPotencia.Enabled = !permissao;
            botaoNovaFuncao.Enabled = !permissao;
            botaoTempoPotencia.Enabled = !permissao;
        }
        // Funcionamento do contador
        private void AtualizaDisplay(string texto)
        {
            textBoxCronometro.Text = texto;
        }
        #endregion
    }
}
