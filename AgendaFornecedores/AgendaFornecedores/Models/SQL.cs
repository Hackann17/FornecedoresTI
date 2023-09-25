using MySql.Data.MySqlClient;

namespace AgendaFornecedores.Models
{ 
    public class SQL
    {
        //aqui estarão diversos comandos do SQL 
        public static string SConexao()
        {
            return "Server=localhost;Port=3306;Database=agenda_fornecedores;User Id=root;Password=Ad#2735G";
        }

        public static bool Procurar( string tabela, List<string> colunas, List<string> parametros) {
            string co = SConexao();
            MySqlConnection con = new MySqlConnection(co);

            try
            {
                con.Open();
                foreach (string coluna in colunas)
                {
                    foreach(string parametro in parametros)
                    {
                        MySqlCommand cmd = new MySqlCommand($"select * from {tabela} where {coluna} = @parametro",con);
                        cmd.Parameters.AddWithValue("@parametro", parametro);
                        MySqlDataReader reader = cmd.ExecuteReader();

                        if (!reader.HasRows)
                        {
                            return true;
                        }
                    }
                    return false;
                }

                return true;
            }
            
            catch (Exception ex)
            {
                return false;

            }
            finally { con.Close(); }
            
        }


    }
}
