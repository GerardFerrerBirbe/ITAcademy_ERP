import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AccountService } from '../services/account.service';


@Injectable()
export class AuthGuardService implements CanActivate {

  constructor(private accountService: AccountService,
    private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
      if (this.accountService.isLogged()) {
        return true;
      } else {
        this.router.navigate(['/login']);
        return false;
      }
    }
}
