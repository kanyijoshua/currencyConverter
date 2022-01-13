namespace currencyConvertor.Models;

public class Logs
{
    public int Id { get; set; }
    public string CurrencyFrom { get; set; }
    public string CurrencyTo { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;
}