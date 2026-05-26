using Interface.ApiServices;
using System;
using System.Windows.Forms;


namespace Interface
{
    public partial class ConfiguracaoAPI : Form
    {
        public string TokenApi { get; private set; }
        public string UrlApi { get; private set; }
        public bool Autenticado { get; private set; }
        public ConfiguracaoAPI()
        {
            InitializeComponent();

            textBoxUrlApi.Text = "https://localhost:7001/";
            textBoxUsuarioApi.Text = "admin";
        }

        private async void botaoAutenticar_Click(object sender, EventArgs e)
        {
            try
            {
                AuthApiService authApiService = new AuthApiService();

                TokenApi = await authApiService.LoginAsync(textBoxUrlApi.Text, textBoxUsuarioApi.Text, textBoxSenhaApi.Text);

                UrlApi = textBoxUrlApi.Text;
                Autenticado = true;

                MessageBox.Show("API autenticada com sucesso!", "Autenticação", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Autenticado = false;
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
