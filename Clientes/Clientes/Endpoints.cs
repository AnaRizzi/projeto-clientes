using Clientes.Domain.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clientes
{
    public static class Endpoints
    {
        public static void MapEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/cliente/cadastro", async (IMediator m, [FromBody] ClienteRequest usuario) =>
            {
                try
                {
                    usuario.Validate();
                    await m.Send(usuario);
                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
                .WithName("CadastroCliente")
                .WithTags("CadastroCliente")
                .Produces(200);
        }
    }
}
