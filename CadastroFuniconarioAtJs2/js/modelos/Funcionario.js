export default class Funcionario{

    constructor(nome, idade, cargo){
        this.nome = nome
        this.idade = idade
        this.cargo = cargo
    }

    seApresentar(setor){
        if(this.cargo != "funcionario"){
            return "Ola Sou o " + this.nome + "! trabalho como" + this.cargo + " no setor " + this.setor
        }
        return "Ola Sou o !"  + this.nome + "trabalho como " + this.cargo + "opa"

    }

    Trabalhar(){
        return "Estou trabalhando!!"
    }

}
