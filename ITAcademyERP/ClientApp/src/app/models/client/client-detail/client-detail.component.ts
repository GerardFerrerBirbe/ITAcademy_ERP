import { Component, OnInit } from '@angular/core';
import { Client } from '../client';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ClientService }  from '../client.service';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { OrderHeaderService } from 'src/app/models/order-header/order-header.service';
import { OrderHeader } from '../../order-header/order-header';
import { AddressService } from '../../address/address.service';
import { AccountService } from 'src/app/login/account.service';

@Component({
  selector: 'app-client-detail',
  templateUrl: './client-detail.component.html',
  styleUrls: ['./client-detail.component.css']
})
export class ClientDetailComponent implements OnInit {
  
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private clientService: ClientService,
    private addressService: AddressService,
    private orderHeaderService: OrderHeaderService,
    private accountService: AccountService, 
    private location: Location
  ) { }

  editionMode: boolean = false;
  formGroup: FormGroup;
  clientId: any;
  personId: any;
  addressesToDelete: number[] = [];

  clients: Client[];
  currentOhs: OrderHeader[];
  oldOhs: OrderHeader[];
  totalOrderAmountByClient: number;
  totalOrderNumberByClient: number;
  

  get addresses(): FormArray {
    return this.formGroup.get('addresses') as FormArray;
  };

  ngOnInit(): void {
    this.formGroup = this.fb.group({
      email: '',
      firstName: '',
      lastName: '',
      addresses: this.fb.array([])
    });  

    this.route.params.subscribe(params => {
      if (params["id"] == undefined){
        return;
      }
      this.editionMode = true;

      this.clientId = params["id"];      

      this.clientService.getClient(this.clientId.toString())
      .subscribe(client => {
        this.loadForm(client);
        this.personId = client.personId;
      });

      this.orderHeaderService.getOHByClient(this.clientId)
      .subscribe(orderHeaders => {
        this.calculateTotalClient(orderHeaders);
        this.currentOhs = orderHeaders.filter(oh => oh.orderState == "En repartiment" || oh.orderState == "En tractament" ||  oh.orderState == "Pendent de tractar"); 
        this.oldOhs = orderHeaders.filter(oh => oh.orderState == "Completat" || oh.orderState == "Cancel·lat");
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
      id: 0,
      personId: this.personId != null ? this.personId : '',
      name: '',
      type: ''
    })
  }
    
  deleteAddress(index: number){
    let addressToDelete = this.addresses.at(index) as FormGroup;
    if (addressToDelete.controls['id'].value != 0) {
      this.addressesToDelete.push(<number>addressToDelete.controls['id'].value);
    }
    this.addresses.removeAt(index);
  }

  loadForm(client: Client){
    this.formGroup.patchValue({
      email: client.email,
      firstName: client.firstName,
      lastName: client.lastName
    });

    client.addresses.forEach(address => {
      let addressFG = this.buildAddress();
      addressFG.patchValue(address);
      this.addresses.push(addressFG);
    });
  }

  save() {
    let client: Client = Object.assign({}, this.formGroup.value);
    console.table(client);

    if (this.editionMode){
      //edit client     
      this.clientId = parseInt(this.clientId);
      client.id = this.clientId;      
      this.clientService.updateClient(client)
      .subscribe(
        () => alert("Actualització realitzada"),
        error => alert(error.error[""])
      );
    } else {
      //add client
      this.clientService.addClient(client)
      .subscribe(
        () => alert("Client " + client.firstName + " " + client.lastName + " creat correctament"),
        error => alert(error.error[""])
      );
    }    
  }

  deleteAddresses(){
    if (this.addressesToDelete.length === 0) {
      return;
    }

    this.addressService.deleteAddresses(this.addressesToDelete)
    .subscribe();
  }

  calculateTotalClient(orderHeaders: OrderHeader[]){

    this.totalOrderAmountByClient = 0;
    this.totalOrderNumberByClient = orderHeaders.length;    

    orderHeaders.forEach(orderHeader => {
      let orderLines = orderHeader.orderLines;
            orderLines.forEach(orderLine => {
        this.totalOrderAmountByClient += orderLine.unitPrice * orderLine.quantity * (1 + orderLine.vat);
      });
    });
  }

  isAdminUser() {
    if (this.accountService.isLogged() && localStorage.getItem('isAdminUser') == 'true') {
      return true;
    }
    return false;
  }
  
  goBack(): void {
    this.location.back();
  }

}

