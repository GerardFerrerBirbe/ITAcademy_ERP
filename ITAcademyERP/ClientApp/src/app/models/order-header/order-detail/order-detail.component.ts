import { Component, OnInit } from '@angular/core';
import { OrderHeader } from '../order-header';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { OrderHeaderService }  from '../../../services/order-header.service';
import { FormGroup, FormBuilder } from '@angular/forms';

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
    private location: Location
  ) { }

  editionMode: boolean = false;
  formGroup: FormGroup;
  orderHeaderId: any;

  orderHeaders: OrderHeader[];

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
      finalisationDate: ''
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
    })
  }

  save() {
    let orderHeader: OrderHeader = Object.assign({}, this.formGroup.value);
    console.table(orderHeader);

    if (this.editionMode){
      //edit order     
      this.orderHeaderId = parseInt(this.orderHeaderId);
      orderHeader.id = this.orderHeaderId;      
      this.orderHeaderService.updateOrderHeader(orderHeader)
      .subscribe();
    } else {
      //add order
      this.orderHeaderService.addOrderHeader(orderHeader)
      .subscribe();
    }    
  }
  
  goBack(): void {
    this.location.back();
  }

}