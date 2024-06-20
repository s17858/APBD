using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Zadanie4.Data;
using Zadanie4.Models;

namespace Zadanie4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehouseController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public WarehouseController(WarehouseContext context)
        {
            _context = context;
        }

        [HttpPost("AddProductToWarehouse")]
        public async Task<IActionResult> AddProductToWarehouse([FromBody] ProductWarehouseRequest request)
        {
            if (request == null || request.Amount <= 0 || request.IdProduct <= 0 || request.IdWarehouse <= 0)
            {
                return BadRequest("Invalid request parameters");
            }

            //if product exists
            var product = await _context.Products.FindAsync(request.IdProduct);
            if (product == null)
            {
                return NotFound("Product not found");
            }

            //if warehouse exists
            var warehouse = await _context.Warehouses.FindAsync(request.IdWarehouse);
            if (warehouse == null)
            {
                return NotFound("Warehouse not found");
            }

            //if order match the product and amount
            var order = await _context.Orders
                .Where(o => o.IdProduct == request.IdProduct && o.Amount == request.Amount && o.CreatedAt < request.CreatedAt)
                .FirstOrDefaultAsync();
            if (order == null)
            {
                return BadRequest("No matching order found");
            }

            //if order has been fulfilled
            var existingProductWarehouse = await _context.ProductWarehouses
                .Where(pw => pw.IdOrder == order.IdOrder)
                .FirstOrDefaultAsync();
            if (existingProductWarehouse != null)
            {
                return BadRequest("Order has already been fulfilled");
            }

            //Update the order's FulfilledAt date
            order.FulfilledAt = DateTime.UtcNow;
            _context.Orders.Update(order);

            //Insert ProductWarehouse
            var productWarehouse = new ProductWarehouse
            {
                IdProduct = request.IdProduct,
                IdWarehouse = request.IdWarehouse,
                IdOrder = order.IdOrder,
                Amount = request.Amount,
                Price = product.Price * request.Amount,
                CreatedAt = DateTime.UtcNow
            };
            _context.ProductWarehouses.Add(productWarehouse);

            //Save
            await _context.SaveChangesAsync();

            return Ok(productWarehouse.IdProductWarehouse);
        }

        [HttpPost("AddProductToWarehouseUsingSP")]
        public async Task<IActionResult> AddProductToWarehouseUsingSP([FromBody] ProductWarehouseRequest request)
        {
            if (request == null || request.Amount <= 0 || request.IdProduct <= 0 || request.IdWarehouse <= 0)
            {
                return BadRequest("Invalid request parameters");
            }

            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "AddProductToWarehouse";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@IdProduct", request.IdProduct));
                    command.Parameters.Add(new SqlParameter("@IdWarehouse", request.IdWarehouse));
                    command.Parameters.Add(new SqlParameter("@Amount", request.Amount));
                    command.Parameters.Add(new SqlParameter("@CreatedAt", request.CreatedAt));

                    _context.Database.OpenConnection();
                    await command.ExecuteNonQueryAsync();

                    return Ok("Product added to warehouse using stored procedure");
                }
            }
            catch (SqlException ex)
            {
                // Handle specific SQL errors if necessary
                switch (ex.Number)
                {
                    case 547: // naruszenie klucza obcego
                        return BadRequest("Foreign Key violation");
                    case 2601: // naruszenie klucza głównego
                    case 2627: // naruszenie unikalnego ograniczenia
                        return Conflict("Data conflict");
                    default:
                        return StatusCode(500, $"SQL error occurred: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                // Handle other errors
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            finally
            {
                _context.Database.CloseConnection();
            }
        }
    }
}