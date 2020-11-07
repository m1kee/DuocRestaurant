using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface IPurchaseService
    {
        IList<Purchase> Get();
        Purchase Get(int purchaseId);
        Purchase Add(Purchase purchase);
        Purchase Edit(int purchaseId, Purchase purchase);
        bool Delete(int purchaseId);
    }
}
