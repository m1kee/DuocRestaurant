import { Component, OnInit } from '@angular/core';
import { Product } from '@domain/product';
import { faSave, faTimes, faEdit, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { ProductService } from '@services/product.service';
import { NgForm } from '@angular/forms';
import { MeasurementUnit } from '@domain/measurement-unit';
import { ProductType } from '@domain/product-type';
import { Provider } from '@domain/provider';
import { ProviderService } from '../../../services/provider.service';
import { MeasurementUnitService } from '../../../services/measurement-unit.service';
import { ProductTypeService } from '../../../services/product-type.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {
  products: Product[] = [];
  productTypes: ProductType[] = [];
  measurementUnits: MeasurementUnit[] = [];
  providers: Provider[] = [];
  currentProduct: Product = new Product();

  icons: any = {
    faSave: faSave,
    faTimes: faTimes,
    faEdit: faEdit,
    faTrashAlt: faTrashAlt
  };

  paginationConfig: any = {
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: this.products.length
  };

  loading: boolean = false;
  numberPattern = "^[0-9]+$"; 

  constructor(private toastrService: ToastrService,
    private productService: ProductService,
    private providerService: ProviderService,
    private measurementUnitService: MeasurementUnitService,
    private productTypeService: ProductTypeService
  ) { }

  ngOnInit() {
    this.getProducts();
    this.getProductTypes();
    this.getMeasurementUnits();
    this.getProviders();
  }

  getProducts() {
    this.loading = true;
    this.productService.getAll().subscribe((products: Product[]) => {
      this.products = products;
      this.loading = false;
    }, (error) => {
      this.loading = false;
    });
  };

  getProductTypes() {
    this.loading = true;
    this.productTypeService.getAll().subscribe((productTypes: ProductType[]) => {
      this.productTypes = productTypes;
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

  getMeasurementUnits() {
    this.loading = true;
    this.measurementUnitService.getAll().subscribe((measurementUnits: MeasurementUnit[]) => {
      this.measurementUnits = measurementUnits;
      this.loading = false;
    }, (error) => {
      this.loading = false;
    });
  };

  pageChanged(event) {
    this.paginationConfig.currentPage = event;
  };

  save(form: NgForm) {
    this.loading = true;
    if (form.valid) {
      if (!this.currentProduct.Id) {
        // post
        this.productService
          .post(this.currentProduct)
          .subscribe((createdProduct: Product) => {
            this.currentProduct = createdProduct;
            this.products.push(createdProduct);
            this.loading = false;
            this.toastrService.success('Se ha creado correctamente', 'Producto Creado');
          }, (error) => {
            this.loading = false;
            let message = 'Error al crear el producto';
            if (error.status === 400) {
              message = error.error;
            }
            this.toastrService.error(message, 'Error');
          });
      } else {
        // put
        this.productService
          .put(this.currentProduct.Id, this.currentProduct)
          .subscribe((editedProduct: Product) => {
            let cIndex = this.products.findIndex((c) => c.Id === editedProduct.Id);
            this.products.splice(cIndex, 1, editedProduct);
            this.loading = false;
            this.toastrService.success('Se ha editado correctamente', 'Producto Editado');
          }, (error) => {
            this.loading = false;
            let message = 'Error al editar el producto';
            if (error.status === 400) {
              message = error.error;
            }
            this.toastrService.error(message, 'Error');
          });
      }
    }
  };

  edit(product: Product, form: NgForm) {
    this.currentProduct = product;
    form.form.markAsPristine();
  };

  delete(product: Product) {
    this.loading = true;
    this.productService.delete(product.Id).subscribe((deletedProduct: Product) => {
      let cIndex = this.products.findIndex((c) => c.Id === deletedProduct.Id);
      this.products.splice(cIndex, 1);
      this.toastrService.success('Se ha eliminado correctamente', 'Producto Eliminado');
    }, (error) => {
      this.loading = false;
      let message = 'Error al eliminar el producto';
      if (error.status === 400) {
        message = error.error;
      }
      this.toastrService.error(message, 'Error');
    });
  };

  cancel() {
    this.currentProduct = new Product();
  };
}
