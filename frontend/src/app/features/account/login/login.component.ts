import {Component, inject} from '@angular/core';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {AccountService} from '../../../core/services/account.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  private fb = new FormBuilder();
  private router = inject(Router);
  accountService = inject(AccountService);

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });

  onSubmit(){
    this.accountService.login(this.loginForm.value).subscribe({
      next: result => {
        this.accountService.getUserInfo().subscribe();
        console.log("result: ", result);
        this.router.navigateByUrl('/');
      }
    })
  }
}
