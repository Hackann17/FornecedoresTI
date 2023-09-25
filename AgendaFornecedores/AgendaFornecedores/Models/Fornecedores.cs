using Humanizer;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System.Drawing;

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

                 List<string> colunas = new List<string> { "cnpj"};
                List<string> parametros = new List<string> { fornecedor.Cnpj };

                if (SQL.Procurar("fornecedores",colunas, parametros))
                {
                    //se nenhum fornecedor com essa caracteristicas for achado ele 
                    MySqlCommand mySqlCommand = new MySqlCommand("INSERT INTO fornecedores( nome, cnpj, contato, email, anotacao)" +
                    " VALUES(@nome, @cnpj, @contato, @email, @anotacao)", con);

                    mySqlCommand.Parameters.AddWithValue("@nome", fornecedor.Nome);
                    mySqlCommand.Parameters.AddWithValue("@cnpj", fornecedor.Cnpj);
                    mySqlCommand.Parameters.AddWithValue("@contato", fornecedor.Contato);
                    mySqlCommand.Parameters.AddWithValue("@email", fornecedor.Email);
                    mySqlCommand.Parameters.AddWithValue("@anotacao", fornecedor.Anotacao);

                    mySqlCommand.ExecuteNonQuery();
                   
                    return true;
                }

                
                return false;
            }
            catch (Exception ex)
            {
                return false;

            }
            finally
            {
                //con.Close();
            }

        }
        public static List<Fornecedores> listarFornecedores()
        {
            MySqlConnection con = new MySqlConnection(SQL.SConexao());

            List<Fornecedores> fornecedores = new List<Fornecedores>();
            try
            {
                con.Open();

                MySqlCommand sqlCommand = new MySqlCommand("select * from fornecedores", con);
                MySqlDataReader leitor = sqlCommand.ExecuteReader();

                while (leitor.Read())
                {
                    string nome = leitor["nome"].ToString();
                    string cnpj = leitor["cnpj"].ToString();
                    string contato = leitor["contato"].ToString();
                    string email = leitor["email"].ToString();
                    string anotacao = leitor["anotacao"].ToString();

                    Fornecedores fornecedor = new Fornecedores(nome, cnpj, contato, email, anotacao);
                    fornecedores.Add(fornecedor);
                }

                return fornecedores;

            }

            catch { return fornecedores; }
            finally { con.Close(); }

        }

    }
}
