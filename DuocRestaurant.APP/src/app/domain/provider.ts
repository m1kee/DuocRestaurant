export class Provider {
  Id: number;
  Name: string;
  Address: string;
  Phone: string;
  Email: string;
  Active: boolean;

  public constructor() {
    this.Active = true;
  }
}
