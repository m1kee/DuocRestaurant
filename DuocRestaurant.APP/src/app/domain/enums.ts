export enum Profiles {
  Administrator = 1,
  Warehouse = 2,
  Finance = 3,
  Kitchen = 4,
  Waiter = 5,
  Customer = 6
}

export enum ProductTypes {
  Supply = 1,
  Consumable = 2
}

export enum SupplyRequestStates {
  NotAssigned = 0,
  Created = 1,
  Confirmed = 2,
  Rejected = 3
}

export enum BookingState {
  Active = 1,
  Canceled = 2,
  Expired = 3
}