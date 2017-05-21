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

        public static List<Invoice> ListInvoices(String scheduleId)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var invoices = context.Invoice.ToList();
                    if (scheduleId != null)
                    {
                        var invoicesBySchedule = context.Schedule.Where(x => x.Id.Equals(scheduleId)).SelectMany(x => x.Gym.Credit.Invoices).ToList();
                        invoices = invoicesBySchedule;
                    }
                    return invoices;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}