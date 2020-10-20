import { Component, OnInit } from '@angular/core';
import { LoadingController } from '@ionic/angular';
import { SupplyRequestService } from '@app/services/supply-request.service';
import { SupplyRequest } from '@app/domain/supply-request';

@Component({
  selector: 'app-supply-order-reception',
  templateUrl: './supply-order-reception.page.html',
  styleUrls: ['./supply-order-reception.page.scss'],
})
export class SupplyOrderReceptionPage implements OnInit {

  supplyOrderCode: string = null;

  constructor(public loadingController: LoadingController,
    public supplyRequestService: SupplyRequestService) { }

  ngOnInit() {
  }

  async searchSupplyOrder() {
    if (!this.supplyOrderCode)
    return;

    let loading = await this.loadingController.create({
      message: `Cargando orden de compra ${this.supplyOrderCode}`
    });
    await loading.present();

    this.supplyRequestService.getByCode(this.supplyOrderCode).subscribe((supplyRequest: SupplyRequest) => {
      
    })

  }
}
