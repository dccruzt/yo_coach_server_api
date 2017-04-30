using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoCoachServer.Models.Enums;

namespace YoCoachServer.Models.Repositories
{
    public class InvoiceRepository
    {
        public static Invoice createInvoiceForGym(double amountExpend)
        {
            try
            {
                var invoice = new Invoice()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTimeOffset.Now,
                    UpdatedAt = DateTimeOffset.Now,
                    AmountExpend = amountExpend
                };
                return invoice;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Invoice createInvoiceForClientDebit(double amountExpend)
        {
            try
            {
                var invoice = new Invoice()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTimeOffset.Now,
                    UpdatedAt = DateTimeOffset.Now,
                    AmountExpend = amountExpend,
                    PaidAt = DateTimeOffset.Now
                };
                return invoice;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}