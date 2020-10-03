import { Component, OnInit } from '@angular/core';
import { OrderHeader } from '../order-header';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { OrderHeaderService }  from '../../../models/order-header/order-header.service';
import { OrderLineService } from '../../order-line/order-line.service';
import { OrderStateService } from '../../order-state/order-state.service';
import { OrderPriorityService } from '../../order-priority/order-priority.service';
import { ClientService } from '../../client/client.service';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { OrderState } from '../../order-state/order-state';
import { OrderPriority } from '../../order-priority/order-priority';
import { Client } from '../../client/client';
import { Product } from '../../product/product';
import { ProductService } from 'src/app/models/product/product.service';
import { EmployeeService } from '../../employee/employee.service';
import { Employee } from '../../employee/employee';
import { AccountService } from 'src/app/login/account.service';
import { OrderLine } from '../../order-line/order-line';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.css']
})
export class OrderDetailComponent implements OnInit {  
  
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private orderHeaderService: OrderHeaderService,
    private orderLineService: OrderLineService,
    private orderStateService: OrderStateService,
    private orderPriorityService: OrderPriorityService,
    private clientService: ClientService,
    private employeeService: EmployeeService,
    private productService: ProductService,
    private accountService: AccountService,
    private location: Location
  ) { }

  editionMode: boolean = false;
  userName: string;
  orderHeaderFormGroup: FormGroup;
  orderLineFormGroup: FormGroup;
  orderHeaderId: any;

  orderHeader: OrderHeader;
  orderHeaders: OrderHeader[];
  orderLines: OrderLine[];

  orderStates: OrderState[];
  orderPriorities: OrderPriority[];
  employees: Employee[];
  clients: Client[];
  products: Product[];

  ngOnInit(): void {
    this.orderHeaderFormGroup = this.fb.group({
      orderNumber: '',
      client: '',
      employee: '',
      orderState: '',
      orderPriority: ''
    });

    this.orderLineFormGroup = this.fb.group({
      productName: '',
      unitPrice: 0,
      vat: 21,
      quantity: 0
    });

    this.getUser();
    
    this.orderStateService.getOrderStates()
    .subscribe(orderStates => this.orderStates = orderStates);

    this.orderPriorityService.getOrderPriorities()
    .subscribe(orderPriorities => this.orderPriorities = orderPriorities);

    this.employeeService.getEmployees()
    .subscribe(employees => this.employees = employees);

    this.clientService.getClients()
    .subscribe(clients => this.clients = clients);

    this.productService.getProducts()
    .subscribe(products => this.products = products);

    this.route.params.subscribe(params => {
      if (params["id"] == undefined){
        return;
      }
      this.editionMode = true;

      // this.orderLineService.getOrderLines()
      // .subscribe(orderLines => this.orderLines = orderLines);      

      this.orderHeaderId = params["id"];
      
      this.orderHeaderService.getOrderHeader(this.orderHeaderId.toString())
      .subscribe(orderHeader => {
        this.orderHeader = orderHeader;
        this.loadForm(orderHeader);
        this.orderLines = orderHeader.orderLines;
      });
    });
  }

  loadForm(orderHeader: OrderHeader){
    this.orderHeaderFormGroup.patchValue({
      orderNumber: orderHeader.orderNumber,
      employee: orderHeader.employee,
      client: orderHeader.client,
      orderState: orderHeader.orderState,
      orderPriority: orderHeader.orderPriority
    });   
  }

  addOrderLine(){    
    let orderLine: OrderLine = Object.assign({}, this.orderLineFormGroup.value);
    console.table(orderLine);
    orderLine.orderHeaderId = this.orderHeaderId;

    this.orderLineService.addOrderLine(orderLine)
    .subscribe(() => this.updateOrderLines());

    this.orderLineFormGroup = this.fb.group({
      productName: '',
      unitPrice: 0,
      vat: 21,
      quantity: 0
    })
  }  
  
  updateOrderLines(){
    this.orderHeaderService.getOrderHeader(this.orderHeaderId)
    .subscribe(orderHeader => this.orderLines = orderHeader.orderLines);
  }

  deleteOrderLine(orderLineToRemove: OrderLine){
    this.orderLines = this.orderLines.filter(r => r !== orderLineToRemove);
    this.orderLineService.deleteOrderLine(orderLineToRemove);
  }

  save() {
    let orderHeader: OrderHeader = Object.assign({}, this.orderHeaderFormGroup.value);
    console.table(orderHeader);

    if (this.editionMode){
      //edit order     
      this.orderHeaderId = parseInt(this.orderHeaderId);
      orderHeader.id = this.orderHeaderId;     
      this.orderHeaderService.updateOrderHeader(orderHeader)
      .subscribe();
    } else {
      //add order
      let userName = localStorage.getItem('userName');
      orderHeader.employee = userName;
      this.orderHeaderService.addOrderHeader(orderHeader)
      .subscribe();
    }    
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

  getUser() {
    this.userName = localStorage.getItem('userName');
  }
  
}