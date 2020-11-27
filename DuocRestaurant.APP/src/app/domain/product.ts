import { ProductType } from './product-type';
import { MeasurementUnit } from './measurement-unit';
import { Provider } from './provider';

export class Product {
    Id: number;
    Name: string;
    Details: string;
    ProductTypeId: number;
    Count: number;
    MeasurementUnitId: number;
    CostPrice: number;
    SalePrice?: number;
    ProviderId: number;
    Active: boolean;

    ProductType: ProductType;
    MeasurementUnit: MeasurementUnit;
    Provider: Provider;

    public constructor() {
        this.Active = true;
        this.CostPrice = 0;
        this.SalePrice = null;
        this.ProductTypeId = null;
        this.MeasurementUnitId = null;
        this.ProviderId = null;
    }
}
