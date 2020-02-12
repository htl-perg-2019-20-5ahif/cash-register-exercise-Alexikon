using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static CashRegister.Shared.Model;

namespace CashRegister.WebApi.Controllers
{
    [ApiController]
    [Route("api/receipts")]
    public class ReceiptsController : ControllerBase
    {
        private readonly CashRegisterDataContext DataContext;

        public ReceiptsController(CashRegisterDataContext dataContext)
        {
            DataContext = dataContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<ReceiptLineDto> receiptLineDto)
        {
            // implementing check 
            if (receiptLineDto == null || receiptLineDto.Count == 0)
            {
                return BadRequest();
            }

            // Here you have to add code that reads all products referenced by product IDs
            // in receiptDto.Lines and store them in the `products` dictionary.
            var products = new Dictionary<int, Product>();
            foreach (var rl in receiptLineDto)
            {
                products[rl.ProductID] = await DataContext.Products.FirstOrDefaultAsync(p => p.ID == rl.ProductID);
                if (products[rl.ProductID] == null)
                {
                    return BadRequest();
                }
            }

            // Build receipt from DTO
            var newReceipt = new Receipt
            {
                ReceiptTimestamp = DateTime.UtcNow,
                ReceiptLines = receiptLineDto.Select(rl => new ReceiptLine
                {
                    // I don't want to question your code from readme.md, but why does ID have to be set? 
                    // I thought it is automatically set at 0 when nobody does something
                    ID = 0,
                    Product = products[rl.ProductID],
                    Amount = rl.Amount,
                    TotalPrice = rl.Amount * products[rl.ProductID].UnitPrice
                }).ToList()
            };
            newReceipt.TotalPrice = newReceipt.ReceiptLines.Sum(rl => rl.TotalPrice);

            await DataContext.Receipts.AddAsync(newReceipt);
            await DataContext.SaveChangesAsync();

            return CreatedAtRoute("api/Receipts/POST", new { id = newReceipt.ID }, newReceipt);
        }
    }
}
