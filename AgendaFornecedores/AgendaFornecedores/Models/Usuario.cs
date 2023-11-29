using System.Data.SqlClient;
using System.DirectoryServices;
using DirectoryEntry = System.DirectoryServices.DirectoryEntry;

namespace AgendaFornecedores.Models
{
    public class Usuario
    {
        string nomeUsuario;
        string senhaUsuario;
        string grupoTrabalho;
        bool midadm;
        bool fulladm;

        public Usuario() { }
        public Usuario(string nomeUsuario,string senhaUsuario,string grupoTrabalho, bool midadm, bool fulladm) { 
            this.nomeUsuario = nomeUsuario;
            this.senhaUsuario = senhaUsuario;
            this.grupoTrabalho = grupoTrabalho;
            this.midadm = midadm;
            this.fulladm = fulladm;
            
        }
        public string NomeUsuario { get => nomeUsuario; set => nomeUsuario = value; }
        public string SenhaUsuario { get => senhaUsuario; set => senhaUsuario = value; }
        public bool Midadm { get => midadm; set => midadm = value; }
        public bool Fulladm { get => fulladm; set => fulladm = value; }
        public string GrupoTrabalho { get => grupoTrabalho; set => grupoTrabalho = value; }

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
                        GrupoAcesso grupoT = verificaAdmin(groposT);
                        if(grupoT != null)
                        {
                            if (grupoT.Fulladm)
                            {
                                //fulladm
                                Usuario us = new Usuario(nomeU, senhaU, grupoT.Nome_grupo, true, true);
                                return us;
                            }
                            else
                            {
                                //midadm
                                Usuario us = new Usuario(nomeU, senhaU, grupoT.Nome_grupo, true, false);
                                return us;
                            }
                        }
                        else
                        {
                        //usuario padrao
                        //o GG_todos é um grupo de trabalho generico,todos os usuarios estao nesse grupo
                        //achar modo de fazer um recolhimento mais especifico do grupo
                        Usuario us = new Usuario(nomeU, senhaU, "GG_Todos", false, false);
                        return us;
                        }
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    return ex; // Lidere com erros de conexão LDAP
                }
            }
        }
        //verificando grupos de acesso do usuario no banco de dados
        public static GrupoAcesso verificaAdmin(List<string> groposT)
        {
            SqlConnection con = new SqlConnection(SQL.SConexao());
            try
            {
                con.Open();
                SqlCommand qry = new SqlCommand("Select * from grupos_acesso", con);
                SqlDataReader leitor = qry.ExecuteReader();

                //enquanto o leitor lê verifica se os grupos sao iguais na lista e no banco de dados
                while (leitor.Read())
                {
                    foreach (string grupo in groposT)
                    {
                        if (grupo == leitor["nome_grupo"].ToString())
                        {
                            GrupoAcesso gt = new GrupoAcesso(int.Parse(leitor["id"].ToString()),
                            leitor["nome_grupo"].ToString(), Convert.ToBoolean(leitor["fulladm"]));

                            return gt;
                        }
                    }
                }

                return null;

            }
            catch (Exception ex)
            {

                return null;
            }

            finally
            {
                con.Close();
            }
        }
    }
}