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
    public userName = '';
    public adminUser = 'false';
    

    collapse() {
      this.isExpanded = false;
    }

    toggle() {
      this.isExpanded = !this.isExpanded;
    }

    logout() {
      this.accountService.logout();
      this.userName = '';
      this.adminUser = 'false';
      this.router.navigate(['/']);
    }

    isLogged() {
      if (this.accountService.isLogged()) {
        this.userName = localStorage.getItem('userName');
        this.adminUser = localStorage.getItem('adminUser');
        return true;
      }
      return false;
    }
}
