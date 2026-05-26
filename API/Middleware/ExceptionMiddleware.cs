using API.Exceptions;
using System.Text.Json;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (RegraDeNegocioExceptions ex)
            {
                await RespostaPadrao(context, 400, ex.Message);

                RegistrarLog(ex);
            }
            catch (Exception ex)
            {
                await RespostaPadrao(context, 500, "Erro interno do servidor");

                RegistrarLog(ex);
            }
        }

        private async Task RespostaPadrao(HttpContext context, int statusCode, string mensagem)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = statusCode;

            var resposta = new
            {
                sucesso = false,
                mensagem = mensagem
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(resposta));
        }

        private void RegistrarLog(Exception ex)
        {
            Directory.CreateDirectory("Logs");

            string caminho = Path.Combine("Logs", "logs.txt");

            string log =
                $@"
Data: {DateTime.Now}

Mensagem:
{ex.Message}

InnerException:
{ex.InnerException}

StackTrace:
{ex.StackTrace}

------------------------------------------------
";

            File.AppendAllText(caminho, log);
        }
    }
}