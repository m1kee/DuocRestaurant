import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  private env = environment;
  private controllerName: string = 'Role';
  private controllerUrl: string = `${this.env.apiUrl}/${this.controllerName}`;
  constructor(private httpClient: HttpClient) { }


  getAll() {
    return this.httpClient.get(`${this.controllerUrl}/GetAll`);
  }
}
