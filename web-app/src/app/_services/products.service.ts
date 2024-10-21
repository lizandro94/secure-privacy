import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '@environments/environments';
import { Product, User } from '@app/_models';
import { AccountService } from '@app/_services';

@Injectable({ providedIn: 'root' })
export class ProductsService {
  user: User | null;
  constructor(
    private accountService: AccountService,
    private http: HttpClient
  ) {
    this.user = this.accountService.userValue;
  }

  getAll() {
    return this.http.get<Product[]>(
      `${environment.apiUrl}/users/${this.user?.id}/products`
    );
  }
}
