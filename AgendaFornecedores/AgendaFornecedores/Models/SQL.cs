using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace AgendaFornecedores.Models
{ 
    public class SQL
    {

        public static string  SConexao()
        { 
            string server = @"TEXTILTI02\SQLEXPRESS";
            string banco = "agenda_fornecedores";
            string usuario = "sa";
            string senha = "Ad#2735G";
            string con = "Data Source=" + server + ";Initial Catalog=" + banco + ";User Id=" + usuario + ";Password=" + senha + ";";

            return con;
        }
        public static bool Procurar( string tabela, List<string> colunas, List<string> parametros) {
            string co = SConexao();
            SqlConnection con = new SqlConnection(co);
            try
            {
                con.Open();
                foreach (string coluna in colunas)
                {
                    foreach(string parametro in parametros)
                    {
                        SqlCommand cmd = new SqlCommand($"select * from {tabela} where {coluna} = @parametro",con);
                        cmd.Parameters.AddWithValue("@parametro", parametro);
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (!reader.HasRows)
                        {
                            reader.Close();
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

        public static bool SCadastrar(string tabela, List<object> colunas, List<object> valores)
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

        internal bool AlterarDados(string tabela, List<string> colunas, List<string> valores)
        {
            //o id se mostrou um atributo necessario agora
            /*UPDATE table_name SET column1 = value1, column2 = value2, ...WHERE condition;*/

            //"UPDATE Tabela SET Propriedade1 = @Valor1, Propriedade2 = @Valor2 WHERE ID = @ID"

            MySqlConnection con = new(SConexao());

            try
            {
                //modelando string SQL
                string colunaValores = $"UPDATE {tabela} SET ";

                for (int i = 0; i < colunas.Count; i++)
                {
                    if (i != colunas.Count - 1) colunaValores += $"{colunas[i]} = @{colunas[i]}, ";
                    else
                    {
                        colunaValores += $"{colunas[i]} = @{colunas[i]} where {colunas[0]} = @{colunas[0]}";
                    }
                }

                con.Open();
                MySqlCommand sqlCommand = new MySqlCommand(colunaValores, con);

                for(int i = 0;i < colunas.Count;i++)
                {
                    sqlCommand.Parameters.AddWithValue($"@{colunas[i]}", valores[i]);
                }
                sqlCommand.ExecuteNonQuery();              
                return true;

            }
            catch(Exception e)
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
