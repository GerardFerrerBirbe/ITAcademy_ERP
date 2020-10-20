import { Component } from '@angular/core';
import { AccountService } from './login/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ERP IT Academy';

  constructor(private accountService: AccountService,
    private router: Router) {}

    isExpanded = false;
    navMenu = '';
    public userName = '';
    public isAdminUser = 'false';    

    logout() {
      this.accountService.logout();
      this.userName = '';
      this.isAdminUser = 'false';
      this.router.navigate(['/']);
    }

    isLogged() {
      if (this.accountService.isLogged()) {
        this.userName = localStorage.getItem('userName');
        this.isAdminUser = localStorage.getItem('isAdminUser');
        return true;
      }
      return false;
    }
}
