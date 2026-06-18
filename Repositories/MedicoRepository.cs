using System;
using System.Collections.Generic;
using ClinicaMedica.Database;
using ClinicaMedica.Models;
using MySqlConnector;

namespace ClinicaMedica.Repositories
{
    // essa classe mexe na tabela medico
    public class MedicoRepository
    {
        private ConexaoBanco _conexao = new ConexaoBanco();

        // salva um medico novo
        public void Inserir(Medico medico)
        {
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "INSERT INTO medico (nome, crm, especialidade, telefone) VALUES (@Nome, @CRM, @Especialidade, @Telefone)";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Nome", medico.Nome);
                    cmd.Parameters.AddWithValue("@CRM", medico.CRM);
                    cmd.Parameters.AddWithValue("@Especialidade", medico.Especialidade);
                    cmd.Parameters.AddWithValue("@Telefone", medico.Telefone);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // muda os dados de um medico
        public void Atualizar(Medico medico)
        {
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "UPDATE medico SET nome = @Nome, crm = @CRM, especialidade = @Especialidade, telefone = @Telefone WHERE id_medico = @Id";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Nome", medico.Nome);
                    cmd.Parameters.AddWithValue("@CRM", medico.CRM);
                    cmd.Parameters.AddWithValue("@Especialidade", medico.Especialidade);
                    cmd.Parameters.AddWithValue("@Telefone", medico.Telefone);
                    cmd.Parameters.AddWithValue("@Id", medico.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // pega todos os medicos
        public List<Medico> ListarTodos()
        {
            List<Medico> lista = new List<Medico>();
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "SELECT * FROM medico";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var m = new Medico();
                            m.Id = reader.GetInt32("id_medico");
                            m.Nome = reader.GetString("nome");
                            m.CRM = reader.GetString("crm");
                            m.Especialidade = reader.GetString("especialidade");
                            m.Telefone = reader.GetString("telefone");
                            lista.Add(m);
                        }
                    }
                }
            }
            return lista;
        }

        // procura um medico pelo id
        public Medico BuscarPorId(int id)
        {
            Medico medico = null;
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "SELECT * FROM medico WHERE id_medico = @Id";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            medico = new Medico();
                            medico.Id = reader.GetInt32("id_medico");
                            medico.Nome = reader.GetString("nome");
                            medico.CRM = reader.GetString("crm");
                            medico.Especialidade = reader.GetString("especialidade");
                            medico.Telefone = reader.GetString("telefone");
                        }
                    }
                }
            }
            return medico;
        }

        // apaga o medico
        public void Remover(int id)
        {
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "DELETE FROM medico WHERE id_medico = @Id";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // pega as especialidades sem repetir (pra usar na hora de agendar)
        public List<string> ListarEspecialidades()
        {
            List<string> lista = new List<string>();
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "SELECT DISTINCT especialidade FROM medico WHERE especialidade IS NOT NULL ORDER BY especialidade";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(reader.GetString("especialidade"));
                        }
                    }
                }
            }
            return lista;
        }

        // pega so os medicos de uma especialidade
        public List<Medico> ListarPorEspecialidade(string especialidade)
        {
            List<Medico> lista = new List<Medico>();
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "SELECT * FROM medico WHERE especialidade = @Especialidade";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Especialidade", especialidade);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var m = new Medico();
                            m.Id = reader.GetInt32("id_medico");
                            m.Nome = reader.GetString("nome");
                            m.CRM = reader.GetString("crm");
                            m.Especialidade = reader.GetString("especialidade");
                            m.Telefone = reader.GetString("telefone");
                            lista.Add(m);
                        }
                    }
                }
            }
            return lista;
        }
    }
}
