import { Component, OnInit } from '@angular/core';
import { OrderHeader } from '../order-header';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { OrderHeaderService }  from '../../../services/order-header.service';
import { OrderLineService } from '../../../services/order-line.service';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';

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
    private location: Location
  ) { }

  editionMode: boolean = false;
  formGroup: FormGroup;
  orderHeaderId: any;
  orderLinesToDelete: number[] = [];

  orderHeaders: OrderHeader[];

  get orderLines(): FormArray {
    return this.formGroup.get('orderLines') as FormArray;
  };

  ngOnInit(): void {
    this.formGroup = this.fb.group({
      orderNumber: '',
      deliveryAddressId: '',
      clientId: '',
      employeeId: '',
      orderStateId: '',
      orderPriorityId: '',
      creationDate: '',
      assignToEmployeeDate: '',
      finalisationDate: '',
      orderLines: this.fb.array([])
    });  

    this.route.params.subscribe(params => {
      if (params["id"] == undefined){
        return;
      }
      this.editionMode = true;

      this.orderHeaderId = params["id"];

      this.orderHeaderService.getOrderHeader(this.orderHeaderId.toString())
      .subscribe(orderHeader => this.loadForm(orderHeader));
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
      productId: 0,
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

  loadForm(orderHeader: OrderHeader){
    this.formGroup.patchValue({
      orderNumber: orderHeader.orderNumber,
      deliveryAddressId: orderHeader.deliveryAddressId,
      clientId: orderHeader.clientId,
      employeeId: orderHeader.employeeId,
      orderStateId: orderHeader.orderStateId,
      orderPriorityId: orderHeader.orderPriorityId,
      creationDate: orderHeader.creationDate,
      assignToEmployeeDate: orderHeader.assignToEmployeeDate,
      finalisationDate: orderHeader.finalisationDate
    });

    orderHeader.orderLines.forEach(orderLine => {
      let orderLineFG = this.buildOrderLine();
      orderLineFG.patchValue(orderLine);
      this.orderLines.push(orderLineFG);
    });
    
  }

  save() {
    let orderHeader: OrderHeader = Object.assign({}, this.formGroup.value);
    console.table(orderHeader);

    if (this.editionMode){
      //edit order     
      this.orderHeaderId = parseInt(this.orderHeaderId);
      orderHeader.id = this.orderHeaderId;      
      this.orderHeaderService.updateOrderHeader(orderHeader)
      .subscribe(orderHeader => this.deleteOrderLines());
    } else {
      //add order
      this.orderHeaderService.addOrderHeader(orderHeader)
      .subscribe();
    }    
  }

  deleteOrderLines(){
    if(this.orderLinesToDelete.length === 0){
      this.goBack();
      return;
    }

    this.orderLineService.deleteOrderLines(this.orderLinesToDelete)
      .subscribe();

  }

  goBack(): void {
    this.location.back();
  }

}