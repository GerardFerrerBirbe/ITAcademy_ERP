import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Role } from '../role';

import { RoleService } from '../role.service';

@Component({
  selector: 'app-role-detail',
  templateUrl: './role-detail.component.html',
  styleUrls: ['./role-detail.component.css']
})
export class RoleDetailComponent implements OnInit {

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private roleService: RoleService,
    private location: Location
  ) { }

  editionMode: boolean = false;
  formGroup: FormGroup;
  roleId: any;
  roleUsersToDelete: number[] = [];

  roles: Role[];

  get roleUsers(): FormArray {
    return this.formGroup.get('roleUsers') as FormArray;
  }

  ngOnInit(): void {
    this.formGroup = this.fb.group({
      roleName: '',
      roleUsers: this.fb.array([])
    });  

    this.route.params.subscribe(params => {
      if (params["id"] == undefined){
        return;
      }
      this.editionMode = true;

      this.roleId = params["id"];

      this.roleService.getRole(this.roleId)
      .subscribe(role => this.loadForm(role));
    });    
  }

  loadForm(role: Role){
    this.formGroup.patchValue({
      roleName: role.roleName
    });

    role.roleUsers.forEach(roleUser => {
      let roleUserFG = this.buildRoleUser();
      roleUserFG.patchValue(roleUser);
      this.roleUsers.push(roleUserFG);
    });
  }

  buildRoleUser(){
    return this.fb.group({
      userId: '',
      roleId: this.roleId != null ? this.roleId : '',
      userName: ''
    })
  }

  addRoleUser(){
    let roleUserFG = this.buildRoleUser();
    this.roleUsers.push(roleUserFG);
  }

  deleteRoleUser(index: number){
    let roleUserToDelete = this.roleUsers.at(index) as FormGroup;
    if (roleUserToDelete.controls['id'].value != '') {
      this.roleUsersToDelete.push(<number>roleUserToDelete.controls['id'].value);
    }
    this.roleUsers.removeAt(index);
  }

  save() {
    let role: Role = Object.assign({}, this.formGroup.value);
    console.table(role);

    if (this.editionMode){
      //edit role
      role.roleId = this.roleId;
      this.roleService.updateRole(role)
      .subscribe();
    } else {
      //add role
      this.roleService.addRole(role)
      .subscribe();
    }    
  }
  
  goBack(): void {
    this.location.back();
  }

}
