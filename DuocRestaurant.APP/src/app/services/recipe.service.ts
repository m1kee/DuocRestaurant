import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { Recipe } from '@app/domain/recipe';

@Injectable({
    providedIn: 'root'
})
export class RecipeService {
    private env = environment;
    private controllerName: string = 'Recipe';
    private controllerUrl: string = `${this.env.apiUrl}/${this.controllerName}`;
    constructor(private httpClient: HttpClient) { }

    getAll() {
        return this.httpClient.get(`${this.controllerUrl}/GetAll`);
    }

    getById(id: number) {
        return this.httpClient.get(`${this.controllerUrl}/GetById/${id}`);
    }

    post(recipe: Recipe) {
        return this.httpClient.post(`${this.controllerUrl}`, recipe);
    }

    postForm(form: FormData) {
        return this.httpClient.post(`${this.controllerUrl}`, form);
    }

    put(id: number, recipe: Recipe) {
        return this.httpClient.put(`${this.controllerUrl}/${id}`, recipe);
    }

    putForm(id: number, form: FormData) {
        return this.httpClient.put(`${this.controllerUrl}/${id}`, form);
    }

    delete(id: number) {
        return this.httpClient.delete(`${this.controllerUrl}/${id}`);
    }
}
