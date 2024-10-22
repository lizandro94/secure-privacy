import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { first } from 'rxjs/operators';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { User } from '@app/_models';

import { AccountService, AlertService } from '@app/_services';

@Component({
  standalone: true,
  templateUrl: 'account.component.html',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
})
export class AccountComponent {
  user: User;
  isDeleting = false;

  constructor(
    private accountService: AccountService,
    private alertService: AlertService
  ) {
    this.user = this.accountService.userValue || {};
  }

  deleteUser(id?: string) {
    if (!id) {
      this.alertService.error('User not found');
      return;
    }
    this.isDeleting = true;
    this.accountService
      .delete(id)
      .pipe(first())
      .subscribe(() => this.accountService.logout());
  }
}
