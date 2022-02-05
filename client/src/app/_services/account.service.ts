import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from "rxjs/operators";
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  // make a url var
  basUrl = 'https://localhost:5001/api/';
  private currentUserSource = new ReplaySubject<User>(1)
  currentUser$ = this.currentUserSource.asObservable();

  // get the http client
  constructor(private http: HttpClient) { }

  // make a login function which takes in a model
  login(model: any){
    // http post to the basurl with the code of the model argument
    return this.http.post(this.basUrl + "account/login", model).pipe(
      map((response: User) => {
        const user = response
        if (user) {
          localStorage.setItem("user", JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    )
  }

  register(model: any) {
    return this.http.post(this.basUrl + "account/register", model).pipe(
      map((user: User) => {
        if(user){
          localStorage.setItem('user', JSON.stringify(user))
          this.currentUserSource.next(user)
        }
      })
    )
  }

  setCurrentUser(user: User){
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
