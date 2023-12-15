// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// funcoes para modelar mascara de cnpj

function ConfirmacaoCadastro() {

    nomeU = document.getElementsByName("nome")[0]
    cnpjU = document.getElementsByName("cnpj")[0]
    contatoU = document.getElementsByName("contato")[0]
    emailU = document.getElementsByName("email")[0]

    //tratando cnpj
    if (/[a-zA-Z]/.test(cnpjU.value) || cnpjU.value.length < 14) {
        cnpjU.classList.add("bordavermelha")
        alert("O" + cnpjU.value + " CNPJ não atente aos requisistos do cadastro")
        return false
    }

    //tratando contato
    if (/[a-zA-Z]/.test(contatoU.value) || contatoU.value.length < 9) {
        contatoU.classList.add("bordavermelha")
        alert("O contato não atende aos requisitos do  cadastro")
        return false
    }

    return VerificaEmail(emailU.value)



    return confirm("Os dados estao corretos? Deseja mesmo fazer o cadastro? ")
}

function Deletar() {
    return confirm("Tem certeza de que deseja deletar esse fornecedor da lista?")
}

function adicionarGrupo() {
    return confirm("Tem certeza de qua deseja adicionar esse grupo aos administradores?")
}

function validarAlterar() {
    //let input = document.getElementsByName("nomeFornecedor")
    //let nF = input[0].value
    // nF += "adfasadf"
    return confirm("Deseja realizar essas alterações no fornecedor ??")
}

function VerificaDestinatario() {
    email = document.getElementsByName("destinatario")[0]
    return VerificaEmail(email.value)
}



function VerificaEmail(email) {

    if (email.includes("@") && email.includes(".com") || email.includes(".com.br")) {
        return true;
    }
    else {
        alert("O email não segue os padrões para o envio")
        return false;
    }    
}
