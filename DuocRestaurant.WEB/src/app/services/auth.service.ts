import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '@env/environment';
import { IUser } from '@domain/user';
import { ICredentials } from '@domain/credentials';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private env = environment;
  private currentUserSubject: BehaviorSubject<IUser>;
  public loggedUser: Observable<IUser>;
  public storageKey: string = 'logged-user';

  constructor(private httpClient: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<IUser>(JSON.parse(localStorage.getItem(this.storageKey)));
    this.loggedUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): IUser {
    return this.currentUserSubject.value;
  }

  login(credentials: ICredentials) {
    return this.httpClient.post(`${this.env.apiUrl}/Auth/SignIn`, credentials)
      .pipe(map((user: IUser) => {
        // login successful if there's a jwt token in the response
        console.log('SignIn: ', user);
        if (user) {
          // store user details and jwt token in local storage to keep user logged in between page refreshes
          localStorage.setItem(this.storageKey, JSON.stringify(user));
          this.currentUserSubject.next(user);
        }

        return user;
      }));
  }

  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem(this.storageKey);
    this.currentUserSubject.next(null);
  }

  isAuthenticated() {
    let isAuth = false;
    if (this.currentUserValue)
      isAuth = true;

    return isAuth;
  }

  hasPermissions(requiredRoles: number[]) {
    return requiredRoles.includes(this.currentUserValue.RoleId);
  };
}
