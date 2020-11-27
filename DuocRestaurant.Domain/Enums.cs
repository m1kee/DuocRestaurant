using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Enums
    {
        public enum ProductType
        {
            NotAssigned = 0,
            Supply = 1,
            Consumable = 2
        }

        public enum Profile
        {
            NotAssigned = 0,
            Administrator = 1,
            Warehouse = 2,
            Finance = 3,
            Kitchen = 4,
            Waiter = 5,
            Customer = 6,
            Reception = 7,
            Table = 8
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
            NotAssigned = 0,
            Active = 1,
            Canceled = 2,
            Expired = 3
        }

        public enum PurchaseState
        {
            NotAssigned = 0,
            PendingPayment = 1,
            Paid = 2,
            Rejected = 3,
            Canceled = 4
        }

        public enum OrderState
        {
            NotAssigned = 0,
            Canceled = 1,
            Pending = 2,
            InPreparation = 3,
            Ready = 4,
            Delivered = 5
        }
    }
}
