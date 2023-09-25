using MySql.Data.MySqlClient;

namespace AgendaFornecedores.Models
{
    public class Grupo_permitido
    {
        string nome_grupo;

       public Grupo_permitido(string nome_grupo)
        {
            this.nome_grupo = nome_grupo;  
        }

        public string Nome_grupo { get => nome_grupo; set => nome_grupo = value; }

        public static List<Grupo_permitido> listarGrupos()
        {
            MySqlConnection con = new MySqlConnection(SQL.SConexao());
            List<Grupo_permitido> grupos = new List<Grupo_permitido>();
            try
            {
                con.Open();

                MySqlCommand qry = new MySqlCommand("SELECT * FROM grupos_permitidos", con);
                MySqlDataReader leitor = qry.ExecuteReader();
                while (leitor.Read())
                {
                   Grupo_permitido gp = new Grupo_permitido( leitor["nome_grupos"].ToString());
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
