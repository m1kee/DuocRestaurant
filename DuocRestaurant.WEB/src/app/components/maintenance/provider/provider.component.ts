import { Component, OnInit } from '@angular/core';
import { Provider } from '@domain/provider';
import { faSave, faTimes, faEdit, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { ProviderService } from '@services/provider.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-provider',
  templateUrl: './provider.component.html',
  styleUrls: ['./provider.component.css']
})
export class ProviderComponent implements OnInit {
  providers: Provider[] = [];
  currentProvider: Provider = new Provider();

  icons: any = {
    faSave: faSave,
    faTimes: faTimes,
    faEdit: faEdit,
    faTrashAlt: faTrashAlt
  };

  paginationConfig: any = {
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: this.providers.length
  };

  loading: boolean = false;

  constructor(private toastrService: ToastrService,
    private providerService: ProviderService
  ) { }

  ngOnInit() {
    this.getProviders();
  }

  getProviders() {
    this.loading = true;
    this.providerService.getAll().subscribe((providers: Provider[]) => {
      this.providers = providers;
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
      if (!this.currentProvider.Id) {
        // post
        this.providerService
          .post(this.currentProvider)
          .subscribe((createdProvider: Provider) => {
            this.currentProvider = createdProvider;
            this.providers.push(createdProvider);
            this.loading = false;
            this.toastrService.success('Se ha creado correctamente', 'Proveedor Creado');
          }, (error) => {
              this.loading = false;
              let message = 'Error al crear el proveedor';
              if (error.status === 400) {
                message = error.error;
              }
              this.toastrService.error(message, 'Error');
          });
      } else {
        // put
        this.providerService
          .put(this.currentProvider.Id, this.currentProvider)
          .subscribe((editedProvider: Provider) => {
            let cIndex = this.providers.findIndex((c) => c.Id === editedProvider.Id);
            this.providers.splice(cIndex, 1, editedProvider);
            this.loading = false;
            this.toastrService.success('Se ha editado correctamente', 'Proveedor Editado');
          }, (error) => {
              this.loading = false;
              let message = 'Error al editar el proveedor';
              if (error.status === 400) {
                message = error.error;
              }
              this.toastrService.error(message, 'Error');
          });
      }
    }
  };

  edit(provider: Provider, form: NgForm) {
    this.currentProvider = provider;
    form.form.markAsPristine();
  };

  delete(provider: Provider) {
    this.loading = true;
    this.providerService.delete(provider.Id).subscribe((deleted: boolean) => {
      let cIndex = this.providers.findIndex((c) => c.Id === provider.Id);
      this.providers.splice(cIndex, 1);
      this.toastrService.success('Se ha eliminado correctamente', 'Proveedor Eliminado');
    }, (error) => {
        this.loading = false;
        let message = 'Error al eliminar el proveedor';
        if (error.status === 400) {
          message = error.error;
        }
        this.toastrService.error(message, 'Error');
    });
  };

  cancel() {
    this.currentProvider = new Provider();
  };
}
