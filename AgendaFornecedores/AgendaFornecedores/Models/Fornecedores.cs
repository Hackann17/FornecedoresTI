using MySql.Data.MySqlClient;

namespace AgendaFornecedores.Models
{
    public class Fornecedores
    {
        string nome;
        string cnpj;
        string contato;
        string email;
        string anotacao;

        public Fornecedores(string nome, string cnpj, string contato, string email, string anotacao)
        {
            this.nome = nome;
            this.cnpj = cnpj;
            this.contato = contato;
            this.email = email;
            this.anotacao = anotacao;
        }

        public string Nome { get => nome; set => nome = value; }
        public string Cnpj { get => cnpj; set => cnpj = value; }
        public string Contato { get => contato; set => contato = value; }
        public string Email { get => email; set => email = value; }
        public string Anotacao { get => anotacao; set => anotacao = value; }
        


        public bool Cadastrar(Fornecedores fornecedor)
        {
            MySqlConnection con = new MySqlConnection(SQL.SConexao());
            //INSERT INTO `agenda_fornecedores`.`grupos_permitidos` (`id`, `nome_grupos`) VALUES ('0', 'GG_TI');

            try
            {       
                con.Open();
                MySqlCommand mySqlCommand = new MySqlCommand("INSERT INTO fornecedores( nome, cnpj, contato, email, anotacao)" +
                    " VALUES(@nome, @cnpj, @contato, @email, @anotacao)",con);

                mySqlCommand.Parameters.AddWithValue("@nome", fornecedor.Nome);
                mySqlCommand.Parameters.AddWithValue("@cnpj", fornecedor.Cnpj);
                mySqlCommand.Parameters.AddWithValue("@contato", fornecedor.Contato);
                mySqlCommand.Parameters.AddWithValue("@email", fornecedor.Email);
                mySqlCommand.Parameters.AddWithValue("@anotacao", fornecedor.Anotacao);

                mySqlCommand.ExecuteNonQuery();

                con.Close();

                return true;

            }
            catch (Exception ex)
            {
                return false;

            }
            finally 
            { 
                con.Close(); 
            }
        }
    }
}
