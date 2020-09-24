import { Component, OnInit } from '@angular/core';
import { Client } from '../client';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ClientService }  from '../../../services/client.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { OrderHeaderService } from 'src/app/services/order-header.service';
import { OrderHeader } from '../../order-header/order-header';

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
    private orderHeaderService: OrderHeaderService,    
    private location: Location
  ) { }

  editionMode: boolean = false;
  formGroup: FormGroup;
  clientId: any;
  personId: any;

  clients: Client[];
  currentOhs: OrderHeader[];
  oldOhs: OrderHeader[];

  ngOnInit(): void {
    this.formGroup = this.fb.group({
      email: '',
      firstName: '',
      lastName: '',
      address:''
    });  

    this.route.params.subscribe(params => {
      if (params["id"] == undefined){
        return;
      }
      this.editionMode = true;

      this.clientId = params["id"];

      this.clientService.getClient(this.clientId.toString())
      .subscribe(client => this.loadForm(client));

      this.orderHeaderService.getOHByClient(this.clientId)
      .subscribe(orderHeaders => {
        this.currentOhs = orderHeaders.filter(oh => oh.orderState == "En repartiment" || oh.orderState == "En tractament" ||  oh.orderState == "Pendent de tractar"); 
        this.oldOhs = orderHeaders.filter(oh => oh.orderState == "Complet" || oh.orderState == "Cancel·lat");
      
      }
      );
    });
  }

  loadForm(client: Client){
    this.formGroup.patchValue({
      email: client.email,
      firstName: client.firstName,
      lastName: client.lastName,
      address: client.address
    })
  }

  save() {
    let client: Client = Object.assign({}, this.formGroup.value);
    console.table(client);

    if (this.editionMode){
      //edit client     
      this.clientId = parseInt(this.clientId);
      client.id = this.clientId;      
      this.clientService.updateClient(client)
      .subscribe();
    } else {
      //add client
      this.clientService.addClient(client)
      .subscribe();
    }    
  }
  
  goBack(): void {
    this.location.back();
  }

}

