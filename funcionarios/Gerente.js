class Gerente extends Funcionario{
    constructor(nome, idade, cargo, departamento ){
        super(nome, idade, cargo);
        this.departamento = departamento;

    }
    
     Gerenciar(){
        alert("Estou gerenciando")
     }

}

ger = new Gerente()
ger.Gerenciar()