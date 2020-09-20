import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { HttpClient } from '@angular/common/http';
import { Provider } from '@domain/provider';

@Injectable({
  providedIn: 'root'
})
export class ProviderService {
  private env = environment;
  private controllerName: string = 'Provider';
  private controllerUrl: string = `${this.env.apiUrl}/${this.controllerName}`;
  constructor(private httpClient: HttpClient) { }

  getAll() {
    return this.httpClient.get(`${this.controllerUrl}/GetAll`);
  }

  getById(id: number) {
    return this.httpClient.get(`${this.controllerUrl}/GetById/${id}`);
  }

  post(provider: Provider) {
    return this.httpClient.post(`${this.controllerUrl}`, provider);
  }

  put(id: number, provider: Provider) {
    return this.httpClient.put(`${this.controllerUrl}/${id}`, provider);
  }

  delete(id: number) {
    return this.httpClient.delete(`${this.controllerUrl}/${id}`);
  }
}
