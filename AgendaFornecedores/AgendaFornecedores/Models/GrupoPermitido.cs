using MySql.Data.MySqlClient;

namespace AgendaFornecedores.Models
{
    public class GrupoPermitido
    {
        string nome_grupo;
        string fulladm;

        public GrupoPermitido() { }
       public GrupoPermitido(string nome_grupo, string fulladm)
        {
            this.nome_grupo = nome_grupo;  
            this.fulladm = fulladm;
        }

        public string Nome_grupo { get => nome_grupo; set => nome_grupo = value; }
        public string Fulladm { get => fulladm; set => fulladm = value; }

        public static List<GrupoPermitido> listarGrupos()
        {
            MySqlConnection con = new MySqlConnection(SQL.SConexao());
            List<GrupoPermitido> grupos = new List<GrupoPermitido>();
            try
            {
                con.Open();

                MySqlCommand qry = new MySqlCommand("SELECT * FROM grupos_permitidos", con);
                MySqlDataReader leitor = qry.ExecuteReader();
                while (leitor.Read())
                {
                   GrupoPermitido gp = new GrupoPermitido(leitor["nome_grupos"].ToString(), leitor["fulladm"].ToString());
                   grupos.Add(gp);
                }
                return grupos; 
            }
            catch (Exception ex) {
                return grupos;
            }
            finally { con.Close(); }
        }

        public bool AdicionarGrupo(GrupoPermitido GT)
        {
            MySqlConnection con = new MySqlConnection(SQL.SConexao());
            try
            {
                con.Open();
                List<string> colunas = new List<string> { "nome_grupos","fulladm"};
                List<string> parametros = new List<string> {GT.Nome_grupo, GT.Fulladm};

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
    }
}
