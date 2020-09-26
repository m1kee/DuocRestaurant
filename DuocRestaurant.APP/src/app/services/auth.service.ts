import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '@env/environment';
import { User } from '@app/domain/user';
import { Credentials } from '@app/domain/credentials';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private env = environment;
  private currentUserSubject: BehaviorSubject<User>;
  public loggedUser: Observable<User>;
  public storageKey: string = 'logged-user';

  constructor(private httpClient: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem(this.storageKey)));
    this.loggedUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  login(credentials: Credentials) {
    return this.httpClient.post(`${this.env.apiUrl}/Auth/SignIn`, credentials)
      .pipe(map((user: User) => {
        // login successful if there's a jwt token in the response
        // console.log('SignIn: ', user);
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
