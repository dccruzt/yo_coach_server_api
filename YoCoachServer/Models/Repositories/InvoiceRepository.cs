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
                    UpdateAt = DateTimeOffset.Now,
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
                    UpdateAt = DateTimeOffset.Now,
                    AmountExpend = amountExpend,
                    PaidAt = DateTime.Now.ToString()
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