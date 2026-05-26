using API.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace API.Services
{
    public class UsuarioService
    {
        private readonly string caminhoArquivo =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Usuarios.json");

        public UsuarioService()
        {
            Inicializar();
        }

        public bool ValidarLogin(string usuario, string senha)
        {
            List<UsuarioDTO> usuarios = ObterTodos();

            string senhaHash = GerarHashSha256(senha);

            return usuarios.Any(u => u.Usuario == usuario && u.SenhaHash == senhaHash);
        }

        private List<UsuarioDTO> ObterTodos()
        {
            Inicializar();

            string json = File.ReadAllText(caminhoArquivo);

            return JsonSerializer.Deserialize<List<UsuarioDTO>>(json) ?? new List<UsuarioDTO>();
        }

        private void Inicializar()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(caminhoArquivo)!);

            if (!File.Exists(caminhoArquivo))
            {
                List<UsuarioDTO> usuarios = new List<UsuarioDTO>
                {
                    new UsuarioDTO
                    {
                        Usuario = "admin",
                        SenhaHash = GerarHashSha256("123")
                    }
                };

                string json = JsonSerializer.Serialize(
                    usuarios,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                File.WriteAllText(caminhoArquivo, json);
            }
        }

        private string GerarHashSha256(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));

                StringBuilder builder = new StringBuilder();

                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}