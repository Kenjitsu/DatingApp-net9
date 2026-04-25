import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account.service';
// import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
// import { ToastrService } from 'ngx-toastr';
import { TitleCasePipe } from '@angular/common';

@Component({
    selector: 'app-nav',
    imports: [FormsModule /*BsDropdownModule*/, RouterLink, RouterLinkActive, TitleCasePipe],
    templateUrl: './nav.component.html',
    styleUrl: './nav.component.css'
})
export class NavComponent {
  protected accountService = inject(AccountService);
  private router = inject(Router);
  // private toastr = inject(ToastrService);
  protected creds: any = {};
  protected loggedIn = signal(false);

  login() {
    this.accountService.login(this.creds).subscribe({
      next: (_) => this.router.navigateByUrl('/members'),
      // error: (error) => this.toastr.error(error.error)
    })
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
