class Funcionario{
    constructor(nome, idade, cargo){
        this.nome = nome;
        this.idade = idade;
        this.cargo = cargo;
    }
     
    Apresentarse(){
        console.log("Olá mundo!!, sou" + this.nome+'!!!!')
    }

    Trabalhar(){
    console.log("Eu"+ this.nome + " estou trabalhando em")
    }

}