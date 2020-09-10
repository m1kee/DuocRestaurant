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
  icons: any = {
    faSave: faSave,
    faTimes: faTimes,
    faEdit: faEdit,
    faTrashAlt: faTrashAlt
  };

  currentTable: Table = new Table();

  paginationConfig: any = {
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: this.tables.length
  };
  loading: boolean = false;

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
    });
  };

  tablePageChanged(event) {
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
          });
      }
    }
  };

  edit(table: Table) {
    this.currentTable = table;
  };

  delete(table: Table) {
    this.tableService.delete(table.Id).subscribe((deletedTable: Table) => {
      let cIndex = this.tables.findIndex((c) => c.Id === deletedTable.Id);
      this.tables.splice(cIndex, 1);
      this.toastrService.success('Se ha eliminado correctamente', 'Mesa Eliminada');
    });
  };

  cancel() {
    this.currentTable = new Table();
  };
}
