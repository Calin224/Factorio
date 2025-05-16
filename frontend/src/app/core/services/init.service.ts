import { inject, Injectable } from '@angular/core';
import { AccountService } from './account.service';
import { catchError, forkJoin, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InitService {
  private accountService = inject(AccountService);

  init(){
    return forkJoin({
      user: this.accountService.getUserInfo()
    }).pipe(
      catchError(error => {
        console.error(error);
        return of({user: null});
      })
    )
  }
}
