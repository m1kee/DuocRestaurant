import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';
import { faSave, faTimes, faEdit, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { TableService } from "@services/table.service";
import { Table } from '@domain/table';

@Component({
  selector: 'app-tables',
  templateUrl: './tables.component.html',
  styleUrls: ['./tables.component.css']
})
export class TablesComponent implements OnInit {
  tables: Table[] = [];
  currentTable: Table = new Table();

  icons: any = {
    faSave: faSave,
    faTimes: faTimes,
    faEdit: faEdit,
    faTrashAlt: faTrashAlt
  };

  paginationConfig: any = {
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: this.tables.length
  };

  loading: boolean = false;
  numberPattern = "^[0-9]+$"; 

  constructor(private toastrService: ToastrService,
    private tableService: TableService
  ) { }

  ngOnInit() {
    this.getTables();
  }

  getTables() {
    this.loading = true;
    this.tableService.getAll().subscribe((tables: Table[]) => {
      this.tables = tables;
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
      if (!this.currentTable.Id) {
        // post
        this.tableService
          .post(this.currentTable)
          .subscribe((createdTable: Table) => {
            this.currentTable = createdTable;
            this.tables.push(createdTable);
            this.loading = false;
            this.toastrService.success('Se ha creado correctamente', 'Mesa Creada');
          }, (error) => {
              this.loading = false;
              let message = 'Error al crear la mesa';
              if (error.status === 400) {
                message = error.error;
              }
              this.toastrService.error(message, 'Error');
          });
      } else {
        // put
        this.tableService
          .put(this.currentTable.Id, this.currentTable)
          .subscribe((editedTable: Table) => {
            let cIndex = this.tables.findIndex((c) => c.Id === editedTable.Id);
            this.tables.splice(cIndex, 1, editedTable);
            this.loading = false;
            this.toastrService.success('Se ha editado correctamente', 'Mesa Editada');
          }, (error) => {
            this.loading = false;
            let message = 'Error al editar la mesa';
            if (error.status === 400) {
              message = error.error;
            }
            this.toastrService.error(message, 'Error');
          });
      }
    }
  };

  edit(table: Table, form: NgForm) {
    this.currentTable = table;
    form.form.markAsPristine();
  };

  delete(table: Table) {
    this.loading = true;
    this.tableService.delete(table.Id).subscribe((deletedTable: Table) => {
      let cIndex = this.tables.findIndex((c) => c.Id === deletedTable.Id);
      this.tables.splice(cIndex, 1);
      this.toastrService.success('Se ha eliminado correctamente', 'Mesa Eliminada');
    }, (error) => {
      this.loading = false;
    });
  };

  cancel() {
    this.currentTable = new Table();
  };
}
