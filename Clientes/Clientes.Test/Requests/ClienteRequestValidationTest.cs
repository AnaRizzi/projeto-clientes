using Clientes.Domain.Requests;
using Clientes.Test.ModelFake;

namespace Clientes.Test.Requests
{
    public class ClienteRequestValidationTest
    {
        [Fact]
        public void DadaRequestValidaRetornarSucesso()
        {
            var request = ClienteRequestFake.RequestValida();

            request.Validate();

            Assert.NotNull(request);
        }

        [Theory]
        [MemberData(nameof(RequestsInvalidas))]
        public void DadaRequestInvalidaDeveRetornarErro(ClienteRequest request)
        {
            Assert.Throws<ArgumentException>(() => request.Validate());
        }

        public static IEnumerable<object[]> RequestsInvalidas()
        {
            yield return new object[] { ClienteRequestFake.RequestInvalidaMenorDeIdade() };
            yield return new object[] { ClienteRequestFake.RequestInvalidaSemNome() };
            yield return new object[] { ClienteRequestFake.RequestInvalidaComCpfMenor() };
            yield return new object[] { ClienteRequestFake.RequestInvalidaComCpfMaior() };
            yield return new object[] { ClienteRequestFake.RequestTotalmenteInvalida() };
        }
    }
}
