using MySqlConnector;

namespace ClinicaMedica.Database
{
    // essa classe só serve pra abrir a conexao com o banco
    public class ConexaoBanco
    {
        // dados pra conectar no mysql. 
        private const string _stringConexao = "Server=localhost;Database=Clinica_Medica;User=root;Password=;";

        // abre a conexao
        public MySqlConnection ObterConexao()
        {
            MySqlConnection conexao = new MySqlConnection(_stringConexao);
            conexao.Open();
            return conexao;
        }
    }
}
