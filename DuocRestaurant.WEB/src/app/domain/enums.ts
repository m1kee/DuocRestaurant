export enum Profiles {
  Administrator = 1,
  Warehouse = 2,
  Finance = 3,
  Kitchen = 4,
  Waiter = 5,
  Customer = 6,
  Reception = 7,
  Table = 8
}

export enum ProductTypes {
  Supply = 1,
  Consumable = 2
}

export enum SupplyRequestStates {
  NotAssigned = 0,
  Created = 1,
  Sended = 2,
  Confirmed = 3,
  Rejected = 4
}

export enum BookingState {
  Active = 1,
  Canceled = 2,
  Expired = 3
}

export enum PurchaseState {
  NotAssigned = 0,
  PendingPayment = 1,
  Paid = 2,
  Rejected = 3,
  Canceled = 4
}

export enum OrderState {
  NotAssigned = 0,
  Canceled = 1,
  Pending = 2,
  InPreparation = 3,
  Ready = 4,
  Delivered = 5
}
