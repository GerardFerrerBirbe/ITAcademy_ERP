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
    private location: Location
  ) { }

  editionMode: boolean = false;
  formGroup: FormGroup;
  orderHeaderId: any;
  orderLinesToDelete: number[] = [];

  orderHeader: OrderHeader;
  orderHeaders: OrderHeader[];

  orderStates: OrderState[];
  orderPriorities: OrderPriority[];
  clients: Client[];
  products: Product[];

  get orderLines(): FormArray {
    return this.formGroup.get('orderLines') as FormArray;
  };

  ngOnInit(): void {
    this.formGroup = this.fb.group({
      orderNumber: '',
      address: '',
      client: '',
      orderState: '',
      orderPriority: '',
      orderLines: this.fb.array([])
    });
    
    this.orderStateService.getOrderStates()
    .subscribe(orderStates => this.orderStates = orderStates);

    this.orderPriorityService.getOrderPriorities()
    .subscribe(orderPriorities => this.orderPriorities = orderPriorities);

    this.clientService.getClients()
    .subscribe(clients => this.clients = clients);

    this.productService.getProducts()
    .subscribe(products => this.products = products);

    this.route.params.subscribe(params => {
      if (params["id"] == undefined){
        return;
      }
      this.editionMode = true;

      this.orderHeaderId = params["id"];
      
      this.orderHeaderService.getOrderHeader(this.orderHeaderId.toString())
      .subscribe(orderHeader => {
        this.orderHeader = orderHeader;
        this.loadForm(orderHeader);
      });
    });
  }

  loadForm(orderHeader: OrderHeader){
    this.formGroup.patchValue({
      orderNumber: orderHeader.orderNumber,
      address: orderHeader.address,
      client: orderHeader.client,
      orderState: orderHeader.orderState,
      orderPriority: orderHeader.orderPriority
    });
        
      orderHeader.orderLines.forEach(orderLine => {
      let orderLineFG = this.buildOrderLine();
      orderLineFG.patchValue(orderLine);
      this.orderLines.push(orderLineFG);
      });   
  }

  addOrderLine(){    
    let orderLineFG = this.buildOrderLine();
    this.orderLines.push(orderLineFG);
  }  
  
  buildOrderLine(){
    return this.fb.group({
      id: 0,
      orderHeaderId: this.orderHeaderId != null ? parseInt(this.orderHeaderId) : 0,
      productName: '',
      unitPrice: 0,
      vat: 0,
      quantity: 0
    })
  }  

  deleteOrderLine(index: number){    
    let orderLineToDelete = this.orderLines.at(index) as FormGroup;
    if (orderLineToDelete.controls['id'].value != 0) {
      this.orderLinesToDelete.push(<number>orderLineToDelete.controls['id'].value);
    }
    this.orderLines.removeAt(index);
  }  

  save() {
    let orderHeader: OrderHeader = Object.assign({}, this.formGroup.value);
    console.table(orderHeader);

    if (this.editionMode){
      //edit order     
      this.orderHeaderId = parseInt(this.orderHeaderId);
      orderHeader.id = this.orderHeaderId;     
      this.orderHeaderService.updateOrderHeader(orderHeader)
      .subscribe(() => this.deleteOrderLines());
    } else {
      //add order
      let userName = localStorage.getItem('userName');
      this.employeeService.getEmployeeName(userName)
      .subscribe(employee => orderHeader.employee = employee.firstName + ' ' + employee.lastName);
      this.orderHeaderService.addOrderHeader(orderHeader)
      .subscribe();
    }    
  }

  deleteOrderLines(){
    if(this.orderLinesToDelete.length === 0){
      return;
    }

    this.orderLineService.deleteOrderLines(this.orderLinesToDelete)
      .subscribe();
  }

  goBack(): void {
    this.location.back();
  }

}