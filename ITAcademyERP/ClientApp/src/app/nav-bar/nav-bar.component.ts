import { Component, OnInit } from '@angular/core';
import { AccountService } from '../login/account.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {

  public navMenu = '';
  public isAdminUser = 'false';
  
  constructor(
    private accountService: AccountService
  ) { }

  ngOnInit(): void {    
  }

  isLogged() {
    if (this.accountService.isLogged()) {
      this.isAdminUser = localStorage.getItem('isAdminUser');
      return true;
    }
    return false;
  }
  
}
