using Clientes.Domain.Requests;

namespace Clientes.Test.ModelFake
{
    public static class ClienteRequestFake
    {
        public static ClienteRequest RequestValida()
        {
            return new ClienteRequest("Fulano da Silva", "12345678900", Convert.ToDateTime("1990-12-12"));
        }

        public static ClienteRequest RequestInvalidaComCpfMenor()
        {
            return new ClienteRequest("Fulano da Silva", "1234567890", Convert.ToDateTime("1990-12-12"));
        }

        public static ClienteRequest RequestInvalidaComCpfMaior()
        {
            return new ClienteRequest("Fulano da Silva", "123456789000", Convert.ToDateTime("1990-12-12"));
        }

        public static ClienteRequest RequestInvalidaSemNome()
        {
            return new ClienteRequest("", "12345678900", Convert.ToDateTime("1990-12-12"));
        }

        public static ClienteRequest RequestInvalidaMenorDeIdade()
        {
            return new ClienteRequest("Fulano da Silva", "12345678900", Convert.ToDateTime("2010-12-12"));
        }

        public static ClienteRequest RequestTotalmenteInvalida()
        {
            return new ClienteRequest("", "1234567890", Convert.ToDateTime("2010-12-12"));
        }
    }
}
