const x = document.getElementsByClassName("toBeEncrypted");
for (var i = 0; i<x.length; i++){
  x[i].value = x[i].value + " change224"
}
console.log("wtf")

var encrypter = document.getElementsByClassName("encryptAction")[0];
encrypter.addEventListener('click', function(){
    var clientKey = document.getElementsByClassName("clientKey")[0];
    console.log(clientKey)
    var valClientKey = document.getElementsByClassName("clientKeyCalc")[0];
    console.log(valClientKey)
    clientKey.value = valClientKey.value;
    console.log("wtf")
})
