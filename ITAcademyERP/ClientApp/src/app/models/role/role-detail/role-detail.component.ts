import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Role } from '../role';

import { RoleService } from '../role.service';
import { RoleUser } from '../roleUser';

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
  roleFormGroup: FormGroup;
  roleUserFormGroup: FormGroup;
  roleId: any;

  roles: Role[];
  users: RoleUser[];
  currentRoleUsers: RoleUser[];

  ngOnInit(): void {
    this.roleFormGroup = this.fb.group({
      name: ''
    });

    this.roleUserFormGroup = this.fb.group({
      name: '',
    });

    this.route.params.subscribe(params => {
      if (params["id"] == undefined){
        return;
      }
      this.editionMode = true;

      this.roleService.getUsers()
      .subscribe(users => this.users = users);

      this.roleId = params["id"];

      this.roleService.getRole(this.roleId)
      .subscribe(role => {
        this.loadRoleForm(role);
        this.currentRoleUsers = role.roleUsers;
      });
    });    
  }

  loadRoleForm(role: Role){
    this.roleFormGroup.patchValue({
      name: role.name
    });
  }

  addRoleUserLine(){
    let roleUser: RoleUser = Object.assign({}, this.roleUserFormGroup.value);
    console.table(roleUser);
    roleUser.roleId = this.roleId;
    
    this.roleService.updateRoleUser(roleUser, 'add')
    .subscribe(() => this.updateRoleUsers());

    this.roleUserFormGroup = this.fb.group({
      name: '',
    });
  }

  updateRoleUsers(){
    this.roleService.getRole(this.roleId)
    .subscribe(role => this.currentRoleUsers = role.roleUsers);  
  }

  removeUserInRole(roleUserToRemove: RoleUser){
    this.currentRoleUsers = this.currentRoleUsers.filter(r => r !== roleUserToRemove);
    this.roleService.updateRoleUser(roleUserToRemove, 'remove').subscribe();
  }

  save() {
    let role: Role = Object.assign({}, this.roleFormGroup.value);
    console.table(role);

    if (this.editionMode){
      //edit role
      role.id = this.roleId;
      this.roleService.updateRole(role)
      .subscribe(
        () => alert("Actualització realitzada"),
        error => alert(error.error[""])
      );
    } else {
      //add role
      this.roleService.addRole(role)
      .subscribe(
        role => alert("Rol " + role.name + " creat correctament"),
        error => alert(error.error[""])
      );
    }
  }
  
  goBack(): void {
    this.location.back();
  }
}
