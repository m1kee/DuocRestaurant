import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SupplyOrder } from '@app/domain/supply-order';
import { environment } from '@env/environment';

@Injectable({
  providedIn: 'root'
})
export class SuppliesOrderService {
  private env = environment;
  private controllerName: string = 'SuppliesOrder';
  private controllerUrl: string = `${this.env.apiUrl}/${this.controllerName}`;
  constructor(private httpClient: HttpClient) { }

  getAll() {
    return this.httpClient.get(`${this.controllerUrl}/GetAll`);
  }

  getById(id: number) {
    return this.httpClient.get(`${this.controllerUrl}/GetById/${id}`);
  }

  post(supplyOrder: SupplyOrder) {
    return this.httpClient.post(`${this.controllerUrl}`, supplyOrder);
  }

  
  put(id: number, supplyOrder: SupplyOrder) {
    return this.httpClient.put(`${this.controllerUrl}/${id}`, supplyOrder);
  }

  delete(id: number) {
    return this.httpClient.delete(`${this.controllerUrl}/${id}`);
  }
}
