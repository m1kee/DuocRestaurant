import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SupplyRequest } from '@app/domain/supply-request';
import { environment } from '@env/environment';

@Injectable({
    providedIn: 'root'
})
export class SupplyRequestService {
    private env = environment;
    private controllerName: string = 'SupplyRequest';
    private controllerUrl: string = `${this.env.apiUrl}/${this.controllerName}`;
    constructor(private httpClient: HttpClient) { }

    getAll() {
        return this.httpClient.get(`${this.controllerUrl}/GetAll`);
    }

    getById(id: number) {
        return this.httpClient.get(`${this.controllerUrl}/GetById/${id}`);
    }

    getByCode(code: string) {
        return this.httpClient.get(`${this.controllerUrl}/GetByCode/${code}`);
    }

    post(supplyRequest: SupplyRequest) {
        return this.httpClient.post(`${this.controllerUrl}`, supplyRequest);
    }

    put(id: number, supplyRequest: SupplyRequest) {
        return this.httpClient.put(`${this.controllerUrl}/${id}`, supplyRequest);
    }

    delete(id: number) {
        return this.httpClient.delete(`${this.controllerUrl}/${id}`);
    }

    filterBy(filters: any) {
        return this.httpClient.post(`${this.controllerUrl}/FilterBy`, filters);
    }

    finalize(request: any) {
        return this.httpClient.post(`${this.controllerUrl}/Finalize`, request);
    }
}
