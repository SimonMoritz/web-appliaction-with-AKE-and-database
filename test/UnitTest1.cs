namespace test;
using applicaton.Models;
using Xunit;
public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        ErrorViewModel e = new();
        Assert.Equal(5, e.Summ(2,3));
    }    
}