
using Novell.Directory.Ldap;
using System.Diagnostics.Eventing.Reader;
using System.DirectoryServices.Protocols;

namespace AgendaFornecedores.Models
{
    public class Usuario
    {
        
        //utlizando o LDap para fazer a verificação dos usuarios
        public string Logar(string nomeUsuario, string senhaUsuario)
        {
            try
            {
                using(var connection = new Novell.Directory.Ldap.LdapConnection())
                {
                    connection.Connect("Bigbag.local", 389);

                    connection.Bind($"cn={nomeUsuario},ou=users,dc=exemple,dc=com ",senhaUsuario);

                    if (connection.Bound)
                    {
                        // Autenticado com sucesso
                        //no controller referente a esse modelo vamos gerar a sessao desse usuario
                        return "1";

                    }
                    else
                    {
                        //falha na autentificação
                        return  "0";
                    }
                }

            }

            catch (Exception ex)
            {

                return ex.Message;

            }

        }






        //receberá os metodos CRUD para realizar com os fornecedores

        //string de conecção como banco de dados
        //StringConnection = "Server=localhost;Port=3306;Database=MP_DOTNET6_API;User Id=root;Password=Ad#2735G;"

    }
}
