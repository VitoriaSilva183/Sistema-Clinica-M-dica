using System;
using System.Collections.Generic;
using System.Data;
using ClinicaMedica.Database;
using ClinicaMedica.Models;
using MySqlConnector;

namespace ClinicaMedica.Repositories
{
    // essa classe mexe na tabela consulta
    public class ConsultaRepository
    {
        private ConexaoBanco _conexao = new ConexaoBanco();

        // marca uma consulta nova
        public void Inserir(Consulta consulta)
        {
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "INSERT INTO consulta (data_consulta, horario, status_consulta, id_paciente, id_medico, id_consultorio) VALUES (@DataConsulta, @Horario, @Status, @PacienteId, @MedicoId, @ConsultorioId)";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    // .Date tira a hora pra salvar so a data
                    cmd.Parameters.AddWithValue("@DataConsulta", consulta.DataConsulta.Date);
                    // o banco guarda como TIME entao mando como TimeSpan
                    cmd.Parameters.AddWithValue("@Horario", consulta.HoraConsulta.ToTimeSpan());
                    cmd.Parameters.AddWithValue("@Status", consulta.Status.ToString());
                    cmd.Parameters.AddWithValue("@PacienteId", consulta.PacienteId);
                    cmd.Parameters.AddWithValue("@MedicoId", consulta.MedicoId);
                    cmd.Parameters.AddWithValue("@ConsultorioId", consulta.ConsultorioId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // muda tudo de uma consulta
        public void Atualizar(Consulta consulta)
        {
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "UPDATE consulta SET data_consulta = @DataConsulta, horario = @Horario, status_consulta = @Status, id_paciente = @PacienteId, id_medico = @MedicoId, id_consultorio = @ConsultorioId WHERE id_consulta = @Id";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@DataConsulta", consulta.DataConsulta.Date);
                    cmd.Parameters.AddWithValue("@Horario", consulta.HoraConsulta.ToTimeSpan());
                    cmd.Parameters.AddWithValue("@Status", consulta.Status.ToString());
                    cmd.Parameters.AddWithValue("@PacienteId", consulta.PacienteId);
                    cmd.Parameters.AddWithValue("@MedicoId", consulta.MedicoId);
                    cmd.Parameters.AddWithValue("@ConsultorioId", consulta.ConsultorioId);
                    cmd.Parameters.AddWithValue("@Id", consulta.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // muda so o status (usado pra cancelar por exemplo)
        public void AtualizarStatus(int id, StatusConsulta status)
        {
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "UPDATE consulta SET status_consulta = @Status WHERE id_consulta = @Id";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Status", status.ToString());
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ve se o medico ja tem consulta nesse mesmo dia e horario
        // retorna true se ja tiver (ai nao pode marcar)
        public bool ExisteConflito(int idMedico, DateTime data, TimeOnly hora)
        {
            using (var conexao = _conexao.ObterConexao())
            {
                // consulta cancelada nao conta como conflito
                string sql = "SELECT COUNT(*) FROM consulta WHERE id_medico = @IdMedico AND data_consulta = @Data AND horario = @Hora AND status_consulta <> 'Cancelada'";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@IdMedico", idMedico);
                    cmd.Parameters.AddWithValue("@Data", data.Date);
                    cmd.Parameters.AddWithValue("@Hora", hora.ToTimeSpan());
                    long quantidade = Convert.ToInt64(cmd.ExecuteScalar());
                    return quantidade > 0;
                }
            }
        }

        // pega todas as consultas (so os ids, sem os nomes)
        public List<Consulta> ListarTodos()
        {
            List<Consulta> lista = new List<Consulta>();
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "SELECT * FROM consulta";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(LerConsulta(reader));
                        }
                    }
                }
            }
            return lista;
        }

        // procura uma consulta pelo id
        public Consulta BuscarPorId(int id)
        {
            Consulta consulta = null;
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "SELECT * FROM consulta WHERE id_consulta = @Id";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            consulta = LerConsulta(reader);
                        }
                    }
                }
            }
            return consulta;
        }

        // apaga a consulta
        public void Remover(int id)
        {
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "DELETE FROM consulta WHERE id_consulta = @Id";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // essa traz as consultas ja com os nomes pra mostrar na tabela da tela
        // usa JOIN pra juntar paciente, medico e consultorio
        public DataTable ListarParaGrade()
        {
            DataTable tabela = new DataTable();
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = @"SELECT c.id_consulta AS Id,
                                      p.nome AS Paciente,
                                      m.nome AS Medico,
                                      m.especialidade AS Especialidade,
                                      co.nome AS Consultorio,
                                      c.data_consulta AS Data,
                                      c.horario AS Hora,
                                      c.status_consulta AS Status
                               FROM consulta c
                               JOIN paciente p ON p.id_paciente = c.id_paciente
                               JOIN medico m ON m.id_medico = c.id_medico
                               JOIN consultorio co ON co.id_consultorio = c.id_consultorio
                               ORDER BY c.data_consulta, c.horario";
                using (var adaptador = new MySqlDataAdapter(sql, conexao))
                {
                    adaptador.Fill(tabela);
                }
            }
            return tabela;
        }

        // funcao auxiliar pra nao repetir o codigo de ler a consulta do banco
        private Consulta LerConsulta(MySqlDataReader reader)
        {
            var c = new Consulta();
            c.Id = reader.GetInt32("id_consulta");
            c.DataConsulta = reader.GetDateTime("data_consulta");
            c.HoraConsulta = TimeOnly.FromTimeSpan(reader.GetTimeSpan("horario"));
            c.Status = Enum.Parse<StatusConsulta>(reader.GetString("status_consulta"), true);
            c.PacienteId = reader.GetInt32("id_paciente");
            c.MedicoId = reader.GetInt32("id_medico");
            c.ConsultorioId = reader.GetInt32("id_consultorio");
            return c;
        }
    }
}
