using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;

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
                   
                }

                return false;
            }
            
            catch (Exception ex)
            {
                return false;

            }
            finally { con.Close(); }
            
        }

        public static bool SCadastrar(string tabela, List<string> colunas, List<string> valores)
        {
            string co = SConexao();
            MySqlConnection con = new MySqlConnection(co);
            try
            {
                //MODELANDO A STRING DE COMANDO SQL
                string comando = $"INSERT INTO {tabela}(";
                string dados = "VALUES(";

                for (int i = 0; i < colunas.Count; i++)
                {
                    if (i == colunas.Count-1)
                    {
                        comando += $"{colunas[i]}) ";
                        dados += $"@{colunas[i]})";
                    }
                    else
                    {
                        comando += $"{colunas[i]}, ";
                        dados += $"@{colunas[i]}, ";
                    }
                }

                comando += dados;

                con.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(comando,con);

                for (int i = 0; i < valores.Count; i++)
                {
                    mySqlCommand.Parameters.AddWithValue($"@{colunas[i]}", valores[i]);
                }

                mySqlCommand.ExecuteNonQuery();

                return true;
            }
            catch(Exception ex)
            { return false; 
            }
            finally 
            {
               con.Close();
            }
        }

        //DELETE FROM Customers WHERE CustomerName='Alfreds Futterkiste';
        public static bool SDeletar(string tabela, List <string> colunas, List<string> parametros )
        {
            MySqlConnection co = new MySqlConnection(SQL.SConexao());
            try
            {
                //modelando string de comando
                string comDel = $"DELETE FROM {tabela} WHERE ";

                for (int i = 0; i < colunas.Count ; i++)
                {
                    comDel += $" {colunas[i]} = @{parametros[i]}";

                    co.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand(comDel, co);
                    mySqlCommand.Parameters.AddWithValue($"@{parametros[i]}", parametros[i]);

                    mySqlCommand.ExecuteNonQuery();
                }
                return true;


            }
            catch (Exception ex) { return false; }

            finally { co.Close(); }


        }
    }
}
