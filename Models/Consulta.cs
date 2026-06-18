using System;

namespace ClinicaMedica.Models
{
    // representa uma consulta marcada
    public class Consulta
    {
        public int Id { get; set; }
        public DateTime DataConsulta { get; set; }
        public TimeOnly HoraConsulta { get; set; }
        // quando a consulta e criada ja entra como agendada
        public StatusConsulta Status { get; set; } = StatusConsulta.Agendada;
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public int ConsultorioId { get; set; }
    }

    // os status que uma consulta pode ter
    public enum StatusConsulta
    {
        Agendada,
        Concluida,
        Cancelada,
        Reagendada
    }
}
