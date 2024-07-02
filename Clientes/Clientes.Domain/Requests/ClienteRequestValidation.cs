using FluentValidation;

namespace Clientes.Domain.Requests
{
    public class ClienteRequestValidation : AbstractValidator<ClienteRequest>
    {
        public ClienteRequestValidation()
        {
            RuleFor(x => x.Nome).NotNull().NotEmpty().WithMessage("Nome não pode ser vazio ou nulo.");
            RuleFor(x => x.Cpf).NotNull().NotEmpty().WithMessage("Cpf não pode ser vazio ou nulo.");
            RuleFor(x => x.Cpf).Matches("\\d{11}").WithMessage("Cpf deve conter apenas dígitos.");
            RuleFor(x => x.Cpf).Length(11).WithMessage("Cpf deve conter 11 dígitos.");
            RuleFor(x => x.Nascimento).LessThan(DateTime.Now.AddYears(-18)).WithMessage("O cliente deve ser maior de idade.");
        }
    }
}
