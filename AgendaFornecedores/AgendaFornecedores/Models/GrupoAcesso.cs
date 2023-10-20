using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace AgendaFornecedores.Models
{
    public class GrupoAcesso
    {
        int id;
        string nome_grupo;
        bool fulladm;

        public GrupoAcesso() { }
       public GrupoAcesso(int id, string nome_grupo, bool fulladm)
        {
            this.id = id;
            this.nome_grupo = nome_grupo;  
            this.fulladm = fulladm;
        }

        public string Nome_grupo { get => nome_grupo; set => nome_grupo = value; }
        public bool Fulladm { get => fulladm; set => fulladm = value; }
        public int Id { get => id; set => id = value; }

        public static List<GrupoAcesso> listarGrupos()
        {
            SqlConnection con = new SqlConnection(SQL.SConexao());
            List<GrupoAcesso> grupos = new List<GrupoAcesso>();
            try
            {
                con.Open();

                SqlCommand qry = new SqlCommand("SELECT * FROM grupos_acesso", con);
                SqlDataReader leitor = qry.ExecuteReader();
                while (leitor.Read())
                {
                    int id = int.Parse(leitor["id"].ToString());
                    string nomesG = leitor["nome_grupo"].ToString();
                    bool adm = bool.Parse(leitor["fulladm"].ToString());

                   GrupoAcesso gp = new GrupoAcesso(id, nomesG, adm);
                   grupos.Add(gp);
                }
                return grupos; 
            }
            catch (Exception ex) {
                return grupos;
            }
            finally { con.Close(); }
        }

        public bool AdicionarGrupo(GrupoAcesso GT)
        {
            SqlConnection con = new SqlConnection(SQL.SConexao());
            try
            {
                con.Open();
                string adicionar = "insert into grupos_acesso(nome_grupo, fulladm)" +
                   $"values('{GT.Nome_grupo}', {Convert.ToInt32(GT.Fulladm)})";

                SqlCommand mySqlCommand = new SqlCommand(adicionar, con);

                if (mySqlCommand.ExecuteNonQuery() != null) return true;
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

        internal bool Apagar(int idGrupo)
        {
            //DELETE FROM tabela WHERE col = valor
            SqlConnection con = new SqlConnection(SQL.SConexao());
            try
            {
                con.Open();

                string deletar = $"DELETE FROM grupos_acesso WHERE id = {idGrupo}";

                SqlCommand mySqlCommand = new SqlCommand(deletar, con);

                if(mySqlCommand.ExecuteNonQuery() != null ) return true;
                return false;
            }
            catch (Exception ex) { return false;}
            finally { con.Close(); }

        }
    }
}
