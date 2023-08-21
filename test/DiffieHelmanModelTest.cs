namespace test;

using System.Formats.Asn1;
using System.Numerics;
using applicaton.Models;
using Xunit;
public class DiffieHelmanModelTest
{
    DiffieHelmanModel dh;

    //calculation of prime from javascript and WolframAlphe(see https://shorturl.at/awzP5). 
    //Original prime is proposed in section 3 at https://datatracker.ietf.org/doc/rfc3526/?include_text=1
    string primeAsString = "32317006071311007300338913926423828248817941241140239112842009751400741706634354222619689417363569347117901737909704191754605873209195028853758986185622153212175412514901774520270235796078236248884246189477587641105928646099411723245426622522193230540919037680524235519125679715870117001058055877651038861847280257976054903569732561526167081339361799541336476559160368317896729073178384589680639671900977202194168647225871031411336429319536193471636533209717077448227988588565369208645296636077250268955505928362751121174096972998068410554359584866583291642136218231078990999448652468262416972035911852507045361090559";
    public DiffieHelmanModelTest(){
        dh = new();
    }

    [Fact]
    public void ShouldGiveCorrectGenerator(){
        BigInteger g = 2;
        Assert.Equal(g, dh.Generator);
    } 

    [Fact]
    public void ShouldGiveCorrectPrime(){
        BigInteger p = dh.Prime;
        Assert.Equal(2048,p.GetBitLength()); //2048 long prime
        Assert.Equal(3, (p*2 + 3)%p);
        Assert.Equal(primeAsString, dh.Prime.ToString());
    }   

    [Fact]
    public void ShouldDoModularExponentiationCorrect(){
        BigInteger a = 3;
        Assert.Equal(8,dh.ModularExponentiation(dh.Generator, a));
        a = 123456789;
        string res2 = "26401662759562254322800114447510604883261839010089584101656845100279879366313359796112505465025059241776952435848331065293228877699583143209230205282888635764056666869765749635016701232128194393660272116804913371800316521413283293643824001842520939655437336764622376854631973615032319172198796782408868543261134224582259242141182053606742818886914695600431841794866344017919276206578091728528869422502130379084509236126468504998538565649292620442173012650787286805492603375681975670446385710455382872736885891237425860468177358609345155085604577456131954779548834675033076181928102709003336242884897030282354050793447";
        // res2 value is the result of 2^(123456789) mod dh.prime in string representation
        Assert.Equal(res2,dh.ModularExponentiation(dh.Generator, a).ToString()); 
    }

    [Fact]
    public void ShouldDoModularEsxponentiationCorrect(){
        BigInteger a = -1;
        Assert.Equal(0,dh.ModularExponentiation(dh.Generator, a));
    }

    [Fact]
    public void ShouldFindCommonKey(){
        BigInteger a = 99932838849;
        BigInteger b = 1000000000000011235;
        BigInteger aliceSends = dh.ModularExponentiation(dh.Generator, a);
        BigInteger bobSends = dh.ModularExponentiation(dh.Generator, b);
        BigInteger aliceKey = dh.ModularExponentiation(bobSends, a);
        BigInteger bobKey = dh.ModularExponentiation(aliceSends, b);
        Assert.Equal(aliceKey, bobKey); //they should have the same encryption key now
    }

    [Fact]
    public void ShouldGiveRandomBytes(){
        for (int i = 0; i < 10; i++){
            Assert.True(dh.Random256BitNumber().GetBitLength() <= 256);
            Assert.True(dh.Random256BitNumber() > -1);
        }
    }
}