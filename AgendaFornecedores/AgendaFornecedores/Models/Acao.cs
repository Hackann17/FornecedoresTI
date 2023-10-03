using Microsoft.AspNetCore.Mvc.ModelBinding;
using MySql.Data.MySqlClient;

namespace AgendaFornecedores.Models
{
    public class Acao
    {
        int id;
        string nomeUsuario;
        string data;
        string action;
        string nomeFornecedor;

        public Acao() { }
        public Acao(int id, string nomeUsuario, string action, string data, string nomeFornecedor)
        {
            this.id = id;
            this.nomeUsuario = nomeUsuario;
            this.action = action;
            this.data = data;
            this.nomeFornecedor = nomeFornecedor;
        }

        public string NomeUsuario { get => nomeUsuario; set => nomeUsuario = value; }


        public string Data { get => data; set => data = value; }
        public string Action { get => action; set => action = value; }

        public string NomeFornecedor { get => nomeFornecedor; set => nomeFornecedor = value; }
        public int Id { get => id; set => id = value; }


        public  bool AdiconarAcao( Acao action)
        {
            MySqlConnection con = new MySqlConnection(SQL.SConexao());

            try
            {
                con.Open();

                List<object> colunas = new List<object> { "nome_usuario", "acao", "data", "nome_fornecedor"};
                List<object> valores = new List<object> { action.NomeUsuario, action.Action, action.Data, action.NomeFornecedor };

                if(SQL.SCadastrar("acoes", colunas, valores))
                {   
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
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
            MySqlConnection con = new MySqlConnection(SQL.SConexao());
            List<Acao> acoes = new List<Acao>();

            try
            {
                con.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("select * from acoes",con);
                MySqlDataReader leitor = mySqlCommand.ExecuteReader();

                while (leitor.Read())
                {
                    int id = int.Parse(leitor["id"].ToString());
                    string nome_u = leitor["nome_usuario"].ToString();
                    string acao = leitor["acao"].ToString();
                    string data = leitor["data"].ToString();
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
    }
}
