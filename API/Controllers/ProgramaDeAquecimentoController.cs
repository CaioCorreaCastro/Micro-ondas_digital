using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProgramaDeAquecimentoController : ControllerBase
    {
        private readonly ProgramaDeAquecimentoService service;

        public ProgramaDeAquecimentoController()
        {
            service = new ProgramaDeAquecimentoService();
        }

        [HttpGet]
        public IActionResult ObterTodos()
        {
            return Ok(new
            {
                sucesso = true,
                dados = service.ObterTodos()
            });
        }

        [HttpPost]
        public IActionResult Cadastrar(ProgramaDeAquecimentoDTO programa)
        {
            service.Cadastrar(programa);

            return Ok(new
            {
                sucesso = true,
                mensagem = "Programa de aquecimento cadastrado com sucesso."
            });
        }

        [HttpDelete("{nome}")]
        public IActionResult Remover(string nome)
        {
            service.Remover(nome);

            return Ok(new
            {
                sucesso = true,
                mensagem = "Programa removido com sucesso."
            });
        }
    }
}