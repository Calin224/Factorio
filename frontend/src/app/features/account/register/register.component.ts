import {Component, inject} from '@angular/core';
import {FormBuilder, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {AccountService} from '../../../core/services/account.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  accountService = inject(AccountService);
  private router = inject(Router);

  registerForm = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', Validators.required],
    password: ['', Validators.required],
  })

  onSubmit(){
    this.accountService.register(this.registerForm.value).subscribe({
      next: () => {
        console.log("Login successfull");
        this.router.navigateByUrl('/account/login');
      }
    })
  }
}
