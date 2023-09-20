using MySql.Data.MySqlClient;
using System.DirectoryServices;
using DirectoryEntry = System.DirectoryServices.DirectoryEntry;

namespace AgendaFornecedores.Models
{
    public class Usuario
    {
        string nomeUsuario;
        string senhaUsuario;
        bool admin;

        public Usuario(string nomeUsuario,string senhaUsuario, bool admin) { 
            this.nomeUsuario = nomeUsuario;
            this.senhaUsuario = senhaUsuario;
            this.admin = admin;
        }

        public string NomeUsuario { get => nomeUsuario; set => nomeUsuario = value; }
        public string SenhaUsuario { get => senhaUsuario; set => senhaUsuario = value; }
        public bool Admin { get => admin; set => admin = value; }

        public static object Logar(string nomeU, string senhaU)
        {
            // Configure a conexão LDAP
            using (DirectoryEntry entry = new("LDAP://BigBag.local"))
            {
                entry.Username = nomeU;
                entry.Password = senhaU;
                try
                {
                    // Tente autenticar o usuário
                    DirectorySearcher searcher = new DirectorySearcher(entry);
                    searcher.Filter = $"(sAMAccountName={nomeU})";
                    searcher.PropertiesToLoad.Add("sAMAccountName"); // Nome de usuário
                    searcher.PropertiesToLoad.Add("memberOf");
                    SearchResult result = searcher.FindOne();

                    if (result != null)
                    {
                        List<string> groposT = new List<string>();
                        // Obtem os grupos de trabalho
                        if (result.Properties.Contains("memberOf"))
                        {
                            foreach (string groupDn in result.Properties["memberOf"])
                            {
                                DirectoryEntry groupEntry = new DirectoryEntry("LDAP://" + groupDn);
                                string groupName = groupEntry.Properties["cn"].Value.ToString();
                                groupEntry.Close();
                                groposT.Add(groupName);

                            }
                        }

                        //verifica se o usuario está nos grupos administradores ou nao                        
                        Usuario us = new Usuario(nomeU, senhaU,verificaGrupo(groposT));
                        return us;
                    }
                    else
                    {
                        // Autenticação falhou
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    return ex; // Lidere com erros de conexão LDAP
                }

            }
        }

        //verificando grupos de acesso do usuario no banco de dados
        public static bool verificaGrupo(List<string> groposT)
        {
            MySqlConnection con = new MySqlConnection(SQL.SConexao());
            try
            {
                con.Open();
                MySqlCommand qry = new MySqlCommand("Select * from grupos_permitidos", con);
                MySqlDataReader leitor = qry.ExecuteReader();

                //enquanto o leitor lê verifica se os grupos sao iguais na lista e no banco de dados
                while (leitor.Read())
                {
                    foreach (string grupo in groposT)
                    {
                        if (grupo == leitor["nome_grupos"].ToString())
                        {
                            return true;

                        }
                    }
                }

                return false;

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
    }
}