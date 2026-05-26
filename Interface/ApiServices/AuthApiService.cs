using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Interface.ApiServices
{
    public class AuthApiService
    {
        public async Task<string> LoginAsync(string urlApi, string usuario, string senha)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi);

                HttpResponseMessage response = await client.PostAsync($"api/auth/login?usuario={usuario}&senha={senha}", null);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Falha ao autenticar com a API.");
                }

                string json = await response.Content.ReadAsStringAsync();

                LoginResponse resposta = JsonConvert.DeserializeObject<LoginResponse>(json);

                return resposta.Token;
            }
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }
}