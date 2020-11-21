import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { HttpClient } from '@angular/common/http';
import { Order } from '@domain/order';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
    private env = environment;
    private controllerName: string = 'Order';
    private controllerUrl: string = `${this.env.apiUrl}/${this.controllerName}`;
    constructor(private httpClient: HttpClient) { }

    getAll() {
        return this.httpClient.get(`${this.controllerUrl}/GetAll`);
    }

    getById(id: number) {
        return this.httpClient.get(`${this.controllerUrl}/GetById/${id}`);
    }

    post(order: Order) {
        return this.httpClient.post(`${this.controllerUrl}`, order);
    }

    put(id: number, order: Order) {
        return this.httpClient.put(`${this.controllerUrl}/${id}`, order);
    }

    delete(id: number) {
        return this.httpClient.delete(`${this.controllerUrl}/${id}`);
    }

    filterBy(filters) {
        return this.httpClient.post(`${this.controllerUrl}/FilterBy`, filters);
    }
}
