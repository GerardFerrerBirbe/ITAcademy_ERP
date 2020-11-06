import { Component, OnInit } from '@angular/core';
import { Employee } from '../employee';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { EmployeeService }  from '../employee.service';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { OrderHeaderService } from 'src/app/models/order-header/order-header.service';
import { OrderHeader } from '../../order-header/order-header';
import { AddressService } from '../../address/address.service';
import { AccountService } from 'src/app/login/account.service';
import { Guid } from 'guid-typescript';
import { AddressType } from '../../address/addressType';
import { OrderState } from '../../order-state/order-state';
import { Person } from '../../person/person';
import { OrderPriority } from '../../order-priority/order-priority';
import { Address } from '../../address/address';

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
    private addressService: AddressService,
    private orderHeaderService: OrderHeaderService,
    private accountService: AccountService,
    private location: Location
  ) { }

  editionMode: boolean = false;
  formGroup: FormGroup;
  employeeId: any;
  personId: any;
  addressesToDelete: string[] = [];

  employees: Employee[];  
  currentOhs: OrderHeader[] = [];
  oldOhs: OrderHeader[] = [];
  deliveryAddresses: Address[];
  OrderState = OrderState;
  OrderPriority = OrderPriority;


  errors: any;

  get addresses(): FormArray {
    return this.formGroup.get('addresses') as FormArray;    
  }

  ngOnInit(): void {
    this.formGroup = this.fb.group({
      email:'',
      firstName: '',
      lastName: '',
      addresses: this.fb.array([]),
      position: '',
      salary: 0
    });

    this.route.params.subscribe(params => {
      if (params["id"] == undefined){
        return;
      }
      this.editionMode = true;

      this.employeeId = params["id"];

      this.employeeService.getEmployee(this.employeeId)
      .subscribe(employee => {
        this.loadForm(employee);
        this.personId = employee.person.id;
      });

      this.orderHeaderService.getOHByEmployee(this.employeeId)
      .subscribe(orderHeaders => {
        this.currentOhs = orderHeaders.filter(oh => oh.orderState == OrderState["En repartiment"] || OrderState["En tractament"] ||  OrderState["Pendent de tractar"]);
        this.oldOhs = orderHeaders.filter(oh => oh.orderState == OrderState.Completada || oh.orderState == OrderState.Cancel·lada);
      }
      );   
    });
  }

  addAddress(){
    let addressFG = this.buildAddress();
    this.addresses.push(addressFG);
  }

  buildAddress(){
    return this.fb.group({
      id:  Guid.EMPTY,
      personId: this.personId != null ? this.personId : '',
      name: '',
      type: ''
    })
  }
    
  deleteAddress(index: number){
    let addressToDelete = this.addresses.at(index) as FormGroup;
    if (addressToDelete.controls['id'].value != 0) {
      this.addressesToDelete.push(<string>addressToDelete.controls['id'].value);
    }
    this.addresses.removeAt(index);
  }

  loadForm(employee: Employee){
    this.formGroup.patchValue({
      email: employee.person.email,
      firstName: employee.person.firstName,
      lastName: employee.person.lastName,
      position: employee.position,
      salary: employee.salary
    });

    employee.person.addresses.forEach(address => {
      let addressFG = this.buildAddress();
      addressFG.patchValue({
        id: address.id,
        personId: address.personId,
        name: address.name,
        type: AddressType[address.type]
      });
      this.addresses.push(addressFG);
    });
  }

  save() {
    this.errors = {};
    //let employee: Employee = Object.assign({}, this.formGroup.value);
    let employee: Employee = {
      id: Guid.EMPTY,
      person: <Person>{
        id: '',
        firstName: this.formGroup.get('firstName').value,
        lastName: this.formGroup.get('lastName').value,
        email: this.formGroup.get('email').value,       
        addresses: this.formGroup.get('addresses').value
      },
      position: this.formGroup.get('position').value,
      salary: this.formGroup.get('salary').value
    }; 
    console.table(employee);

    if (this.editionMode){
      //edit employee
      employee.id = this.employeeId;
      employee.person.id = this.personId;     
      this.employeeService.updateEmployee(employee)
      .subscribe(
        () => { this.deleteAddresses();
          alert("Actualització realitzada")},
          error => {
            if (error.error == null) {
              alert(error.status + " Usuari no autoritzat");                            
            } else if (error.error.errors == undefined) {
              this.errors = error.error;
            } else {
              this.errors = error.error.errors;
            }   
          });
    } else {
      //add employee 
      this.employeeService.addEmployee(employee)
      .subscribe(
        () => alert("Empleat " + employee.person.firstName + " " + employee.person.lastName + " creat correctament"),
        error => {
            if (error.error == null) {
              alert(error.status + " Usuari no autoritzat");                            
            } else if (error.error.errors == undefined) {
              this.errors = error.error;
            } else {
              this.errors = error.error.errors;
            }
          });
    }    
  }

  deleteAddresses(){
    if (this.addressesToDelete.length === 0) {
      return;
    }
    this.addressService.deleteAddresses(this.addressesToDelete)
    .subscribe();
  }
  
  goBack(): void {
    this.location.back();
  }

  isAdminUser() {
    if (this.accountService.isLogged() && localStorage.getItem('isAdminUser') == 'true') {      
      return true;
    }
    return false;
  }
}
