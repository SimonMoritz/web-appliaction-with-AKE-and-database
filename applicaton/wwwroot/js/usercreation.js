
hexP = "FFFFFFFFFFFFFFFFC90FDAA22168C234C4C6628B80DC1CD129024E088A67CC74020BBEA63B139B22514A08798E3404DDEF9519B3CD3A431B302B0A6DF25F14374FE1356D6D51C245E485B576625E7EC6F44C42E9A637ED6B0BFF5CB6F406B7EDEE386BFB5A899FA5AE9F24117C4B1FE649286651ECE45B3DC2007CB8A163BF0598DA48361C55D39A69163FA8FD24CF5F83655D23DCA3AD961C62F356208552BB9ED529077096966D670C354E4ABC9804F1746C08CA18217C32905E462E36CE3BE39E772C180E86039B2783A2EC07A28FB5C55DF06F4C52C9DE2BCBF6955817183995497CEA956AE515D2261898FA051015728E5A8AACAA68FFFFFFFFFFFFFFFF";
p = BigInt("0x" + hexP);
g = BigInt(2);
const a = createRandomNumber();
var valClientKey = document.getElementsByClassName("clientKeyCalc")[0];
valClientKey.value = modolusExponentiation(g, a, p).toString();

console.log("heææp")
const x = document.getElementsByClassName("toBeEncrypted");
for (var i = 0; i<x.length; i++){
  x[i].value = x[i].value + " change224"
}

var encrypter = document.getElementsByClassName("encryptAction")[0];
encrypter.addEventListener('click', function(){
    var clientKey = document.getElementsByClassName("clientKey")[0];
    var valClientKey = document.getElementsByClassName("clientKeyCalc")[0];
    clientKey.value = valClientKey.value;
    
    var commonKeyElement = document.getElementsByClassName("commonKey")[0];
    var commonKey = BigInt(commonKeyElement.value)
    var commonKeyString = reverseString(commonKey.toString(16))
    var commonKeyWord = CryptoJS.enc.Hex.parse(commonKeyString);
    console.log("commonkey is " + commonKeyString)
    
    var passwordElement = document.getElementsByClassName("toBeEncrypted")[0];
    var commonKeyHash = CryptoJS.SHA256(commonKeyWord);
    var iv = CryptoJS.enc.Utf8.parse("simonmoritzjense");
    var encrypted = CryptoJS.AES.encrypt(passwordElement.value, commonKeyHash, {iv: iv});
    passwordElement.value = encrypted.ciphertext.toString()


    var secret = CryptoJS.enc.Utf8.parse("simon");
    var hash = CryptoJS.SHA256("whatever");
    var cipher = CryptoJS.AES.encrypt(secret, hash, {iv: iv});
    console.log("cipher is " + cipher.ciphertext.toString())
    console.log("iv is " + cipher.iv);
    console.log("salt is " + cipher.salt)
})

var calculateCommonKey = document.getElementsByClassName("powerBya")[0];
calculateCommonKey.addEventListener('click', function(){
  var serverKey = BigInt(document.getElementsByClassName("serverKey")[0].value);
  var commonKey = modolusExponentiation(serverKey, a, p);
  var commonKeyElement = document.getElementsByClassName("commonKey")[0];
  console.log(commonKey.toString())
  commonKeyElement.value = commonKey.toString();


  var key = "2e35f242a46d67eeb74aabc37d5e5d05";
  var data = CryptoJS.AES.encrypt("Message", key); // Encryption Part
  var decrypted = CryptoJS.AES.decrypt(data, key).toString(CryptoJS.enc.Utf8); // Message
  console.log(decrypted)
  var hash = CryptoJS.SHA256("Message");
  console.log("Message")
  console.log(typeof hash)
  console.log(hash.toString())
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

function reverseString(str) {
  var newString = "";
  for (var i = str.length - 1; i >= 0; i--) {
      newString += str[i];
  }
  var newSecondString = "";
  for (var i=0; i<str.length; i = i+2){
    newSecondString = newSecondString + newString[i+1] + newString[i];
  }
  return newSecondString;
}