using Humanizer;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System.Drawing;

namespace AgendaFornecedores.Models
{
    public class Fornecedor
    {
        string nome;
        string cnpj;
        string contato;
        string email;
        string anotacao;
        string grupoTrabalho;
        string vencimentoFatura;

        public Fornecedor(string nome, string cnpj, string contato, string email, string anotacao, string grupo_trabalho, string vencimentoFatura)
        {
            this.nome = nome;
            this.cnpj = cnpj;
            this.contato = contato;
            this.email = email;
            this.anotacao = anotacao;
            this.grupoTrabalho = grupo_trabalho;
            this.vencimentoFatura = vencimentoFatura;
        }

        public string Nome { get => nome; set => nome = value; }
        public string Cnpj { get => cnpj; set => cnpj = value; }
        public string Contato { get => contato; set => contato = value; }
        public string Email { get => email; set => email = value; }
        public string Anotacao { get => anotacao; set => anotacao = value; }
        public string Grupo_trabalho { get => grupoTrabalho; set => grupoTrabalho = value; }
        public string VencimentoFatura { get => vencimentoFatura; set => vencimentoFatura = value; }

        public bool Cadastrar(Fornecedor fornecedor)
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
                    colunas = new List<string>{ "nome", "cnpj","contato","email", "anotacao", "grupo_trabalho", "vencimento_fatura"};

                    parametros = new List<string> { fornecedor.Nome, fornecedor.Cnpj, fornecedor.Contato, fornecedor.Email, 
                        fornecedor.Anotacao, fornecedor.Grupo_trabalho, fornecedor.VencimentoFatura };

                    if (SQL.SCadastrar("fornecedores", colunas, parametros))
                    {
                        return true;
                    }

                    return false;
                }

                
                return false;
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
        public static List<Fornecedor> listarFornecedores()
        {
            MySqlConnection con = new MySqlConnection(SQL.SConexao());

            List<Fornecedor> fornecedores = new List<Fornecedor>();
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
                    string grupoTrab = leitor["grupo_trabalho"].ToString();

                    Fornecedor fornecedor = new Fornecedor(nome, cnpj, contato, email, anotacao,grupoTrab,"");
                    fornecedores.Add(fornecedor);
                }

                return fornecedores;

            }

            catch { return fornecedores; }
            finally { con.Close(); }

        }

    }
}
