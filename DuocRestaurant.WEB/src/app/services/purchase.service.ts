import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { HttpClient } from '@angular/common/http';
import { Purchase } from '@domain/purchase';

@Injectable({
  providedIn: 'root'
})
export class PurchaseService {
  private env = environment;
  private controllerName: string = 'Purchase';
  private controllerUrl: string = `${this.env.apiUrl}/${this.controllerName}`;
  constructor(private httpClient: HttpClient) { }

  getAll() {
    return this.httpClient.get(`${this.controllerUrl}/GetAll`);
  }

  getById(id: number) {
    return this.httpClient.get(`${this.controllerUrl}/GetById/${id}`);
  }

  post(purchase: Purchase) {
    return this.httpClient.post(`${this.controllerUrl}`, purchase);
  }

  put(id: number, purchase: Purchase) {
    return this.httpClient.put(`${this.controllerUrl}/${id}`, purchase);
  }

  delete(id: number) {
    return this.httpClient.delete(`${this.controllerUrl}/${id}`);
  }

  filterBy(filters) {
    return this.httpClient.post(`${this.controllerUrl}/FilterBy`, filters);
  }

  validatePayment(id: number) {
    return this.httpClient.get(`${this.controllerUrl}/ValidatePayment/${id}`);
  }
}
