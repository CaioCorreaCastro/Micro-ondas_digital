using API.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MicroondasController : ControllerBase
    {
        private static int tempoAtual = 0;
        private static int potenciaAtual = 0;
        private static string estadoAtual = "PARADO";

        [HttpPost("iniciar")]
        public IActionResult Iniciar([FromBody] IniciarMicroondasRequest request)
        {
            if (request.Tempo <= 0)
            {
                throw new RegraDeNegocioExceptions("O tempo deve ser maior que zero.");
            }

            if (request.Potencia < 1 || request.Potencia > 10)
            {
                throw new RegraDeNegocioExceptions("A potência deve estar entre 1 e 10.");
            }

            tempoAtual = request.Tempo;
            potenciaAtual = request.Potencia;
            estadoAtual = "INICIADO";

            return Ok(new
            {
                sucesso = true,
                mensagem = "Aquecimento iniciado com sucesso.",
                dados = new
                {
                    tempo = tempoAtual,
                    potencia = potenciaAtual,
                    estado = estadoAtual
                }
            });
        }

        [HttpPost("pausar")]
        public IActionResult Pausar()
        {
            if (estadoAtual != "INICIADO")
            {
                throw new RegraDeNegocioExceptions("Só é possível pausar um aquecimento iniciado.");
            }

            estadoAtual = "PAUSADO";

            return Ok(new
            {
                sucesso = true,
                mensagem = "Aquecimento pausado.",
                dados = new
                {
                    tempo = tempoAtual,
                    potencia = potenciaAtual,
                    estado = estadoAtual
                }
            });
        }

        [HttpPost("continuar")]
        public IActionResult Continuar()
        {
            if (estadoAtual != "PAUSADO")
            {
                throw new RegraDeNegocioExceptions("Só é possível continuar um aquecimento pausado.");
            }

            estadoAtual = "INICIADO";

            return Ok(new
            {
                sucesso = true,
                mensagem = "Aquecimento retomado.",
                dados = new
                {
                    tempo = tempoAtual,
                    potencia = potenciaAtual,
                    estado = estadoAtual
                }
            });
        }

        [HttpPost("cancelar")]
        public IActionResult Cancelar()
        {
            tempoAtual = 0;
            potenciaAtual = 0;
            estadoAtual = "PARADO";

            return Ok(new
            {
                sucesso = true,
                mensagem = "Aquecimento cancelado.",
                dados = new
                {
                    tempo = tempoAtual,
                    potencia = potenciaAtual,
                    estado = estadoAtual
                }
            });
        }

        [HttpPost("adicionar-tempo")]
        public IActionResult AdicionarTempo()
        {
            if (estadoAtual != "INICIADO")
            {
                throw new RegraDeNegocioExceptions("Só é possível adicionar tempo com o micro-ondas iniciado.");
            }

            tempoAtual += 30;

            if (tempoAtual > 120)
            {
                tempoAtual = 120;
            }

            return Ok(new
            {
                sucesso = true,
                mensagem = "Tempo adicionado com sucesso.",
                dados = new
                {
                    tempo = tempoAtual,
                    potencia = potenciaAtual,
                    estado = estadoAtual
                }
            });
        }

        [HttpGet("status")]
        public IActionResult Status()
        {
            return Ok(new
            {
                sucesso = true,
                dados = new
                {
                    tempo = tempoAtual,
                    potencia = potenciaAtual,
                    estado = estadoAtual
                }
            });
        }
    }

    public class IniciarMicroondasRequest
    {
        public int Tempo { get; set; }
        public int Potencia { get; set; }
    }
}