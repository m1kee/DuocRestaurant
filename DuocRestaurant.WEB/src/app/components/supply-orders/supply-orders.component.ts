import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { SupplyRequestStates } from '@app/domain/enums';
import { Product } from '@app/domain/product';
import { Provider } from '@app/domain/provider';
import { SupplyRequest } from '@app/domain/supply-request';
import { SupplyRequestDetail } from '@app/domain/supply-request-detail';
import { DECIMAL_REGEX, NUMBER_REGEX } from '@app/helpers/validations/common-regex';
import { ProductService } from '@app/services/product.service';
import { ProviderService } from '@app/services/provider.service';
import { SupplyRequestService } from '@app/services/supply-request.service';
import { faClock, faDollarSign, faEdit, faPlus, faSave, faTimes, faTrashAlt, faUpload, faUndo } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-supply-orders',
  templateUrl: './supply-orders.component.html',
  styleUrls: ['./supply-orders.component.css']
})
export class SupplyOrdersComponent implements OnInit {
  currentOrder: SupplyRequest = new SupplyRequest();
  supplyRequests: SupplyRequest[] = [];
  products: Product[] = [];
  providers: Provider[] = [];
  supplyRequestStates = SupplyRequestStates;

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
    totalItems: this.supplyRequests.length
  };

  loading: boolean = false;
  numberPattern = NUMBER_REGEX;
  decimalPattern = DECIMAL_REGEX;

  constructor(
    public supplyRequestService: SupplyRequestService,
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
    this.supplyRequestService.getAll().subscribe((supplyRequests: SupplyRequest[]) => {
      this.supplyRequests = supplyRequests;
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
        this.supplyRequestService
          .post(this.currentOrder)
          .subscribe((created: SupplyRequest) => {
            this.currentOrder = created;
            this.supplyRequests.push(created);
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
        this.supplyRequestService
          .put(this.currentOrder.Id, this.currentOrder)
          .subscribe((edited: SupplyRequest) => {
            let cIndex = this.supplyRequests.findIndex((c) => c.Id === edited.Id);
            this.supplyRequests.splice(cIndex, 1, edited);
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

  edit(supplyOrder: SupplyRequest, form: NgForm) {
    this.currentOrder = supplyOrder;
    form.form.markAsPristine();
  };

  delete(supplyOrder: SupplyRequest) {
    this.loading = true;
    this.supplyRequestService.delete(supplyOrder.Id).subscribe((deleted: boolean) => {
      let cIndex = this.supplyRequests.findIndex((c) => c.Id === supplyOrder.Id);
      this.supplyRequests.splice(cIndex, 1);
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
    this.currentOrder = new SupplyRequest();
  };

  deleteSupplyOrderDetail(supplyOrderDetail: SupplyRequestDetail) {
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

    let defaultSupplyOrderDetail = new SupplyRequestDetail();
    if (!this.currentOrder.SupplyOrderDetails.find(x => x.SupplyOrderId === defaultSupplyOrderDetail.SupplyOrderId && x.ProductId === defaultSupplyOrderDetail.ProductId && x.Count === defaultSupplyOrderDetail.Count))
      this.currentOrder.SupplyOrderDetails.push(defaultSupplyOrderDetail);
  };

  getFormControlName(text: string, index: number) {
    return `${text}${index}`;
  };

  getOnlyUnselectedProducts(supplyOrderDetail: SupplyRequestDetail) {
    let unselectedProducts = this.products.filter(product => supplyOrderDetail.ProductId === product.Id || !this.currentOrder.SupplyOrderDetails.find(rd => rd.ProductId === product.Id));
    return unselectedProducts;
  };

  pageChanged(currentPage: number) {
    this.paginationConfig.currentPage = currentPage;
  };

}
