
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using Humanizer;

namespace AgendaFornecedores.Models
{
    public class Fornecedor
    {
        int id;
        string nome;
        string cnpj;
        string contato;
        string email;
        string anotacao;
        string grupoTrabalho;
        DateOnly vencimentoFatura;

        public Fornecedor() { }
        public Fornecedor(int id, string nome, string cnpj, string contato, string email, string anotacao, string grupo_trabalho, DateOnly vencimentoFatura)
        {
            this.id = id;
            this.nome = nome;
            this.cnpj = cnpj;
            this.contato = contato;
            this.email = email;
            this.anotacao = anotacao;
            this.grupoTrabalho = grupo_trabalho;
            this.vencimentoFatura = vencimentoFatura;
        }

        public string Nome { get => nome; set => nome = value; }
        public string Cnpj { get => cnpj; set => cnpj = value; }
        public string Contato { get => contato; set => contato = value; }
        public string Email { get => email; set => email = value; }
        public string Anotacao { get => anotacao; set => anotacao = value; }
        public string Grupo_trabalho { get => grupoTrabalho; set => grupoTrabalho = value; }
        public DateOnly VencimentoFatura { get => vencimentoFatura; set => vencimentoFatura = value; }
        public int Id { get => id; set => id = value; }


        public bool Cadastrar(Fornecedor fornecedor)
        {
            SqlConnection con = new SqlConnection(SQL.SConexao());
            //INSERT INTO `agenda_fornecedores`.`grupos_permitidos` (`id`, `nome_grupos`) VALUES ('0', 'GG_TI');

            try
            {
                List<string> colunas = new List<string> { "cnpj" };
                List<string> parametros = new List<string> { fornecedor.Cnpj };

                if (SQL.Procurar("fornecedores", colunas, parametros))
                {
                    con.Open();

                    string insert = $"insert into fornecedores(id, nome, cnpj, contato, email, anotacao, grupo_trabalho, vencimento_fatura)" +
                    $"values(NEXT VALUE FOR fornecedores_seq,'{fornecedor.Nome}','{fornecedor.Cnpj}','{fornecedor.Contato}','{fornecedor.Email}','{fornecedor.Anotacao}','{fornecedor.Grupo_trabalho}','{fornecedor.VencimentoFatura.ToString("yyyy-MM-dd")}')";

                    SqlCommand mySqlCommand = new SqlCommand(insert, con);

                    mySqlCommand.ExecuteNonQuery();

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }

        }
        public static List<Fornecedor> listarFornecedores()
        {
            SqlConnection con = new SqlConnection(SQL.SConexao());

            List<Fornecedor> fornecedores = new List<Fornecedor>();
            try
            {
                con.Open();

                //organizar lista de fornecedores pelo mais proximo do vencimento da fatura...

                SqlCommand sqlCommand = new SqlCommand("select * from fornecedores", con);
                SqlDataReader leitor = sqlCommand.ExecuteReader();

                while (leitor.Read())
                {
                    int id = int.Parse(leitor["id"].ToString());
                    string nome = leitor["nome"].ToString();
                    string cnpj = leitor["cnpj"].ToString();
                    string contato = leitor["contato"].ToString();
                    string email = leitor["email"].ToString();
                    string anotacao = leitor["anotacao"].ToString();
                    string grupoTrab = leitor["grupo_trabalho"].ToString();

                    //o ParseExact gera um objeto datetime modelave, sigando o contexto da aplicação e algumas diretrizes
                    DateOnly vencimento = DateOnly.ParseExact(leitor["vencimento_fatura"].ToString(), "dd/MM/yyyy 00:00:00", null);

                    Fornecedor fornecedor = new(id, nome, cnpj, contato, email, anotacao, grupoTrab, vencimento);

                    fornecedores.Add(fornecedor);
                }

                //organiza a lista de forma crescente da data mais recente para a mais antiga
                fornecedores = fornecedores.OrderBy(forn => forn.VencimentoFatura).ToList();

                return fornecedores;

            }

            catch (Exception ex) { return fornecedores; }
            finally { con.Close(); }

        }

        public void AnaliseVencFatura(List<Fornecedor> fornecedores, Usuario us)
        {
            DateOnly hoje = DateOnly.FromDateTime(DateTime.Now);
            foreach (var fornecedor in fornecedores)
            {
                // verificar se a data estiver ha uma semana de vencer
                if (fornecedor.VencimentoFatura.Month == hoje.Month)
                {
                    if (fornecedor.VencimentoFatura.Day - hoje.Day <= 7)
                    {
                        if (CriaEnviaEmail(fornecedor,us))
                        {
                            //atualizar no banco de dados
                            AtualizaVencimentoFatura(fornecedor);
                        }
                    }
                }  
            }
        }

        private static bool CriaEnviaEmail(Fornecedor fornecedor, Usuario us)
        {
            // Set your SMTP server and credentials
            SmtpClient smtpClient = new("email-ssl.com.br")
            {
                Port = 587, // Porta SMTP para ssl
                Credentials = new NetworkCredential("pedro.godinho@bsstex.com.br","Bsspgodinho1#e"),
                EnableSsl = true, // Necessário para locaweb SSL/TLS
            };

            // Create and configure the email message
            MailMessage mailMessage = new MailMessage($"pedro.godinho@bsstex.com.br", $"{us.NomeUsuario}@bsstex.com.br",
                $"Fatura vencimento {fornecedor.VencimentoFatura}", $"A fatura do fornecedor {fornecedor.Nome} já venceu ou vencerá na semana que vem no dia {fornecedor.VencimentoFatura}," +
                $" preste atenção. A próxima data de pagamento referente ao fornecedor já foi atribuida ao sistema.")
            {
                IsBodyHtml = false // Set to true if the body contains HTML content
            };

            try
           {
                //Envia a mensagem baseado nos dados do objeto Email 
                smtpClient.Send(mailMessage);
                return true;

           }
           catch(Exception ex) 
           {
               return false;
           }
           finally
           {
                // Libera os recursos
                mailMessage.Dispose();
           }
        }

        private static void AtualizaVencimentoFatura(Fornecedor forn)
        {
            //após o email ser enviado o sistema atualiza o banco de dados
            SqlConnection con = new SqlConnection(SQL.SConexao());
            try
            {
                con.Open();
                string inserir = $"UPDATE fornecedores SET vencimento_fatura = DATEADD(day, 30, vencimento_fatura) where id = {forn.Id};" ;
                SqlCommand sqlCommand = new SqlCommand(inserir,con);
                sqlCommand.ExecuteNonQuery();
            }
            catch(Exception ex){}
            finally { con.Close(); }
        }

        public bool DeletarFornecedor(int id)
        {
            SqlConnection con = new(SQL.SConexao());

            try
            {
                con.Open();
                string delete = $"DELETE FROM fornecedores WHERE id = {id}";
                SqlCommand mySqlCommand = new SqlCommand(delete, con);

                if (mySqlCommand.ExecuteNonQuery() != null) return true;
                return false;
            }
            catch
            { return false; }
            finally
            {
                con.Close();
            }
        }
        internal bool AlterarFornecedor(Fornecedor fornecedor)
        {
            //"UPDATE Tabela SET Propriedade1 = @Valor1, Propriedade2 = @Valor2 WHERE ID = @ID"
            SqlConnection con = new(SQL.SConexao());
            try
            {
                con.Open();
                string alterar = $"UPDATE fornecedores SET nome = '{fornecedor.Nome}', cnpj = '{fornecedor.Cnpj}'," +
                    $" contato = '{fornecedor.Contato}', email = '{fornecedor.Email}', anotacao = '{fornecedor.Anotacao}'," +
                    $"vencimento_fatura = '{fornecedor.VencimentoFatura.ToString("yyyy/MM/dd")}' where id = {fornecedor.Id}";
                SqlCommand mySql = new SqlCommand(alterar, con);

                if (mySql.ExecuteNonQuery() != null) return true;
                return false;
            }
            catch { return false; }
            finally { con.Close(); }
        }
    }
}
