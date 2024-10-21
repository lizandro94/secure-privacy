import { Component } from '@angular/core';
import { NgFor, NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';
import { first } from 'rxjs/operators';

import { User } from '@app/_models';
import { AccountService, ProductsService } from '@app/_services';

@Component({
  standalone: true,
  templateUrl: 'home.component.html',
  imports: [RouterLink, NgFor, NgIf],
})
export class HomeComponent {
  user: User | null;
  products?: any[];

  constructor(
    private accountService: AccountService,
    private productsService: ProductsService
  ) {
    this.user = this.accountService.userValue;
  }

  ngOnInit() {
    this.productsService
      .getAll()
      .pipe(first())
      .subscribe((products) => (this.products = products));
  }
}
