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
  roleUserLinesToDelete: string[] = [];

  roles: Role[];

  get roleUsers(): FormArray {
    return this.formGroup.get('roleUsers') as FormArray;
  }

  ngOnInit(): void {
    this.formGroup = this.fb.group({
      name: '',
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
      name: role.name
    });

    role.roleUsers.forEach(roleUser => {
      let roleUserFG = this.buildRoleUser();
      roleUserFG.patchValue(roleUser);
      this.roleUsers.push(roleUserFG);
    });
  }

  buildRoleUser(){
    return this.fb.group({
      id: '',
      roleId: this.roleId != null ? this.roleId : '',
      name: ''
    })
  }

  addRoleUser(){
    let roleUserFG = this.buildRoleUser();
    this.roleUsers.push(roleUserFG);
  }

  deleteRoleUser(index: number){
    let roleUserToDelete = this.roleUsers.at(index) as FormGroup;
    if (roleUserToDelete.controls['id'].value != '') {
      this.roleUserLinesToDelete.push(<string>roleUserToDelete.controls['id'].value);
    }
    this.roleUsers.removeAt(index);
  }

  save() {
    let role: Role = Object.assign({}, this.formGroup.value);
    console.table(role);

    if (this.editionMode){
      //edit role
      role.id = this.roleId;
      this.roleService.updateRole(role)
      .subscribe(() => this.deleteRoleUserLines());
    } else {
      //add role
      this.roleService.addRole(role)
      .subscribe();
    }    
  }

  deleteRoleUserLines(){
    if (this.roleUserLinesToDelete.length === 0) {
      return;
    }

    this.roleService.deleteRoleUserLines(this.roleUserLinesToDelete, this.roleId)
    .subscribe();
  }
  
  goBack(): void {
    this.location.back();
  }

}
