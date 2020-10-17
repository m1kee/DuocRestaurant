import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Product } from '@app/domain/product';
import { Provider } from '@app/domain/provider';
import { SupplyOrder } from '@app/domain/supply-order';
import { SupplyOrderDetail } from '@app/domain/supply-order-detail';
import { DECIMAL_REGEX, NUMBER_REGEX } from '@app/helpers/validations/common-regex';
import { ProductService } from '@app/services/product.service';
import { ProviderService } from '@app/services/provider.service';
import { SuppliesOrderService } from '@app/services/supplies-order.service';
import { faClock, faDollarSign, faEdit, faPlus, faSave, faTimes, faTrashAlt, faUpload, faUndo } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-supply-orders',
  templateUrl: './supply-orders.component.html',
  styleUrls: ['./supply-orders.component.css']
})
export class SupplyOrdersComponent implements OnInit {
  currentOrder: SupplyOrder = new SupplyOrder();
  supplyOrders: SupplyOrder[] = [];
  products: Product[] = [];
  providers: Provider[] = [];

  icons: any = {
    faSave: faSave,
    faTimes: faTimes,
    faEdit: faEdit,
    faTrashAlt: faTrashAlt,
    faPlus: faPlus,
    faUpload: faUpload,
    faClock: faClock,
    faDollarSign: faDollarSign,
    faUndo: faUndo
  };

  paginationConfig: any = {
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: this.supplyOrders.length
  };

  loading: boolean = false;
  numberPattern = NUMBER_REGEX;
  decimalPattern = DECIMAL_REGEX;

  constructor(
    public suppliesOrderService: SuppliesOrderService,
    public productService: ProductService,
    private providerService: ProviderService,
    private toastrService: ToastrService
  ) { };

  ngOnInit() {
    this.getSupplyOrders();
    this.getProviders();
  };

  getSupplyOrders() {
    this.loading = true;
    this.suppliesOrderService.getAll().subscribe((supplyOrders: SupplyOrder[]) => {
      this.supplyOrders = supplyOrders;
      this.loading = false;
    }, (error) => {
      this.loading = false;
    });
  };

  getProducts() {
    this.loading = true;

    let filters = {
      Active: true,
      ProviderId: this.currentOrder.ProviderId
    };

    this.productService.filterBy(filters).subscribe((products: Product[]) => {
      this.products = products;
      this.loading = false;
    }, (error) => {
      this.loading = false;
    });
  };

  getProviders() {
    this.loading = true;
    this.providerService.getAll().subscribe((providers: Provider[]) => {
      this.providers = providers;
      this.loading = false;
    }, (error) => {
      this.loading = false;
    });
  };

  save(form: NgForm) {
    this.loading = true;
    if (form.valid) {
      if (!this.currentOrder.Id) {
        // post
        this.suppliesOrderService
          .post(this.currentOrder)
          .subscribe((created: SupplyOrder) => {
            this.currentOrder = created;
            this.supplyOrders.push(created);
            this.loading = false;
            this.toastrService.success('Se ha creado correctamente', 'Orden Creada');
            form.form.markAsPristine();
          }, (error) => {
            this.loading = false;
            let message = 'Error al crear la orden de compra';
            if (error.status === 400) {
              message = error.error;
            }
            this.toastrService.error(message, 'Error');
          });
      } else {
        // put
        this.suppliesOrderService
          .put(this.currentOrder.Id, this.currentOrder)
          .subscribe((edited: SupplyOrder) => {
            let cIndex = this.supplyOrders.findIndex((c) => c.Id === edited.Id);
            this.supplyOrders.splice(cIndex, 1, edited);
            this.loading = false;
            this.toastrService.success('Se ha editado correctamente', 'Orden Editada');
            form.form.markAsPristine();
          }, (error) => {
            this.loading = false;
            let message = 'Error al editar la orden';
            if (error.status === 400) {
              message = error.error;
            }
            this.toastrService.error(message, 'Error');
          });
      }
    }
  };

  edit(supplyOrder: SupplyOrder, form: NgForm) {
    this.currentOrder = supplyOrder;
    form.form.markAsPristine();
  };

  delete(supplyOrder: SupplyOrder) {
    this.loading = true;
    this.suppliesOrderService.delete(supplyOrder.Id).subscribe((deleted: boolean) => {
      let cIndex = this.supplyOrders.findIndex((c) => c.Id === supplyOrder.Id);
      this.supplyOrders.splice(cIndex, 1);
      this.toastrService.success('Se ha eliminado correctamente', 'Orden Eliminada');
      this.loading = false;
    }, (error) => {
      this.loading = false;
      let message = 'Error al eliminar la orden';
      if (error.status === 400) {
        message = error.error;
      }
      this.toastrService.error(message, 'Error');
    });
  };

  cancel() {
    this.currentOrder = new SupplyOrder();
  };

  deleteSupplyOrderDetail(supplyOrderDetail: SupplyOrderDetail) {
    if (!supplyOrderDetail)
      return;
  };

  getSelectedProductUnitOfMeasurement(productId) {
    if (!productId || !this.products || this.products.length === 0)
      return;

    let product = this.products.find(product => product.Id === productId);
    if (product)
      return product.MeasurementUnit.Code;
  };

  addSupplyOrderDetail() {
    if (!this.currentOrder)
      return;

    let defaultSupplyOrderDetail = new SupplyOrderDetail();
    if (!this.currentOrder.SupplyOrderDetails.find(x => x.SupplyOrderId === defaultSupplyOrderDetail.SupplyOrderId && x.ProductId === defaultSupplyOrderDetail.ProductId && x.Count === defaultSupplyOrderDetail.Count))
      this.currentOrder.SupplyOrderDetails.push(defaultSupplyOrderDetail);
  };

  getFormControlName(text: string, index: number) {
    return `${text}${index}`;
  };

  getOnlyUnselectedProducts(supplyOrderDetail: SupplyOrderDetail) {
    let unselectedProducts = this.products.filter(product => supplyOrderDetail.ProductId === product.Id || !this.currentOrder.SupplyOrderDetails.find(rd => rd.ProductId === product.Id));
    return unselectedProducts;
  };

  pageChanged(currentPage: number) {
    this.paginationConfig.currentPage = currentPage;
  };

}
