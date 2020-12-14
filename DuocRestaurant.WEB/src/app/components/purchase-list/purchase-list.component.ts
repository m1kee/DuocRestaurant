import { Component, OnInit, TemplateRef } from '@angular/core';
import { Purchase } from '@domain/purchase';
import { PurchaseService } from '@services/purchase.service';
import { AuthService } from '@services/auth.service';
import { User } from '@domain/user';
import { PurchaseState } from '@domain/enums';
import { faEye, faMoneyBillAlt } from '@fortawesome/free-solid-svg-icons';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { Order } from '../../domain/order';
import { OrderService } from '../../services/order.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-purchase-list',
  templateUrl: './purchase-list.component.html',
  styleUrls: ['./purchase-list.component.css']
})
export class PurchaseListComponent implements OnInit {

  purchases: Purchase[] = [];
  currentUser: User;
  purchaseStates = PurchaseState;
  bsModalRef: BsModalRef;

  paginationConfig: any = {
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: this.purchases.length
  };

  icons: any = {
    faEye: faEye,
    faMoneyBillAlt: faMoneyBillAlt
  };

  constructor(private purchaseService: PurchaseService,
    private authService: AuthService,
    private modalService: BsModalService,
    private toastService: ToastrService
  ) { }

  ngOnInit() {
    this.refreshPayments();
  }

  refreshPayments = async () => {
    
    let filters = {
      Month: new Date().getMonth()
    };

    this.purchaseService.filterBy(filters).subscribe((purchases: Purchase[]) => {
      if (purchases && purchases[0]) {
        this.purchases = purchases;
      }
      else {
        this.purchases = null;
      }
    }, async () => {
      this.purchases = null;
    });
  };
  validate = async (purchase: Purchase) => {
    this.purchaseService.validatePayment(purchase.Id).subscribe((validatedPurchase: Purchase) => {
      let cIndex = this.purchases.findIndex((c) => c.Id === validatedPurchase.Id);
      this.purchases.splice(cIndex, 1, validatedPurchase);
    }, (error) => {
        this.toastService.error('Ocurrió un error al validar el pago');
    });
  };
  view(purchase: Purchase, template: TemplateRef<any>) {
    const initialState = {
      purchaseId: purchase.Id,
      title: 'Detalle compra',
      closeBtnName: 'Cerrar'
    };
    this.bsModalRef = this.modalService.show(ModalContentComponent, { initialState });
  };
  getStateBadgeColor = (purchase: Purchase) => {
    let color = '';

    switch (purchase.StateId) {
      case this.purchaseStates.PendingPayment:
        color = 'light';
        break;
      case this.purchaseStates.Rejected:
        color = 'danger';
        break;
      case this.purchaseStates.Canceled:
        color = 'warning';
        break;
      case this.purchaseStates.Paid:
        color = 'success';
        break;
      default:
        color = 'light';
        break;
    }

    return color;
  };
  getStateBadge = (purchase: Purchase) => {
    let state = '';

    switch (purchase.StateId) {
      case this.purchaseStates.PendingPayment:
        state = 'Pendiente';
        break;
      case this.purchaseStates.Canceled:
        state = 'Cancelada';
        break;
      case this.purchaseStates.Rejected:
        state = 'Rechazada';
        break;
      case this.purchaseStates.Paid:
        state = 'Pagada';
        break;
      default:
        state = 'Pendiente';
        break;
    }

    return state;
  };
  pageChanged = (event) => {
    this.paginationConfig.currentPage = event;
  };
}

@Component({
  selector: 'modal-content',
  template: `
    <div class="modal-header">
      <h4 class="modal-title pull-left">{{title}}</h4>
      <button type="button" class="close pull-right" aria-label="Close" (click)="bsModalRef.hide()">
        <span aria-hidden="true">&times;</span>
      </button>
    </div>
    <div class="modal-body">
      <div class="list-group">
        <div class="list-group-item list-group-item-action flex-column align-items-start" *ngFor="let order of orders">
          <div class="d-flex w-100 justify-content-between">
            <h5 class="mb-1">{{getOrderTotal(order) | currency:'CLP':'symbol-narrow'}}</h5>
          </div>
          <ul>
            <li *ngFor="let detail of order.OrderDetails">
                {{detail.Count}} x {{ detail.ProductId ? detail.Product.Name : detail.Recipe.Name }}
            </li>
          </ul>
          <small>Nota: {{order.Note}}</small>
        </div>
      </div>
    </div>
    <div class="modal-footer">
      <button type="button" class="btn btn-default" (click)="bsModalRef.hide()">{{closeBtnName}}</button>
    </div>
  `
})

export class ModalContentComponent implements OnInit {
  title: string;
  closeBtnName: string;
  purchaseId: number;
  orders: Order[] = [];

  constructor(public bsModalRef: BsModalRef,
    private orderService: OrderService
  ) { }

  ngOnInit() {
    this.getOrders();
  }

  getOrders = async () => {
    let filters = {
      PurchaseId: this.purchaseId
    };
    this.orderService.filterBy(filters).subscribe((orders: Order[]) => {
      this.orders = orders;
    }, async (error) => {
      this.orders = [];
    });
  };

  getOrderTotal = (order: Order) => {
    let total = 0;

    if (order && order.OrderDetails) {
      for (let i = 0; i < order.OrderDetails.length; i++) {
        let detail = order.OrderDetails[i];

        total += detail.Price;
      }
    }

    return total;
  };
}
