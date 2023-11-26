import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { RouterModule } from '@angular/router';
import { TestErrorComponent } from './test-error/test-error.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { ToastrModule } from 'ngx-toastr';
import { SectionHeaderComponent } from './section-header/section-header.component';
import { BreadcrumbModule } from 'xng-breadcrumb';
//import { NgxSpinnerModule } from 'ngx-spinner';



@NgModule({
  declarations: [ NavBarComponent, TestErrorComponent, NotFoundComponent, SectionHeaderComponent],
  imports: [
    CommonModule,
    RouterModule,
    ToastrModule,
    BreadcrumbModule,
   // NgxSpinnerModule
  ],
  exports:[
    NavBarComponent,
    TestErrorComponent,
    NotFoundComponent,
    SectionHeaderComponent,
   // NgxSpinnerModule
  ]
})
export class CoreModule { }
