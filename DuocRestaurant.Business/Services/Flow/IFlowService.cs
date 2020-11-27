using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface IFlowService
    {
        CreateEmailResponse CreateEmailPayment(string email, int amount, string description, int paymentId);
        PaymentStatusResponse GetStatus(int commerceId);
    }
}
