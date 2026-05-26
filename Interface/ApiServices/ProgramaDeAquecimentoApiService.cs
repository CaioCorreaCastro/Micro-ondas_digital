using Dominio.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Interface.ApiServices
{
    public class ProgramaDeAquecimentoApiService
    {
        public async Task<List<ProgramaDeAquecimento>> ObterTodosAsync(string urlApi, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync("api/ProgramaDeAquecimento");

                string json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Erro ao buscar programas da API: " + json);
                }

                ApiResponse<List<ProgramaDeAquecimento>> resposta = JsonConvert.DeserializeObject<ApiResponse<List<ProgramaDeAquecimento>>>(json);

                return resposta.Dados ?? new List<ProgramaDeAquecimento>();
            }
        }

        public async Task CadastrarAsync(string urlApi, string token,ProgramaDeAquecimento programa)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string json = JsonConvert.SerializeObject(programa);

                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("api/ProgramaDeAquecimento", content);

                string resposta = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Erro ao cadastrar programa na API: " + resposta);
                }
            }
        }
    }

    public class ApiResponse<T>
    {
        public bool Sucesso { get; set; }
        public T Dados { get; set; }
        public string Mensagem { get; set; }
    }
}