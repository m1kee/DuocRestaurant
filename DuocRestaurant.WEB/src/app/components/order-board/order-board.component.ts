import { Component, OnInit, OnDestroy } from '@angular/core';
import { faAngleRight, faAngleDown, faUser, faHourglassHalf, faUtensils, faCheck, faSpinner, faWineGlassAlt, faHamburger } from '@fortawesome/free-solid-svg-icons';
import { OrderService } from '@services/order.service';
import { Order } from '@domain/order';
import { OrderState } from '@domain/enums';
import { ToastrService } from 'ngx-toastr';
import * as signalR from '@aspnet/signalr';
import { environment } from '@env/environment';

@Component({
  selector: 'app-order-board',
  templateUrl: './order-board.component.html',
  styleUrls: ['./order-board.component.css']
})
export class OrderBoardComponent implements OnInit, OnDestroy {
  private env = environment;
  orders: Order[] = [];
  orderStates = OrderState;
  hubConnection: signalR.HubConnection;

  icons: any = {
    faAngleRight: faAngleRight,
    faAngleDown: faAngleDown,
    faUser: faUser,
    faHourglassHalf: faHourglassHalf,
    faUtensils: faUtensils,
    faCheck: faCheck,
    faSpinner: faSpinner,
    faHamburger: faHamburger,
    faWineGlassAlt: faWineGlassAlt
  };

  constructor(private orderService: OrderService, private toastrService: ToastrService) { }

  ngOnInit()  {
    this.getOrders();

    this.startHubConnection();
  }

  ngOnDestroy()  {
    this.endHubConnection();
  }

  getOrders = () => {
    let filters = {
      States: [this.orderStates.Pending, this.orderStates.InPreparation]
    };
    this.orderService.filterBy(filters).subscribe((orders: Order[]) => {
      this.orders = orders;
    });
  };

  changeOrderState = (order: Order, orderState: OrderState, previousState: OrderState) => {
    order._isLoading = true;

    order.StateId = orderState;
    this.orderService.put(order.Id, order).subscribe((edited: Order) => {
      order._isLoading = false;
      order = edited;

      if (order.StateId === this.orderStates.Ready) {
        let index = this.orders.findIndex((c) => c.Id === order.Id);
        this.orders.splice(index, 1);

        this.toastrService.success('Pedido completado con éxito');
      }
    }, (error) => {
      order._isLoading = false;
      order.StateId = previousState;

      let message = 'Error al actualizar la orden';
      if (error.status === 400) {
        message = error.error;
      }
      this.toastrService.error(message, 'Error');
    });
  };

  toggleDetails = (order: Order) => {
    order._showDetails = !order._showDetails;
  };

  getMaxPreparationTime = (order: Order) => {
    let mins = 0;

    if (order.OrderDetails) {
      for (let i = 0; i < order.OrderDetails.length; i++) {
        let detail = order.OrderDetails[i];
        if (detail.Recipe && mins < detail.Recipe.PreparationTime)
          mins = detail.Recipe.PreparationTime;
      }
    }

    return mins;
  };

  getOrderStateText = (order: Order) => {
    let state = '';
    switch (order.StateId) {
      case this.orderStates.Pending:
        state = 'Pendiente';
        break;
      case this.orderStates.InPreparation:
        state = 'En preparación';
        break;
      case this.orderStates.Ready:
        state = 'Listo';
        break;
      case this.orderStates.Canceled:
        state = 'Cancelado';
        break;
      default:
        state = 'No asignado';
        break;
    }
    return state;
  };

  getOrderStateColor = (order: Order) => {
    let state = '';
    switch (order.StateId) {
      case this.orderStates.Pending:
        state = 'badge-secondary';
        break;
      case this.orderStates.InPreparation:
        state = 'badge-warning';
        break;
      case this.orderStates.Ready:
        state = 'badge-success';
        break;
      case this.orderStates.Canceled:
        state = 'badge-danger';
        break;
      default:
        state = 'badge-secondary';
        break;
    }
    return state;
  };

  startHubConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.env.hubUrl}/orders`)
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('Connection started')
        this.hubConnection.on('ReloadOrders', (data) => {
          this.getOrders();
        })
      })
      .catch(err => {
        console.log('Error while starting connection: ' + err)
        setTimeout(() => {
          this.startHubConnection();
        }, 10000);
      });
  };

  endHubConnection = () => {
    this.hubConnection
      .stop()
      .then(() => console.log('Connection closed'))
      .catch((err) => console.log('Error while closing connection: ' + err));
  };
}
