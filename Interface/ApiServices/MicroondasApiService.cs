using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Interface.ApiServices
{
    public class MicroondasApiService
    {
        public async Task IniciarAsync(string urlApi, string token, int tempo, int potencia)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var request = new
                {
                    Tempo = tempo,
                    Potencia = potencia
                };

                string json = JsonConvert.SerializeObject(request);

                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("api/microondas/iniciar", content);

                string resposta = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Erro ao iniciar micro-ondas na API: " + resposta);
                }
            }
        }

        public async Task PausarAsync(string urlApi, string token)
        {
            await PostSemBodyAsync(urlApi, token, "api/microondas/pausar");
        }

        public async Task ContinuarAsync(string urlApi, string token)
        {
            await PostSemBodyAsync(urlApi, token, "api/microondas/continuar");
        }

        public async Task CancelarAsync(string urlApi, string token)
        {
            await PostSemBodyAsync(urlApi, token, "api/microondas/cancelar");
        }

        public async Task AdicionarTempoAsync(string urlApi, string token)
        {
            await PostSemBodyAsync(urlApi, token, "api/microondas/adicionar-tempo");
        }

        private async Task PostSemBodyAsync(string urlApi, string token, string endpoint)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.PostAsync(endpoint, null);

                string resposta = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Erro ao executar ação na API: " + resposta);
                }
            }
        }
    }
}