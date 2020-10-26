import { RecipeDetail } from './recipe-detail';

export class Recipe {
    Id: number;
    Name: string;
    Price: number;
    Details: string;
    PreparationTime: number;
    ImageBase64: string;
    Image: any;
    Active: boolean;

    RecipeDetails: RecipeDetail[];

    public constructor() {
        this.Active = true;
        this.Price = 0;
        this.RecipeDetails = [new RecipeDetail()];
    }
}