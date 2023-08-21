using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.SignalR;

namespace applicaton.Models;

public class DiffieHelmanModel
{

    private readonly BigInteger g = 2;
    private static readonly string hex = "0FFFFFFFFFFFFFFFFC90FDAA22168C234C4C6628B80DC1CD129024E088A67CC74020BBEA63B139B22514A08798E3404DDEF9519B3CD3A431B302B0A6DF25F14374FE1356D6D51C245E485B576625E7EC6F44C42E9A637ED6B0BFF5CB6F406B7EDEE386BFB5A899FA5AE9F24117C4B1FE649286651ECE45B3DC2007CB8A163BF0598DA48361C55D39A69163FA8FD24CF5F83655D23DCA3AD961C62F356208552BB9ED529077096966D670C354E4ABC9804F1746C08CA18217C32905E462E36CE3BE39E772C180E86039B2783A2EC07A28FB5C55DF06F4C52C9DE2BCBF6955817183995497CEA956AE515D2261898FA051015728E5A8AACAA68FFFFFFFFFFFFFFFF";
    public BigInteger p = BigInteger.Parse(hex, System.Globalization.NumberStyles.AllowHexSpecifier);

    /// <summary>
    /// Prime is a 2048 bit prime integer represented as <c>BigInteger</c>. 
    /// The value of Prime is proposed in section 3 at https://datatracker.ietf.org/doc/rfc3526/?include_text=1.
    /// </summary>
    public BigInteger Prime 
    { 
        get {return p;}
    }

    /// <summary>
    /// The Generator is 2 as proposed in section 3 at https://datatracker.ietf.org/doc/rfc3526/?include_text=1.
    /// </summary>
    public BigInteger Generator 
    {
        get {return g;} 
    }

    public BigInteger Random256BitNumber(){
        var rand = new Random();
        var bytes = new byte[33];
        rand.NextBytes(bytes);
        bytes[32] = 0x00; //we want unsigned 32 bit integer, no negative values
        return new BigInteger(bytes);
    }

    /// <summary>
    /// Helper function that returns one of the factors in the product that calculates the result. 
    /// For the general equation 2^15 mod p, the first factor returned is the value of 2^8 together with 7, which represents 
    /// that 2^7 mod p is what remains in the products that equal 2^15 mod p.
    /// </summary>
    /// <param name="numberbase"> base in [numberbase^a mod p] </param>
    /// <param name="a">exponent in [numberbase^a mod p]</param>
    /// <returns>one factor and remainder of exponent</returns>
    private (BigInteger, BigInteger) ModularExpSubRoutine(BigInteger numberbase,  BigInteger a){
        BigInteger temp = numberbase;
        BigInteger tempPower = 1;
        while (tempPower < a){
            tempPower *= 2;
            if (tempPower > a){
                tempPower /= 2;
                return (temp, a - tempPower);
            }
            temp = temp * temp % p;
        }
        return (temp % p, 0);
    }

    /// <summary>
    /// Calculates modular exponensiation efficiently as a product of smaller factors. Uses the mathematical fact that b * c mod d = (b mod d)*(b mod d) mod d.
    /// Consider the fact that:  2^8 * 2^8 mod d = (2^8 mod d)*(2^8 mod d) mod d = 2^16 mod d. This provides an efficient basis 
    /// for calculation with very large exponents <c>a</c>
    /// </summary>
    /// <param name="numberbase"> base in [numberbase^a mod p] </param>
    /// <param name="a">exponent in [numberbase^a mod p]</param>
    /// <returns>the value of [numberbase^a mod p] as a <c>BigInteger</c>, if a<1 returns 0 </returns>
    public BigInteger ModularExponentiation(BigInteger numberbase, BigInteger a){
        if (a < 1){
            return new (0);
        }
        var result = ModularExpSubRoutine(numberbase ,a);
        BigInteger product = result.Item1;
        while (result.Item2 != 0){
            result = ModularExpSubRoutine(numberbase, result.Item2);
            product = product * result.Item1 % p;
        }
        return product;
    }
}