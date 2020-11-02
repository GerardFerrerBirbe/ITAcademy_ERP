import { Component } from '@angular/core';
import { AccountService } from './login/account.service';
import { Router } from '@angular/router';
import { NavBarComponent } from './nav-bar/nav-bar.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ERP IT Academy';
  
  public userName = '';   
  
  constructor(
    private accountService: AccountService,
    private navBarComponent: NavBarComponent,
    private router: Router
  ) {}    

  logout() {
    this.accountService.logout();
    this.userName = '';
    this.navBarComponent.isAdminUser = 'false';
    this.navBarComponent.navMenu = '';
    this.router.navigate(['/']);
  }

  isLogged() {
    if (this.accountService.isLogged()) {
      this.userName = localStorage.getItem('userName');
      return true;
    }
    return false;
  }
}
