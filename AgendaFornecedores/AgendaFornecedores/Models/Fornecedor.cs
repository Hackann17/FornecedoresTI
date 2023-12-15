
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Mime;
using AgendaFornecedores.Views.Home;
using MySqlConnector;
using MySql.Data.MySqlClient;

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

                if (SQL.Procurar("Fornecedores", colunas, parametros))
                {
                    con.Open();

                    string insert = $"insert into Fornecedores(id, nome, cnpj, contato, email, anotacao, grupo_trabalho, vencimento_fatura)" +
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

                SqlCommand sqlCommand = new SqlCommand("select * from Fornecedores", con);
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

        public bool DeletarFornecedor(Fornecedor fornecedor)
        {
            SqlConnection con = new(SQL.SConexao());
            try
            {
                con.Open();
                string delete = $"DELETE FROM Fornecedores WHERE id = {fornecedor.Id}";
                SqlCommand mySqlCommand = new SqlCommand(delete, con);

                if (mySqlCommand.ExecuteNonQuery() != null)
                {
                    return true;
                }
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
               Fornecedor forn = SelecionaFornecedor(fornecedor.Id);
                //acessar o diretorio
                //realizar a atualização
                if(forn != null) AlteraDiretorio(forn,fornecedor.Nome);

                con.Open();
                string alterar = $"UPDATE Fornecedores SET nome = '{fornecedor.Nome}', cnpj = '{fornecedor.Cnpj}'," +
                    $" contato = '{fornecedor.Contato}', email = '{fornecedor.Email}', anotacao = '{fornecedor.Anotacao}'," +
                    $"vencimento_fatura = '{fornecedor.VencimentoFatura.ToString("yyyy/MM/dd")}' where id = {fornecedor.Id}";
                SqlCommand mySql = new SqlCommand(alterar, con);

                if (mySql.ExecuteNonQuery() != null) return true;
                return false;
            }
            catch { return false; }
            finally { con.Close(); }
        }

        private void AlteraDiretorio(Fornecedor forn, string nome)
        {
            // $"C:\\Users\\pedro.godinho\\Documents\\Pedro\\{DateTime.Now.Year}\\{forn.nome}
            string diretorio = Path.Combine("Z:","Fornecedores",DateTime.Now.Year.ToString(),forn.nome) ;
            Console.WriteLine(diretorio);
            if(Directory.Exists(diretorio))
            {
                //$"C:\\Users\\pedro.godinho\\Documents\\Pedro\\{DateTime.Now.Year}\\{nome}";
                string novoDiretorio = Path.Combine("Z:","Fornecedores",DateTime.Now.Year.ToString(),nome);
                Directory.Move(diretorio,novoDiretorio);
            }
        }

        private Fornecedor SelecionaFornecedor(int id)
        {
            SqlConnection con = new(SQL.SConexao());
            //selecionar o objeto antigo do banco de dados
            string select = $"SELECT * FROM Fornecedores where id = {id}";
            try
            {
                con.Open();
                SqlCommand mySelect = new SqlCommand(select, con);

                SqlDataReader leitor = mySelect.ExecuteReader();

                while (leitor.Read())
                {
                    int Id = int.Parse(leitor["id"].ToString());
                    string nome = leitor["nome"].ToString();
                    string cnpj = leitor["cnpj"].ToString();
                    string contato = leitor["contato"].ToString();
                    string email = leitor["email"].ToString();
                    string anotacao = leitor["anotacao"].ToString();
                    string grupoTrab = leitor["grupo_trabalho"].ToString();

                    //o ParseExact gera um objeto datetime modelave, sigando o contexto da aplicação e algumas diretrizes
                    DateOnly vencimento = DateOnly.ParseExact(leitor["vencimento_fatura"].ToString(), "dd/MM/yyyy 00:00:00", null);

                    return new(Id, nome, cnpj, contato, email, anotacao, grupoTrab, vencimento);
                }
            }
            catch (Exception ex) { return null; }
            finally { con.Close(); }
            throw new NotImplementedException();
        }

        public bool AnaliseVencFatura(List<Fornecedor> fornecedores, Usuario us)
        {
            DateOnly hoje = DateOnly.FromDateTime(DateTime.Now);
            foreach (var fornecedor in fornecedores)
            {
                // verificar se a data estiver ha uma semana de vencer
                if (fornecedor.VencimentoFatura.Month == hoje.Month)
                {
                    if (fornecedor.VencimentoFatura.Day - hoje.Day <= 7)
                    {
                        string assunto = $"Fatura vencimento {fornecedor.VencimentoFatura}";
                        string mensagem = $"A fatura do fornecedor {fornecedor.Nome} já venceu ou vencerá no dia {fornecedor.VencimentoFatura}," +
                                           $" preste atenção. A próxima data de pagamento referente ao fornecedor será atribuida ao sistema.";

                        if (CriaEnviaEmail($"{us.NomeUsuario}@bsstex.com.br", assunto, mensagem, null))
                        {
                            //atualizar no banco de dados
                            AtualizaVencimentoFatura(fornecedor);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static bool CriaEnviaEmail(string destinatario, string assunto, string mensagem, List<string> arqs)
        {
            // Set your SMTP server and credentials
            //o email do remetente é fixo
            SmtpClient smtpClient = new("email-ssl.com.br")
            {
                Port = 587, // Porta SMTP para ssl
                Credentials = new NetworkCredential("pedro.godinho@bsstex.com.br", "Bsspgodinho1#e"),
                EnableSsl = true, // Necessário para locaweb SSL/TLS
            };

            //fazemos a verificação caso haja uma lista de arquivos a serem enviados
            MailMessage mailMessage = new MailMessage();

            if (arqs!= null)
            {
                // Create and configure the email message
                // o destinatario é um usuario externo
                mailMessage = new MailMessage($"pedro.godinho@bsstex.com.br",destinatario, assunto, mensagem)
                {
                    IsBodyHtml = false // Set to true if the body contains HTML content
                };

                foreach (string arq in arqs)
                {
                    Attachment anexo = new Attachment(arq, MediaTypeNames.Text.Plain);
                    mailMessage.Attachments.Add(anexo);
                }
            }
            else
            {
                mailMessage = new MailMessage($"pedro.godinho@bsstex.com.br",destinatario, assunto, mensagem)
                {
                    IsBodyHtml = false // Set to true if the body contains HTML content
                };
            }
            try
            {
                //Envia a mensagem baseado nos dados do objeto Email 
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
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
                string inserir = $"UPDATE nomesFornecedores SET vencimento_fatura = DATEADD(day, 30, vencimento_fatura) where id = {forn.Id};";
                SqlCommand sqlCommand = new SqlCommand(inserir, con);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex) { }
            finally { con.Close(); }
        }        

        internal List<string> EnviarNota(string destinatario, string mensagem)
        {
            Acao acao = new Acao();

            List<string> nomesFornecedores;
            List<Fornecedor> fornecedeores = listarFornecedores();
            nomesFornecedores = acao.VerificaAcaoEnvio(fornecedeores);

            if(nomesFornecedores.Count <= 0 )return nomesFornecedores;

            //verificação no diretorio , se existem os arquivos de faturas para cada fornecedor
            //gerar metodo que confere os fornecedores que tieveram suas notas enviadas
            try
            {
                List<string> notas = SelecionaNotas(nomesFornecedores);
                if ( notas.Count > 0)
                {
                    nomesFornecedores = SeparaFornecedores(notas,nomesFornecedores);
                    //modelagem e envio de email
                    if (CriaEnviaEmail(destinatario, "Notas dos Fornecedores", mensagem, notas))
                    {
                        return nomesFornecedores;
                    }
                }
                else 
                {
                    //como as notas não foram enviadas limpamos a lista
                    nomesFornecedores.Clear();
                    return nomesFornecedores;
                }
                //como as notas não foram enviadas limpamos a lista
                nomesFornecedores.Clear();
                return nomesFornecedores;

            }
            catch(Exception ex) { return nomesFornecedores; }
        }

        private List<string> SeparaFornecedores(List<string> notas, List<string> nomesFornecedores)
        {
            List<string> strings = new List<string>();

            foreach(string nota in notas)
            {
                foreach (string nomeF in nomesFornecedores)
                {
                    if (nota.Contains(nomeF) && !strings.Contains(nomeF)) strings.Add(nomeF); 
                }
            }
            return strings;
        }

        // nesse metodo verificamos se ja enviamos as notas de cada fornecedor para dai sim verificar
        // o diretorio e enviar as notas
        private List<string> SelecionaNotas(List<string> fornecedores)
        {
            //lista todas as notas de todos os fornecedores
            List<string>? caminhosNotas = new List<string> ();
            DateTime hoje = DateTime.Now;
            try
            {
                foreach (string forn in fornecedores)
                {
                    string diretorio = verificaCriaDiretorio(forn,hoje);

                    if (diretorio != "")
                    {
                        string[] arquivos = Directory.GetFiles(diretorio);
                        caminhosNotas.AddRange(arquivos);  
                    }       
                }
            }
            catch (Exception e) { }

            return caminhosNotas;
        }


        public bool CriaPastas(List<Fornecedor> fornecedores)
        {
            DateTime hoje = DateTime.Now;
            foreach(Fornecedor fornecedor in fornecedores)
            {
                if(verificaCriaDiretorio(fornecedor.nome, hoje) == "")
                {
                    return false;
                }
            }
            return true;
        }

        //verifica e modela um diretorio padrao
        private string verificaCriaDiretorio(String nomeFornecedor, DateTime hoje)
        {
            // modelo padrao diretorio "ano/fornecedor/faturas/X_MES               
            string diretorioAno = $"Z:\\Fornecedores\\{hoje.Year}";

            // Lista para armazenar os nomes das pastas
            string[] pastas = {"01_JAN", "02_FEV", "03_MAR", "04_ABR", "05_MAI", "06_JUN", "07_JUL", "08_AGO", "09_SET", "10_OUT", "11_NOV", "12_DEZ" };
            string diretorio = "";
            try
            {
                // Função para criar diretório recursivamente
                void CriarDiretorioRecursivo(string caminho)
                {
                    if (!Directory.Exists(caminho))
                    {
                        Directory.CreateDirectory(caminho);
                    }
                }

                CriarDiretorioRecursivo(diretorioAno);

                string dirFornecedor = Path.Combine(diretorioAno, nomeFornecedor);
                CriarDiretorioRecursivo(dirFornecedor);

                string dirFaturas = Path.Combine(dirFornecedor, "Faturas");
                CriarDiretorioRecursivo(dirFaturas);

                // confere e cria todas as pastas dos meses do ano 
                string dirMes;
                foreach (string pasta in pastas)
                {
                    dirMes = Path.Combine(dirFaturas, pasta);
                    CriarDiretorioRecursivo(dirMes);
                }
                // Prepara odiretorio completo
                diretorio = Path.Combine(dirFaturas, pastas[hoje.Month - 1]);

                return diretorio;
                              
            }
            catch (Exception ex) { }

            return diretorio;
        }
    }
}
