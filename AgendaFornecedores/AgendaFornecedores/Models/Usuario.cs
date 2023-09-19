using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Novell.Directory.Ldap;
using System.Reflection.PortableExecutable;
using System.Diagnostics.Eventing.Reader;
using System.DirectoryServices;
using DirectoryEntry = System.DirectoryServices.DirectoryEntry;

namespace AgendaFornecedores.Models

{
    public class Usuario
    {
        // o preenchimento dos dados serão recolhidos atravez da sessão do windows( descobrir como)

        //receberá os metodos CRUD para realizar com os fornecedores

        //string de conecção como banco de dados
        //StringConnection = "Server=127.0.0.1;Port=5432;Database=MP_DOTNET6_API;User Id=postgres;Password=teste123;"

        public string Logar(string nomeUsuario, string senhaUsuario)
        {
            // Configure a conexão LDAP
            using (DirectoryEntry entry = new("LDAP://BigBag.local"))
            {
                entry.Username = nomeUsuario;
                entry.Password = senhaUsuario;
                try
                {
                    // Tente autenticar o usuário
                    DirectorySearcher searcher = new DirectorySearcher(entry);
                    searcher.Filter = $"(sAMAccountName={nomeUsuario})";
                    SearchResult result = searcher.FindOne();

                    if (result != null)
                    {
                        // O usuário foi autenticado com sucesso
                        return "1";
                    }
                    else
                    {
                        // Autenticação falhou
                        // Exiba uma mensagem de erro
                        return "0";
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message; // Lidere com erros de conexão LDAP
                }

            }
        }

    }
}

