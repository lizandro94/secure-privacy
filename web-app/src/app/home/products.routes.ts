import { Routes } from '@angular/router';

import { ProductFormComponent } from './product-form.component';

export const PRODUCTS_ROUTES: Routes = [
  { path: 'add', component: ProductFormComponent },
];
