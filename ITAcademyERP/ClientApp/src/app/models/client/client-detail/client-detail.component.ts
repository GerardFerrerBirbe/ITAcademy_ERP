import { Component, OnInit } from '@angular/core';
import { Client } from '../client';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ClientService }  from '../../../services/client.service';
import { FormGroup, FormBuilder } from '@angular/forms';

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
    private location: Location
  ) { }

  editionMode: boolean = false;
  formGroup: FormGroup;
  clientId: any;
  personId: any;

  clients: Client[];

  ngOnInit(): void {
    this.formGroup = this.fb.group({
      personId: '',
      firstName: '',
      lastName: ''
    });  

    this.route.params.subscribe(params => {
      if (params["id"] == undefined){
        return;
      }
      this.editionMode = true;

      this.clientId = params["id"];

      this.clientService.getClient(this.clientId.toString())
      .subscribe(client => this.loadForm(client));
    });
  }

  loadForm(client: Client){
    this.formGroup.patchValue({
      personId: client.personId,
      firstName: client.firstName,
      lastName: client.lastName
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
      this.personId = 6;
      client.personId = this.personId;
      this.clientService.addClient(client)
      .subscribe();
    }    
  }
  
  goBack(): void {
    this.location.back();
  }

}

