import {Component} from '@angular/core';
import {Routes} from '@angular/router';
import {HomeComponent} from './features/home/home.component';
import {RegisterComponent} from './features/account/register/register.component';
import {LoginComponent} from './features/account/login/login.component';

export const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'account/register', component: RegisterComponent},
  {path: 'account/login', component: LoginComponent},
];
