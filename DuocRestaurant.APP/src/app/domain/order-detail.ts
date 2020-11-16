import { Product } from './product';
import { Recipe } from './recipe';

export class OrderDetail {
    OrderId: number;
    ProductId?: number;
    RecipeId?: number;
    Count: number;
    Price: number;

    Product?: Product;
    Recipe?: Recipe;

    public constructor() {}
}
