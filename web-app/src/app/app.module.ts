import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { AppComponent } from './app.component';
import { HomeComponent } from './home';
import { LoginComponent, RegisterComponent } from './account';

@NgModule({
  imports: [BrowserModule, ReactiveFormsModule, CommonModule],
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
