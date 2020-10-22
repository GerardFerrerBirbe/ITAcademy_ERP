import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { HhrrComponent } from './hhrr.component';
import { EmployeeComponent } from 'src/app/models/employee/employee.component';
import { EmployeeDetailComponent } from 'src/app/models/employee/employee-detail/employee-detail.component';
import { RoleComponent } from 'src/app/models/role/role.component';
import { RoleDetailComponent } from 'src/app/models/role/role-detail/role-detail.component';

import { EmployeeService } from 'src/app/models/employee/employee.service';
import { RoleService } from 'src/app/models/role/role.service';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { AccountService } from 'src/app/login/account.service';

@NgModule({
  declarations: [
    HhrrComponent,
    EmployeeComponent,
    EmployeeDetailComponent,
    RoleComponent,
    RoleDetailComponent
  ],
  imports: [
    CommonModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    EmployeeService,
    RoleService
  ]
})
export class HhrrModule { }
