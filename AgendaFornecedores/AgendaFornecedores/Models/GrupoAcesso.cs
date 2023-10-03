using MySql.Data.MySqlClient;

namespace AgendaFornecedores.Models
{
    public class GrupoAcesso
    {
        int id;
        string nome_grupo;
        int fulladm;

        public GrupoAcesso() { }
       public GrupoAcesso(int id, string nome_grupo, int fulladm)
        {
            this.id = id;
            this.nome_grupo = nome_grupo;  
            this.fulladm = fulladm;
        }

        public string Nome_grupo { get => nome_grupo; set => nome_grupo = value; }
        public int Fulladm { get => fulladm; set => fulladm = value; }
        public int Id { get => id; set => id = value; }

        public static List<GrupoAcesso> listarGrupos()
        {
            MySqlConnection con = new MySqlConnection(SQL.SConexao());
            List<GrupoAcesso> grupos = new List<GrupoAcesso>();
            try
            {
                con.Open();

                MySqlCommand qry = new MySqlCommand("SELECT * FROM grupos_permitidos", con);
                MySqlDataReader leitor = qry.ExecuteReader();
                while (leitor.Read())
                {
                   GrupoAcesso gp = new GrupoAcesso(int.Parse(leitor["id"].ToString()), leitor["nome_grupos"].ToString(), int.Parse(leitor["fulladm"].ToString()));
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
            MySqlConnection con = new MySqlConnection(SQL.SConexao());
            try
            {
                con.Open();
                List<object> colunas = new List<object> { "nome_grupos","fulladm"};
                List<object> parametros = new List<object> {GT.Nome_grupo, GT.Fulladm.ToString()};

                if (SQL.SCadastrar("grupos_permitidos", colunas, parametros)) return true;
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

        internal bool ApagarGrupoAcesso(string nomeGrupo)
        {
            try
            {
                return true;
            }
            catch (Exception ex) { return false;}

        }
    }
}
