// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// funcoes para modelar mascara de cnpj

function ConfirmacaoCadastro() {
    var cnpjInput = document.getElementById('CNPJ');
    let contatoinput = document.getElementById('contato');
    //cnpjInput.value = formatarCNPJ(cnpjInput.value);
    alert("macaco loko" + cnpjInput.value + contatoinput.value)

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
