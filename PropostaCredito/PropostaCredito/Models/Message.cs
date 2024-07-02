namespace PropostaCredito.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public DateTime Nascimento { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
