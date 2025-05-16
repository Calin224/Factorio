import { Component, inject } from '@angular/core';
import {Router, RouterLink} from '@angular/router';
import { AccountService } from '../../core/services/account.service';

@Component({
  selector: 'app-header',
  imports: [
    RouterLink
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  accountService = inject(AccountService);
  private router = inject(Router);

  logout(){
    this.accountService.logout().subscribe({
      next: () => {
        this.accountService.currentUser.set(null);
        this.router.navigateByUrl('/');
      }
    })
  }

  loginWithGoogle() {
    window.location.href = 'https://localhost:5000/api/account/external-login';
  }
}
