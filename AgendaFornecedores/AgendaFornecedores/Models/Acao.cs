namespace AgendaFornecedores.Models
{
    public class Acao
    {
        string nomeUsuario;
        string data;
        string nomeFornecedor;
        static string conexao = "Server=localhost;Port=3306;Database=agenda_fornecedores;User Id=root;Password=Ad#2735G";

        public Acao( string nomeUsuario, string acao, string data, string nomeFornecedor)
        {
            this.nomeUsuario = nomeUsuario;
            SetAcao(acao);
            this.data = data;
            this.nomeFornecedor = nomeFornecedor;
        }

        public string NomeUsuario { get => nomeUsuario; set => nomeUsuario = value; }

        private string acao;

        public string GetAcao()
        {
            return acao;
        }

        public void SetAcao(string value)
        {
            acao = value;
        }

        public string Data { get => data; set => data = value; }
        public string NomeFornecedor { get => nomeFornecedor; set => nomeFornecedor = value; }
    }
}
