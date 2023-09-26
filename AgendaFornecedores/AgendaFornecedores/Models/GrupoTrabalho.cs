using MySql.Data.MySqlClient;

namespace AgendaFornecedores.Models
{
    public class GrupoTrabalho
    {
        string nome_grupo;
        string fulladm;
       public GrupoTrabalho(string nome_grupo, string fulladm)
        {
            this.nome_grupo = nome_grupo;  
            this.fulladm = fulladm;
        }

        public string Nome_grupo { get => nome_grupo; set => nome_grupo = value; }
        public string Fulladm { get => fulladm; set => fulladm = value; }

        public static List<GrupoTrabalho> listarGrupos()
        {
            MySqlConnection con = new MySqlConnection(SQL.SConexao());
            List<GrupoTrabalho> grupos = new List<GrupoTrabalho>();
            try
            {
                con.Open();

                MySqlCommand qry = new MySqlCommand("SELECT * FROM grupos_permitidos", con);
                MySqlDataReader leitor = qry.ExecuteReader();
                while (leitor.Read())
                {
                   GrupoTrabalho gp = new GrupoTrabalho(leitor["nome_grupos"].ToString(), leitor["fulladm"].ToString());
                   grupos.Add(gp);
                }
                return grupos; 
            }
            catch (Exception ex) {
                return grupos;
            }
            finally { con.Close(); }

        }


    }
}
