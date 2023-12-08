using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
using System.Data.SqlClient;
using System.Linq;

namespace AgendaFornecedores.Models
{
    public class Acao
    {
        int id;
        string nomeUsuario;
        DateTime data;
        string action;
        string nomeFornecedor;
        public Acao() { }
        public Acao(int id, string nomeUsuario, string action, DateTime data, string nomeFornecedor)
        {
            this.id = id;
            this.nomeUsuario = nomeUsuario;
            this.action = action;
            this.data = data;
            this.nomeFornecedor = nomeFornecedor;
        }

        public string NomeUsuario { get => nomeUsuario; set => nomeUsuario = value; }


        public DateTime Data { get => data; set => data = value; }
        public string Action { get => action; set => action = value; }

        public string NomeFornecedor { get => nomeFornecedor; set => nomeFornecedor = value; }
        public int Id { get => id; set => id = value; }

        public  bool AdiconarAcao( Acao action)
        {
            SqlConnection con = new (SQL.SConexao());

            try
            {
                con.Open();
                string insert = "insert into acoes(nome_usuario, acao, nome_fornecedor, data)" +
                    $"values('{action.NomeUsuario}','{action.Action}','{action.NomeFornecedor}', '{action.Data}')";
                SqlCommand SqlCommand = new SqlCommand(insert, con); 
                SqlCommand.ExecuteNonQuery();

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
        public static List<Acao> listarAcoes()
        {
            SqlConnection con = new SqlConnection(SQL.SConexao());
            List<Acao> acoes = new List<Acao>();

            try
            {
                con.Open();
                //SELECT TOP 20 * FROM sua_tabela ORDER BY data_insercao DESC;

                SqlCommand SqlCommand = new SqlCommand("select * from acoes order by id desc",con);
                SqlDataReader leitor = SqlCommand.ExecuteReader();

                while (leitor.Read())
                {
                    int id = int.Parse(leitor["id"].ToString());
                    string nome_u = leitor["nome_usuario"].ToString();
                    string acao = leitor["acao"].ToString();
                    DateTime data = (DateTime)leitor["data"];
                    string nome_f = leitor["nome_fornecedor"].ToString();

                    Acao acao1 = new Acao(id,nome_u, acao, data, nome_f);
                    acoes.Add(acao1);
                }

                acoes.Reverse();

                return acoes;
            }
            catch { return acoes; }

            finally { con.Close(); }
        }

        public List<string> VerificaAcaoEnvio(List<Fornecedor> fornecedores)
        {
            List<Acao> acoes = listarAcoes();
            List<string> nomesFornecedores = new List<string>();

            //nenuma açao foi realizada aos fornecedores, quase impossivel
            if(acoes.Count == 0)
            {
                foreach(Fornecedor fornecedor in fornecedores)
                {
                    nomesFornecedores.Add(fornecedor.Nome);
                }
                return nomesFornecedores;
            }

            //prefiltrar lista de ações 
            acoes.RemoveAll(ac => ac.Action != "EnviarNota");

            foreach (Fornecedor fornecedor in fornecedores)
            {
                for (int i = 0; i < acoes.Count; i++)
                {     
                   if(fornecedor.Nome == acoes[i].NomeFornecedor)
                    {
                        if (acoes[i].Action != "EnviarNota" || acoes[i].Action == "EnviarNota" && !(acoes[i].Data.Month <= DateTime.Now.Month))
                        {
                            if (!nomesFornecedores.Contains(acoes[i].NomeFornecedor))
                            {
                                nomesFornecedores.Add(acoes[i].NomeFornecedor);
                            }
                        }
                   }
                }    
            }
            return nomesFornecedores;
        }
    }
}
