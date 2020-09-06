import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NgxPaginationModule } from 'ngx-pagination';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { APP_ROUTING } from '@app/app.routing';

import { AppComponent } from './app.component';
import { LoginComponent } from '@components/shared/login/login.component';
import { PageNotFoundComponent } from '@components/shared/page-not-found/page-not-found.component';
import { NavbarComponent } from '@components/shared/navbar/navbar.component';
import { HomeComponent } from '@components/home/home.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    PageNotFoundComponent,
    NavbarComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FontAwesomeModule,
    CollapseModule.forRoot(),
    NgMultiSelectDropDownModule.forRoot(),
    TooltipModule.forRoot(),
    BsDropdownModule.forRoot(),
    APP_ROUTING,
    NgxPaginationModule,
    ToastrModule.forRoot({
      closeButton: true,
      timeOut: 2500,
      extendedTimeOut: 1000,
      disableTimeOut: false,
      easing: 'ease-in',
      easeTime: 300,
      enableHtml: false,
      progressBar: true,
      progressAnimation: 'decreasing',
      toastClass: 'toast',
      positionClass: 'toast-top-center',
      titleClass: 'toast-title',
      messageClass: 'toast-message',
      tapToDismiss: true,
      onActivateTick: false
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
