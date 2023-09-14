﻿namespace AgendaFornecedores.Models
{
    public class Fornecedores
    {
        string nome;
        string cnpj;
        string contato;
        string email;
        string anotacao;

        public Fornecedores(string nome, string cnpj, string contato, string email, string anotacao) { 
            this.nome = nome;
            this.cnpj = cnpj;
            this.contato = contato;
            this.email = email;
            this.anotacao = anotacao;
        }

        public string Nome { get => nome; set => nome = value; }
        public string Cnpj { get => cnpj; set => cnpj = value; }
        public string Contato { get => contato; set => contato = value; }
        public string Email { get => email; set => email = value; }
        public string Anotacao { get => anotacao; set => anotacao = value; }
        //string de conecção como banco de dados
        //StringConnection = "Server=127.0.0.1;Port=5432;Database=MP_DOTNET6_API;User Id=postgres;Password=teste123;"


        public string Cadastrar( Fornecedores fornecedor)
        {
            return "os dados estam aki "+fornecedor.nome + fornecedor.contato + fornecedor.email + fornecedor.anotacao;    
        }
    }
}
