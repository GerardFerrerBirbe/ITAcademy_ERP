import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { IUserInfo } from './user-info';
import { AccountService } from './account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private fb: FormBuilder,
    private accountService: AccountService,
    private router: Router) { }
  
  formGroup: FormGroup;

  ngOnInit() {
    this.formGroup = this.fb.group({
      email: '',
      password: '',
    });
  }

  login() {
    let userInfo: IUserInfo = Object.assign({}, this.formGroup.value);
    this.accountService.login(userInfo).subscribe(token => this.getToken(token),
      error => this.manageError(error));
  }

  register() {
    let userInfo: IUserInfo = Object.assign({}, this.formGroup.value);
    this.accountService.create(userInfo).subscribe(token => this.getToken(token),
      error => this.manageError(error));
  }

  getToken(token) {
    localStorage.setItem('token', token.token);
    localStorage.setItem('tokenExpiration', token.expiration);
    localStorage.setItem('userName', token.userName);
    localStorage.setItem('adminUser', token.adminUser);
    this.router.navigate([""]);
  }

  manageError(error) {
    if (error && error.error) {
      alert(error.error[""]);
    }
  }

}
