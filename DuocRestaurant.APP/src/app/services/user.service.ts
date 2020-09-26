import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';
import { User } from '@app/domain/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private env = environment;
  private controllerName: string = 'User';
  private controllerUrl: string = `${this.env.apiUrl}/${this.controllerName}`;
  constructor(private httpClient: HttpClient) { }


  getAll() {
    return this.httpClient.get(`${this.controllerUrl}/GetAll`);
  }

  getById(id: number) {
    return this.httpClient.get(`${this.controllerUrl}/GetById/${id}`);
  }

  post(user: User) {
    return this.httpClient.post(`${this.controllerUrl}`, user);
  }

  put(id: number, user: User) {
    return this.httpClient.put(`${this.controllerUrl}/${id}`, user);
  }

  delete(id: number) {
    return this.httpClient.delete(`${this.controllerUrl}/${id}`);
  }
}
