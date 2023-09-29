using Humanizer;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System.Drawing;
using ZstdSharp.Unsafe;

namespace AgendaFornecedores.Models
{
    public class Fornecedor
    {
        int id;
        string nome;
        string cnpj;
        string contato;
        string email;
        string anotacao;
        string grupoTrabalho;
        string vencimentoFatura;

        public Fornecedor() { }
        public Fornecedor(int id, string nome, string cnpj, string contato, string email, string anotacao, string grupo_trabalho, string vencimentoFatura)
        {
            this.id = id;
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
        public int Id { get => id; set => id = value; }

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

                    if (SQL.SCadastrar("fornecedores", colunas, parametros))return true;
 
                    return false;
                }        
                return false;
            }
            catch
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
                    int id = int.Parse(leitor["id"].ToString());
                    string nome = leitor["nome"].ToString();
                    string cnpj = leitor["cnpj"].ToString();
                    string contato = leitor["contato"].ToString();
                    string email = leitor["email"].ToString();
                    string anotacao = leitor["anotacao"].ToString();
                    string grupoTrab = leitor["grupo_trabalho"].ToString();
                    string vencimento = leitor["vencimento_fatura"].ToString();

                    Fornecedor fornecedor = new(id, nome, cnpj, contato, email, anotacao,grupoTrab,vencimento);
                    fornecedores.Add(fornecedor);
                }

                return fornecedores;

            }

            catch (Exception ex){ return fornecedores; }
            finally { con.Close(); }

        }

        public bool DeletarFornecedor(string cnpj)
        {
            MySqlConnection con = new(SQL.SConexao());

            try
            {
                con.Open();

                List<string> colunas = new List<string>{"cnpj"};
                List<string> parametros = new List<string> { cnpj };
                if(SQL.SDeletar("fornecedores",colunas,parametros))return true;
                return false;
            }
            catch
            { return false; }
            finally
            {
                con.Close();
            }
        }

        internal bool AlterarFornecedor(Fornecedor fornecedor)
        {
            try
            {
                List<string> cols = new List<string> { "id","nome","cnpj","contato","email","anotacao","grupo_trabalho","vencimento_fatura"};
                List<string> valores = new List<string> {fornecedor.Id.ToString(), fornecedor.Nome, fornecedor.Cnpj,fornecedor.Contato,fornecedor.Email,
                fornecedor.anotacao,fornecedor.Grupo_trabalho,fornecedor.VencimentoFatura};

                SQL sQL = new SQL();
                if (sQL.AlterarDados("fornecedores", cols, valores)) return true;

                return false;
            }
            catch { return false; }

        }
    }
}
