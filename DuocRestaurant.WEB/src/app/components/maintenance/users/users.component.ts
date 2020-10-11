import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';
import { faSave, faTimes, faEdit, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { UserService } from "@services/user.service";
import { User } from '@domain/user';
import { Role } from '@domain/role';
import { RoleService } from '@services//role.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: User[] = [];
  currentUser: User = new User();
  roles: Role[] = [];

  icons: any = {
    faSave: faSave,
    faTimes: faTimes,
    faEdit: faEdit,
    faTrashAlt: faTrashAlt
  };

  paginationConfig: any = {
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: this.users.length
  };

  loading: boolean = false;

  constructor(private toastrService: ToastrService,
    private userService: UserService,
    private roleService: RoleService
  ) { }

  ngOnInit() {
    this.getUsers();
    this.getRoles();
  }

  getUsers() {
    this.loading = true;
    this.userService.getAll().subscribe((users: User[]) => {
      this.users = users;
      this.loading = false;
    }, (error) => {
      this.loading = false;
    });
  };

  getRoles() {
    this.roleService.getAll().subscribe((roles: Role[]) => {
      this.roles = roles;
    });
  };

  pageChanged(event) {
    this.paginationConfig.currentPage = event;
  };

  save(form: NgForm) {
    this.loading = true;
    if (form.valid) {
      if (!this.currentUser.Id) {
        // post
        this.userService
          .post(this.currentUser)
          .subscribe((createdUser: User) => {
            this.currentUser = createdUser;
            this.users.push(createdUser);
            this.loading = false;
            this.toastrService.success('Se ha creado correctamente', 'Usuario Creado');
          }, (error) => {
              this.loading = false;
              let message = 'Error al crear el usuario';
              if (error.status === 400) {
                message = error.error;
              }
              this.toastrService.error(message, 'Error');
          });
      } else {
        // put
        this.userService
          .put(this.currentUser.Id, this.currentUser)
          .subscribe((editedUser: User) => {
            let cIndex = this.users.findIndex((c) => c.Id === editedUser.Id);
            this.users.splice(cIndex, 1, editedUser);
            this.loading = false;
            this.toastrService.success('Se ha editado correctamente', 'Usuario Editado');
          }, (error) => {
              this.loading = false;
              let message = 'Error al editar el usuario';
              if (error.status === 400) {
                message = error.error;
              }
              this.toastrService.error(message, 'Error');
          });
      }
    }
  };

  edit(user: User, form: NgForm) {
    this.currentUser = user;
    form.form.markAsPristine();
  };

  delete(user: User) {
    this.loading = true;
    this.userService.delete(user.Id).subscribe((deleted: boolean) => {
      let cIndex = this.users.findIndex((c) => c.Id === user.Id);
      this.users.splice(cIndex, 1);
      this.toastrService.success('Se ha eliminado correctamente', 'Usuario Eliminado');
    }, (error) => {
        this.loading = false;
        let message = 'Error al eliminar el usuario';
        if (error.status === 400) {
          message = error.error;
        }
        this.toastrService.error(message, 'Error');
    });
  };

  cancel() {
    this.currentUser = new User();
  };
}
