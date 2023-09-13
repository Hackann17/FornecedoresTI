import Funcionario from "./Funcionario"

export default class Desenvolvedor extends Funcionario{
    constructor(nome, idade, cargo, linguagem){
        super(nome, idade, cargo)
        this.linguagem = linguagem
    }

    Programar(){
        return "Programo logo existo"
    }

}
