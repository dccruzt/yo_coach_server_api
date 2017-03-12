using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoCoachServer.Models.Enums;

namespace YoCoachServer.Models.Repositories
{
    public class InvoiceRepository
    {
        public static Invoice createInvoiceForGym(double unitExpent)
        {
            try
            {
                var invoice = new Invoice()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now.ToString(),
                    UpdateAt = DateTime.Now.ToString(),
                    UnitExpent = unitExpent
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