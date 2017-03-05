using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoCoachServer.Models.BindingModels;
using YoCoachServer.Models.Enums;

namespace YoCoachServer.Models.Repositories
{
    public class ClientRepository
    {
        public static ValuesBindingModel FetchClientValues(string coachId, GetValuesBindingModel model)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var schedules = context.Schedule.Where(x => x.Clients.FirstOrDefault().ClientId.Equals(model.ClientId) && x.Coach.CoachId.Equals(coachId)).ToList();
                    double? notConfirmedAmount = 0;
                    double? confirmedAmount = 0;
                    double? creditsPreQuantity = 0;
                    double? creditsPostQuantity = 0;
                    if (schedules != null)
                    {
                        foreach (Schedule schedule in schedules)
                        {
                            if (schedule.IsConfirmed.HasValue && schedule.IsConfirmed.Value)
                            {
                                confirmedAmount += schedule.TotalValue;
                            }
                            else
                            {
                                notConfirmedAmount += schedule.TotalValue;
                            }
                            if(schedule.Gym != null)
                            {
                                if(schedule.Gym.CreditType.Equals(CreditType.PREPAGO))
                                {
                                    creditsPreQuantity += schedule.CreditsQuantity;
                                }
                                else if (schedule.Gym.CreditType.Equals(CreditType.POSTPAGO))
                                {
                                    creditsPostQuantity += schedule.CreditsQuantity;
                                }
                            }
                        }
                    }
                    var clientCoach = context.ClientCoach.FirstOrDefault(x => x.ClientId.Equals(model.ClientId) && x.CoachId.Equals(coachId));
                    ValuesBindingModel valueModel = new ValuesBindingModel()
                    {
                        ClientType = clientCoach.ClientType,
                        CreditsPreQuantity = creditsPreQuantity,
                        CreditsPostQuantity = creditsPostQuantity,
                        ConfirmedAmount = confirmedAmount,
                        NotConfirmedAmount = notConfirmedAmount,
                        TotalAmount = confirmedAmount + notConfirmedAmount
                    };
                    return valueModel;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}