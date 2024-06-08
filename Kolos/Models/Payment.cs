namespace Kolos.Models;

public class Payment
{
    public int IdPayment { get; set; }
    public DateTime Date { get; set; }
    public int IdClient { get; set; }
    public Client Client { get; set; }
    public int IdSubscription { get; set; }
    public Subscription Subscription { get; set; }
}