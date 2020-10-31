using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuocRestaurant.API.Model
{
    public class FinalizeSupplyRequest
    {
        public int UserId { get; set; }
        public int SupplyRequestId { get; set; }
        public string Reason { get; set; }
        public Enums.SupplyRequestState SupplyRequestState { get; set; }
    }
}
