
import Funcionario from "./modelos/Funcionario.js";
import Gerente from "./modelos/Gerente.js"
import Desenvolvedor from "./modelos/Desenvolvedor.js";

function criarObject(){

    const cadastros = []
    try{
        //imput variables
        let idade = document.getElementById("idade");
        let nome = document.getElementById("nome")
        let cargo = document.getElementById("cargo")

        //veryfing conditions to create new instances
        if(cargo.value.toLowerCase() == "gerente"){
            const gerente = new Gerente(nome.value, idade.value, cargo.value, "almocharifado")
            console.log(gerente.Gerenciar() +"\n"+ gerente.Trabalhar())
            cadastros.push(gerente)
            alert("A" + cadastros)
            return gerente

        }else if(cargo.value.toLowerCase() == "desenvolvedor"){
            const desenvolvedor = new Desenvolvedor(nome.value, idade.value, cargo.value, "js")
            console.log(desenvolvedor.Programar() +"\n"+ desenvolvedor.Trabalhar())
            cadastros.push(desenvolvedor)
            alert("b" + cadastros)

            return desenvolvedor

        }else if (cargo.value.toLowerCase() == "funcionario"){
            const funcionario = new Funcionario(nome.value, idade.value, cargo.value)
            console.log(funcionario.seApresentar()  +"\n"+ funcionario.Trabalhar())
            cadastros.push(funcionario)
            alert("c" + cadastros)
            return funcionario;

        }
            throw new Error("Oops! Parece que não reconhecemos nenhumm desses cargos em...")
        
    }

    catch(erro){
        alert(erro.message)
    }
    finally{
        alert("Gere um novo cadastro")
    }

}

const botao = document.getElementById("butao")
botao.addEventListener("click", criarObject)

