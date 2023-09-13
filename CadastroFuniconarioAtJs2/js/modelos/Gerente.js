
import Funcionario from "./Funcionario"

export default class Gerente extends Funcionario{
    constructor(nome, idade, cargo, departamento){
        super(nome, idade,cargo)
        this.departamento = departamento
    }

    Gerenciar(){
        return "Quem sabe faz. quem não sabe gerencia"
    }
}
