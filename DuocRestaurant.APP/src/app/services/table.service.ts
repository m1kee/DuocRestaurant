import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';
import { Table } from '@app/domain/table';

@Injectable({
  providedIn: 'root'
})
export class TableService {
  private env = environment;
  private controllerName: string = 'Table';
  private controllerUrl: string = `${this.env.apiUrl}/${this.controllerName}`;
  constructor(private httpClient: HttpClient) { }


  getAll() {
    return this.httpClient.get(`${this.controllerUrl}/GetAll`);
  }

  getById(id: number) {
    return this.httpClient.get(`${this.controllerUrl}/GetById/${id}`);
  }

  post(table: Table) {
    return this.httpClient.post(`${this.controllerUrl}`, table);
  }

  put(id: number, table: Table) {
    return this.httpClient.put(`${this.controllerUrl}/${id}`, table);
  }

  delete(id: number) {
    return this.httpClient.delete(`${this.controllerUrl}/${id}`);
  }
}
