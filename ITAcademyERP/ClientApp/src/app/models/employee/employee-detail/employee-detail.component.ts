import { Component, OnInit } from '@angular/core';
import { Employee } from '../employee';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { EmployeeService }  from '../../../services/employee.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { OrderHeaderService } from 'src/app/services/order-header.service';
import { OrderHeader } from '../../order-header/order-header';

@Component({
  selector: 'app-employee-detail',
  templateUrl: './employee-detail.component.html',
  styleUrls: ['./employee-detail.component.css']
})
export class EmployeeDetailComponent implements OnInit {

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private employeeService: EmployeeService,
    private orderHeaderService: OrderHeaderService,
    private location: Location
  ) { }

  editionMode: boolean = false;
  formGroup: FormGroup;
  employeeId: any;

  employees: Employee[];  
  currentOhs: OrderHeader[];
  oldOhs: OrderHeader[];

  ngOnInit(): void {
    this.formGroup = this.fb.group({
      email:'',
      firstName: '',
      lastName: '',
      address: '',
      position: '',
      salary: ''
    });  

    this.route.params.subscribe(params => {
      if (params["id"] == undefined){
        return;
      }
      this.editionMode = true;

      this.employeeId = params["id"];

      this.employeeService.getEmployee(this.employeeId.toString())
      .subscribe(employee => this.loadForm(employee));

      this.orderHeaderService.getOHByEmployee(this.employeeId)
      .subscribe(orderHeaders => {
        this.currentOhs = orderHeaders.filter(oh => oh.orderState == "En repartiment" || oh.orderState == "En tractament" ||  oh.orderState == "Pendent de tractar"); 
        this.oldOhs = orderHeaders.filter(oh => oh.orderState == "Complet" || oh.orderState == "Cancel·lat");
      }
      );   
    });
  }

  getCurrentOhs(orderHeaders: OrderHeader[]){
    this.currentOhs = orderHeaders.filter(oh => oh.orderState == "En repartiment"); 
  }

  getOldOhs(orderHeaders: OrderHeader[]){
    this.oldOhs = orderHeaders.filter(oh => oh.orderState == "Complet"); 
  }

  loadForm(employee: Employee){
    this.formGroup.patchValue({
      email: employee.email,
      firstName: employee.firstName,
      lastName: employee.lastName,
      address: employee.address,
      position: employee.position,
      salary: employee.salary
    })
  }

  save() {
    let employee: Employee = Object.assign({}, this.formGroup.value);
    console.table(employee);

    if (this.editionMode){
      //edit employee     
      this.employeeId = parseInt(this.employeeId);
      employee.id = this.employeeId;      
      this.employeeService.updateEmployee(employee)
      .subscribe();
    } else {
      //add employee
      this.employeeService.addEmployee(employee)
      .subscribe();
    }    
  }
  
  goBack(): void {
    this.location.back();
  }

}
