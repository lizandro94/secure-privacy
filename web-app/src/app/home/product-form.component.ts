import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { NgIf, NgClass } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { first } from 'rxjs/operators';

import { AlertService, ProductsService } from '@app/_services';

@Component({
  standalone: true,
  templateUrl: 'product-form.component.html',
  imports: [NgIf, ReactiveFormsModule, NgClass, RouterLink],
})
export class ProductFormComponent implements OnInit {
  form!: FormGroup;
  id?: string;
  title!: string;
  loading = false;
  submitting = false;
  submitted = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private alertService: AlertService,
    private productsService: ProductsService
  ) {}

  ngOnInit() {
    this.id = this.route.snapshot.params['id'];

    // form with validation rules
    this.form = this.formBuilder.group({
      name: ['', Validators.required],
      quantity: ['', Validators.required],
    });

    this.title = 'Add Product';
  }

  // convenience getter for easy access to form fields
  get f() {
    return this.form.controls;
  }

  onSubmit() {
    this.submitted = true;

    // reset alerts on submit
    this.alertService.clear();

    // stop here if form is invalid
    if (this.form.invalid) {
      return;
    }

    this.submitting = true;
    this.saveProduct()
      .pipe(first())
      .subscribe({
        next: () => {
          this.alertService.success('Product saved', true);
          this.router.navigateByUrl('/');
        },
        error: (error) => {
          this.alertService.error(error);
          this.submitting = false;
        },
      });
  }

  private saveProduct() {
    return this.productsService.create(this.form.value);
  }
}
