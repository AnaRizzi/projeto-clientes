namespace CartaoCredito.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Nome { get; set; } = default!;
        public string Cpf { get; set; } = default!;
        public DateTime Nascimento { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
