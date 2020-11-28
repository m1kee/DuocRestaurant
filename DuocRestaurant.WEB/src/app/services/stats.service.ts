import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class StatsService {
  private env = environment;
  private controllerName: string = 'Stats';
  private controllerUrl: string = `${this.env.apiUrl}/${this.controllerName}`;
  constructor(private httpClient: HttpClient) { }

  getMonthlySells() {
    return this.httpClient.get(`${this.controllerUrl}/GetMonthlySells`);
  }
}
