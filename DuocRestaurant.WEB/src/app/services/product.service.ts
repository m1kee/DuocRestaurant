import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { Product } from '@domain/product';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private env = environment;
  private controllerName: string = 'Product';
  private controllerUrl: string = `${this.env.apiUrl}/${this.controllerName}`;
  constructor(private httpClient: HttpClient) { }

  getAll() {
    return this.httpClient.get(`${this.controllerUrl}/GetAll`);
  }

  getById(id: number) {
    return this.httpClient.get(`${this.controllerUrl}/GetById/${id}`);
  }

  post(product: Product) {
    return this.httpClient.post(`${this.controllerUrl}`, product);
  }

  put(id: number, product: Product) {
    return this.httpClient.put(`${this.controllerUrl}/${id}`, product);
  }

  delete(id: number) {
    return this.httpClient.delete(`${this.controllerUrl}/${id}`);
  }

  filterBy(filters) {
    return this.httpClient.post(`${this.controllerUrl}/FilterBy`, filters);
  }
}
