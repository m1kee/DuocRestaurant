import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class MailService {
    private env = environment;
    private controllerName: string = 'Mail';
    private controllerUrl: string = `${this.env.apiUrl}/${this.controllerName}`;
    constructor(private httpClient: HttpClient) { }

    sendMail(mailRequest) {
        return this.httpClient.post(`${this.controllerUrl}/SendMail`, mailRequest);
    }
}
