import { Component, OnInit } from '@angular/core';
import { Client } from './client';
import { ClientService } from 'src/app/services/client.service';


@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.css']
})
export class ClientComponent implements OnInit {

  public clients: Client[];
  
  constructor(private clientService: ClientService) { }

  ngOnInit(): void {
    this.getClients();
  }

  getClients(): void {
    this.clientService.getClients()
    .subscribe(clients => this.clients = clients);
  }

  delete(client: Client): void {
    this.clients = this.clients.filter(e => e !== client);
    this.clientService.deleteClient(client).subscribe();
  }

}
