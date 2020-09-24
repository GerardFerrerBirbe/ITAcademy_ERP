import { Component, OnInit } from '@angular/core';
import { Role } from './role';
import { RoleService } from 'src/app/services/role.service';

@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.css']
})
export class RoleComponent implements OnInit {

  public roles: Role[];
  
  constructor(private roleService: RoleService) { }

  ngOnInit(): void {
    this.getRoles();
  }

  getRoles(): void {
    this.roleService.getRoles()
    .subscribe(roles => this.roles = roles);
  }

  delete(role: Role): void {
    this.roles = this.roles.filter(r => r !== role);
    this.roleService.deleteRole(role).subscribe();
  }

}
