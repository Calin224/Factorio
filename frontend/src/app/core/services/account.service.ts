import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { map, tap } from 'rxjs';
import { User } from '../../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl: string = 'https://localhost:5000/api/';
  private http = inject(HttpClient);

  currentUser = signal<User | null>(null);

  login(values: any){
    let params = new HttpParams();
    params = params.append('useCookies', true);

    return this.http.post<User>(this.baseUrl + 'login', values, {withCredentials: true, params}).pipe(
      tap((user: User) => {
        console.log("user din account_service: ", user);
        this.currentUser.set(user);
      })
    )
  }

  register(values: any){
    return this.http.post<User>(this.baseUrl + 'account/register', values);
  }

  getUserInfo(){
    return this.http.get<User>(this.baseUrl + 'account/user-info', {withCredentials: true}).pipe(
      map(user => {
        console.log("user info retrieved: ", user);
        this.currentUser.set(user);
        return user;
      })
    )
  }

  logout(){
    this.currentUser.set(null);
    return this.http.post(this.baseUrl + 'account/logout', {}, {withCredentials: true});
  }
}
