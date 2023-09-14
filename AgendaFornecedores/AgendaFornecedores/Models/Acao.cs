namespace AgendaFornecedores.Models
{
    public class Acao
    {
        string nomeUsuario;
        string data;
        string fornecedor;

        public Acao( string nomeUsuario, string acao, string data, string fornecedor)
        {
            this.nomeUsuario = nomeUsuario;
            SetAcao(acao);
            this.data = data;
            this.fornecedor = fornecedor;
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
        public string Fornecedor { get => fornecedor; set => fornecedor = value; }
    }
}
