import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Booking } from '@app/domain/booking';
import { environment } from '@env/environment';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private env = environment;
  private controllerName: string = 'Booking';
  private controllerUrl: string = `${this.env.apiUrl}/${this.controllerName}`;
  constructor(private httpClient: HttpClient) { }

  getAll() {
    return this.httpClient.get(`${this.controllerUrl}/GetAll`);
  }

  getById(id: number) {
    return this.httpClient.get(`${this.controllerUrl}/GetById/${id}`);
  }

  getByCode(code: number) {
    return this.httpClient.get(`${this.controllerUrl}/GetByCode/${code}`);
  }

  post(booking: Booking) {
    return this.httpClient.post(`${this.controllerUrl}`, booking);
  }

  put(id: number, booking: Booking) {
    return this.httpClient.put(`${this.controllerUrl}/${id}`, booking);
  }

  delete(id: number) {
    return this.httpClient.delete(`${this.controllerUrl}/${id}`);
  }

  filterBy(filters) {
    return this.httpClient.post(`${this.controllerUrl}/FilterBy`, filters);
  }
}
