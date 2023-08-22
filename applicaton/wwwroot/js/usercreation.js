hexP = "FFFFFFFFFFFFFFFFC90FDAA22168C234C4C6628B80DC1CD129024E088A67CC74020BBEA63B139B22514A08798E3404DDEF9519B3CD3A431B302B0A6DF25F14374FE1356D6D51C245E485B576625E7EC6F44C42E9A637ED6B0BFF5CB6F406B7EDEE386BFB5A899FA5AE9F24117C4B1FE649286651ECE45B3DC2007CB8A163BF0598DA48361C55D39A69163FA8FD24CF5F83655D23DCA3AD961C62F356208552BB9ED529077096966D670C354E4ABC9804F1746C08CA18217C32905E462E36CE3BE39E772C180E86039B2783A2EC07A28FB5C55DF06F4C52C9DE2BCBF6955817183995497CEA956AE515D2261898FA051015728E5A8AACAA68FFFFFFFFFFFFFFFF";
p = BigInt("0x" + hexP);
g = BigInt(2);
const a = createRandomNumber();
var valClientKey = document.getElementsByClassName("clientKeyCalc")[0];
valClientKey.value = modolusExponentiation(g, a, p).toString();

const x = document.getElementsByClassName("toBeEncrypted");
for (var i = 0; i<x.length; i++){
  x[i].value = x[i].value + " change224"
}

var encrypter = document.getElementsByClassName("encryptAction")[0];
encrypter.addEventListener('click', function(){
    var clientKey = document.getElementsByClassName("clientKey")[0];
    var valClientKey = document.getElementsByClassName("clientKeyCalc")[0];
    clientKey.value = valClientKey.value;
})

var calculateCommonKey = document.getElementsByClassName("powerBya")[0];
calculateCommonKey.addEventListener('click', function(){
  var serverKey = BigInt(document.getElementsByClassName("serverKey")[0].value);
  var commonKey = modolusExponentiation(serverKey, a, p);
  var commonKeyElement = document.getElementsByClassName("commonKey")[0];
  console.log(commonKey)
  commonKeyElement.value = commonKey.toString();
});

function createRandomNumber(){
  const array = new BigUint64Array(4);
  self.crypto.getRandomValues(array);
  var fullNumberString = "";
  for (const num of array) {
    var numString = num.toString(2);
    var loops = 64-numString.length
    for (var j=0; j<loops; j++){
      numString = "0" + numString;
    }
    fullNumberString = fullNumberString + numString 
  }
  var randomNumber = BigInt(0);
  for (var i=0n; i<256n; i++){
    randomNumber = randomNumber + BigInt(fullNumberString[i])*(2n**i)
  }
  return randomNumber
}

function modolusExponentiation(base,a,p){
  function subroutine(base,a,p){
      temp = base;
      tempPower = 1n;
      while(tempPower <= a){
          tempPower = tempPower * 2n;
          if (tempPower > a){
              tempPower = tempPower / 2n;
              return [temp, (a - tempPower)];
          }
          temp = (temp * temp) % p;
      }
      return [temp % p, 0n];
  }
  res = subroutine(base,a,p);
  result = res[0];
  while (res[1] != 0n){
      res = subroutine(base,res[1], p);
      result = (result * res[0]) % p;
  }
  return result % p;
}