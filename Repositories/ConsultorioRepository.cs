using System;
using System.Collections.Generic;
using ClinicaMedica.Database;
using ClinicaMedica.Models;
using MySqlConnector;

namespace ClinicaMedica.Repositories
{
    // essa classe mexe na tabela consultorio
    public class ConsultorioRepository
    {
        private ConexaoBanco _conexao = new ConexaoBanco();

        // salva um consultorio novo
        public void Inserir(Consultorio consultorio)
        {
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "INSERT INTO consultorio (nome, endereco, numero, complemento) VALUES (@Nome, @Endereco, @Numero, @Complemento)";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Nome", consultorio.Nome);
                    cmd.Parameters.AddWithValue("@Endereco", consultorio.Endereco);
                    cmd.Parameters.AddWithValue("@Numero", consultorio.Numero);
                    cmd.Parameters.AddWithValue("@Complemento", consultorio.Complemento);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // muda os dados do consultorio
        public void Atualizar(Consultorio consultorio)
        {
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "UPDATE consultorio SET nome = @Nome, endereco = @Endereco, numero = @Numero, complemento = @Complemento WHERE id_consultorio = @Id";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Nome", consultorio.Nome);
                    cmd.Parameters.AddWithValue("@Endereco", consultorio.Endereco);
                    cmd.Parameters.AddWithValue("@Numero", consultorio.Numero);
                    cmd.Parameters.AddWithValue("@Complemento", consultorio.Complemento);
                    cmd.Parameters.AddWithValue("@Id", consultorio.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // pega todos os consultorios
        public List<Consultorio> ListarTodos()
        {
            List<Consultorio> lista = new List<Consultorio>();
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "SELECT * FROM consultorio";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var c = new Consultorio();
                            c.Id = reader.GetInt32("id_consultorio");
                            c.Nome = reader.GetString("nome");
                            c.Endereco = reader.GetString("endereco");
                            c.Numero = reader.GetString("numero");
                            c.Complemento = reader.GetString("complemento");
                            lista.Add(c);
                        }
                    }
                }
            }
            return lista;
        }

        // procura um consultorio pelo id
        public Consultorio BuscarPorId(int id)
        {
            Consultorio consultorio = null;
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "SELECT * FROM consultorio WHERE id_consultorio = @Id";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            consultorio = new Consultorio();
                            consultorio.Id = reader.GetInt32("id_consultorio");
                            consultorio.Nome = reader.GetString("nome");
                            consultorio.Endereco = reader.GetString("endereco");
                            consultorio.Numero = reader.GetString("numero");
                            consultorio.Complemento = reader.GetString("complemento");
                        }
                    }
                }
            }
            return consultorio;
        }

        // apaga o consultorio
        public void Remover(int id)
        {
            using (var conexao = _conexao.ObterConexao())
            {
                string sql = "DELETE FROM consultorio WHERE id_consultorio = @Id";
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
