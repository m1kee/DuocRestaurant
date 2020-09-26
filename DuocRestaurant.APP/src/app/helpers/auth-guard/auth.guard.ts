import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '@services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private authService: AuthService) { }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot)
    : Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    // obtenemos el usuario que está dentro del local storage 
    let loggedUser = this.authService.currentUserValue;
    // si obtenemos un valor quiere decir que está autenticado dentro de la app
    // TODO: validación de permisología

    if (state && state.url === '/login') {
      if (loggedUser) {
        this.router.navigate(['/home']);
      }
      else {
        return true;
      }
    }

    if (loggedUser) {
      return true;
    }

    if (state && (state.url === '/home')) {
      this.router.navigate(['/login']);
    }
    else {
      // retornamos al login con el url al cual queríamos dirigirnos
      // de tal forma que una vez realizado el login, podemos volver a esa misma página
      this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    }
  }

}
