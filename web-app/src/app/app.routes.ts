import { Routes } from '@angular/router';
import { HomeComponent } from './home';
import { LoginComponent, RegisterComponent } from './account';
import { AuthGuard } from './_helpers';

export const routes: Routes = [
  { path: '', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'account/login', component: LoginComponent },
  { path: 'account/register', component: RegisterComponent },

  // otherwise redirect to home
  { path: '**', redirectTo: '' },
];
