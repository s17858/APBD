namespace Kolos.Controllers;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Kolos.Data;
using Kolos.Models;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class SubscriptionController : ControllerBase
{
    private readonly AppContext _context;

    public SubscriptionController(AppContext context)
    {
        _context = context;
    }

    [HttpGet("{idClient}")]
    public async Task<IActionResult> GetClientSubscriptions(int idClient)
    {
        var client = await _context.Clients
            .Include(c => c.Payments)
            .ThenInclude(p => p.Subscription)
            .FirstOrDefaultAsync(c => c.IdClient == idClient);

        if (client == null)
        {
            return NotFound();
        }

        var result = new
        {
            client.FirstName,
            client.LastName,
            client.Email,
            client.Phone,
            subscriptions = client.Payments
                .GroupBy(p => new { p.Subscription.IdSubscription, p.Subscription.Name })
                .Select(g => new
                {
                    IdSubscription = g.Key.IdSubscription,
                    Name = g.Key.Name,
                    TotalPaidAmount = g.Sum(p => p.Subscription.Price)
                })
        };

        return Ok(result);
    }

    [HttpPost("payment")]
    public async Task<IActionResult> CreatePayment([FromBody] Payment payment)
    {
        if (payment == null)
        {
            return BadRequest("Payment is null");
        }

        var client = await _context.Clients.FindAsync(payment.IdClient);
        if (client == null)
        {
            return NotFound("Client not found");
        }

        var subscription = await _context.Subscriptions.FindAsync(payment.IdSubscription);
        if (subscription == null)
        {
            return NotFound("Sub not found");
        }

        var sale = await _context.Sales
            .Where(s => s.IdClient == payment.IdClient && s.IdSubscription == payment.IdSubscription)
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefaultAsync();

        if (sale == null)
        {
            return NotFound("Sale not found for this client and sub");
        }

        var startPeriod = sale.CreatedAt;
        var renewalPeriod = subscription.RenewalPeriod;
        var nextPaymentDueDate = startPeriod.AddMonths(renewalPeriod);

        if (payment.Date < startPeriod || payment.Date > nextPaymentDueDate)
        {
            return BadRequest($"Payment date should be between {startPeriod.ToShortDateString()} and {nextPaymentDueDate.ToShortDateString()}");
        }

        if (ModelState.IsValid)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClientSubscriptions), new { idClient = payment.IdClient }, payment);
        }

        return BadRequest(ModelState);
    }
    
}
