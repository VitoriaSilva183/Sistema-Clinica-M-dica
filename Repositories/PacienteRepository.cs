using System;
using System.Collections.Generic;
using ClinicaMedica.Database;
using ClinicaMedica.Models;
using MySqlConnector;

namespace ClinicaMedica.Repositories
{
    // essa classe mexe na tabela paciente do banco
    public class PacienteRepository
    {
        private ConexaoBanco _conexao = new ConexaoBanco();

        // salva um paciente novo
        public void Inserir(Paciente paciente)
        {
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "INSERT INTO paciente (nome, cpf, data_nascimento, endereco, telefone) VALUES (@Nome, @CPF, @DataNascimento, @Endereco, @Telefone)";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Nome", paciente.Nome);
                    cmd.Parameters.AddWithValue("@CPF", paciente.CPF);
                    cmd.Parameters.AddWithValue("@DataNascimento", paciente.DataNascimento);
                    cmd.Parameters.AddWithValue("@Endereco", paciente.Endereco);
                    cmd.Parameters.AddWithValue("@Telefone", paciente.Telefone);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // muda os dados de um paciente que ja existe
        public void Atualizar(Paciente paciente)
        {
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "UPDATE paciente SET nome = @Nome, cpf = @CPF, data_nascimento = @DataNascimento, endereco = @Endereco, telefone = @Telefone WHERE id_paciente = @Id";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Nome", paciente.Nome);
                    cmd.Parameters.AddWithValue("@CPF", paciente.CPF);
                    cmd.Parameters.AddWithValue("@DataNascimento", paciente.DataNascimento);
                    cmd.Parameters.AddWithValue("@Endereco", paciente.Endereco);
                    cmd.Parameters.AddWithValue("@Telefone", paciente.Telefone);
                    cmd.Parameters.AddWithValue("@Id", paciente.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // pega todos os pacientes e devolve numa lista
        public List<Paciente> ListarTodos()
        {
            List<Paciente> lista = new List<Paciente>();
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "SELECT * FROM paciente";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var p = new Paciente();
                            p.Id = reader.GetInt32("id_paciente");
                            p.Nome = reader.GetString("nome");
                            p.CPF = reader.GetString("cpf");
                            p.DataNascimento = reader.GetDateTime("data_nascimento");
                            p.Endereco = reader.GetString("endereco");
                            p.Telefone = reader.GetString("telefone");
                            lista.Add(p);
                        }
                    }
                }
            }
            return lista;
        }

        // procura um paciente pelo id
        public Paciente BuscarPorId(int id)
        {
            Paciente paciente = null;
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "SELECT * FROM paciente WHERE id_paciente = @Id";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            paciente = new Paciente();
                            paciente.Id = reader.GetInt32("id_paciente");
                            paciente.Nome = reader.GetString("nome");
                            paciente.CPF = reader.GetString("cpf");
                            paciente.DataNascimento = reader.GetDateTime("data_nascimento");
                            paciente.Endereco = reader.GetString("endereco");
                            paciente.Telefone = reader.GetString("telefone");
                        }
                    }
                }
            }
            return paciente;
        }

        // apaga o paciente do banco
        public void Remover(int id)
        {
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "DELETE FROM paciente WHERE id_paciente = @Id";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
