
const lista1 = document.getElementById("lista1")
const times = ["fluminense", "palmeiras", "vasco", "15dePiracicaba"]


function mostraTimes(){
    const lista1 = document.getElementById("lista1")
    lista1.style.backgroundColor = "blue"
    
    
    for ( i = 0; i< times.length; i++){
        let lItem = document.createElement("li")
        lItem.innerText = times[i]
        lItem.style.color = "white"
        lista1.append(lItem)
    
    }
    

}


function apagarTimes(){
    alert("Macaco")

    for ( i = 0; i< times.length; i++){
        lista1[i].innerText = "Macaco"
        lista1.append(lista1[i])

    }

    console.log(lista1[1])
    alert("Macaco"+ times[3])

}

