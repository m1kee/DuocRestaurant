import { Product } from './product';

export class RecipeDetail {
    RecipeId: number;
    ProductId: number;
    Count: number;
    Active: boolean;

    Product: Product;

    public constructor() {
        this.Active = true;
        this.Count = 0;
        this.ProductId = null; 
    }
}