import { Component, OnInit } from '@angular/core';
import { LoadingController } from '@ionic/angular';
import { SupplyRequestService } from '@app/services/supply-request.service';
import { SupplyRequest } from '@app/domain/supply-request';

@Component({
  selector: 'app-supply-request-reception',
  templateUrl: './supply-request-reception.page.html',
  styleUrls: ['./supply-request-reception.page.scss'],
})
export class SupplyRequestReceptionPage implements OnInit {

  supplyRequestCode: string = null;

  constructor(public loadingController: LoadingController,
    public supplyRequestService: SupplyRequestService) { }

  ngOnInit() {
    this.getCreatedRequests();
  }

  async getCreatedRequests() {

  }

  async searchSupplyRequest() {
    if (!this.supplyRequestCode)
    return;

    let loading = await this.loadingController.create({
      message: `Cargando orden de compra ${this.supplyRequestCode}`
    });
    await loading.present();

    this.supplyRequestService.getByCode(this.supplyRequestCode).subscribe((supplyRequest: SupplyRequest) => {
      
    })

  }
}
