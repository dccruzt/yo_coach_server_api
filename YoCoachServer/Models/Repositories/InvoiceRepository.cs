using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoCoachServer.Models.Enums;

namespace YoCoachServer.Models.Repositories
{
    public class InvoiceRepository
    {
        public static Invoice createInvoiceForGym(double? creditsAmount)
        {
            try
            {
                var invoice = new Invoice()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTimeOffset.Now,
                    UpdatedAt = DateTimeOffset.Now,
                    CreditsAmount = creditsAmount
                };
                return invoice;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Invoice createInvoiceForStudentPayment(double creditsAmount)
        {
            try
            {
                var invoice = new Invoice()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTimeOffset.Now,
                    UpdatedAt = DateTimeOffset.Now,
                    CreditsAmount = creditsAmount,
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