using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Enums
    {
        public enum ProductType
        {
            Supply = 1,
            Consumable = 2
        }

        public enum Profile
        {
            Administrator = 1,
            Warehouse = 2,
            Finance = 3,
            Kitchen = 4,
            Waiter = 5,
            Customer = 6
        }

        public enum SupplyRequestState
        {
            NotAssigned = 0,
            Created = 1,
            Sended = 2,
            Confirmed = 3,
            Rejected = 4
        }

        public enum BookingState
        {
            Active = 1,
            Canceled = 2,
            Expired = 3
        }
    }
}
