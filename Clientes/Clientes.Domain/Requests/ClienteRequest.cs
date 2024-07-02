using MediatR;

namespace Clientes.Domain.Requests
{
    public record ClienteRequest(string Nome, string Cpf, DateTime Nascimento) : IRequest
    {
        public void Validate()
        {
            ClienteRequestValidation validator = new ClienteRequestValidation();

            var results = validator.Validate(this);

            if (!results.IsValid)
            {
                var erro = string.Empty;
                foreach (var failure in results.Errors)
                {
                    erro += $"{failure.PropertyName}: {failure.ErrorMessage} ";
                }
                throw new ArgumentException(erro);
            }
        }
    }
}