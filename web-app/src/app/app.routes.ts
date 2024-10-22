import { Routes } from '@angular/router';
import { HomeComponent } from './home';
import { LoginComponent, RegisterComponent, AccountComponent } from './account';
import { authGuard } from './_helpers';

const productsRoutes = () =>
  import('./home/products.routes').then((x) => x.PRODUCTS_ROUTES);

export const routes: Routes = [
  { path: '', component: HomeComponent, canActivate: [authGuard] },
  { path: 'products', loadChildren: productsRoutes, canActivate: [authGuard] },
  { path: 'account/login', component: LoginComponent },
  { path: 'account/register', component: RegisterComponent },
  { path: 'account', component: AccountComponent },

  // otherwise redirect to home
  { path: '**', redirectTo: '' },
];
