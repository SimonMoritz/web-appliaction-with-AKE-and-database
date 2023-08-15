namespace applicaton.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public int Summ(int a, int b){
        return a + b;
    }
}
