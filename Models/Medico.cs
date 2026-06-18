namespace ClinicaMedica.Models
{
    // representa um medico
    public class Medico
    {
        public int Id { get; set; }
        public string CRM { get; set; }
        public string Nome { get; set; }
        public string Especialidade { get; set; }
        public string Telefone { get; set; }
    }
}
