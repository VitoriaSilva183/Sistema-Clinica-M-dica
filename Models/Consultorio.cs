namespace ClinicaMedica.Models
{
    // representa a sala/consultorio onde a consulta acontece
    public class Consultorio
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
    }
}
