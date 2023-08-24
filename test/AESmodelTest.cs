namespace test;

using System.Security.Cryptography;
using System.Numerics;
using applicaton.Models;
using Xunit;
using System.Text;

public class AESmodelTest {

    public AESModel aes;

    public AESmodelTest(){
        aes = new ();
        
    }

    [Fact]
    public void ShouldDecryptCorrectly(){
        var rand = new Random();
        var bytes = new byte[2048];
        byte[] b1, cipher;
        byte[] iv = Encoding.UTF8.GetBytes("asdfghjklzxcvbnm");
        rand.NextBytes(bytes);
        byte[] b2 = Encoding.UTF8.GetBytes("Message");
        using (SHA256 sha256 = SHA256.Create())
        {
            b1 = Encoding.UTF8.GetBytes("whatever");
            b2 = sha256.ComputeHash(b2);
        }
        Assert.Equal(32, b2.Length);
        cipher = aes.EncryptStringToBytes_Aes("simon", b1, iv);
        string decCipher = aes.DecryptStringFromBytes_Aes(cipher, b1, iv);
        Assert.Equal("simon", decCipher);
    }
}

