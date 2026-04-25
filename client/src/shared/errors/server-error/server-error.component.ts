import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { ApiError } from '../../../types/error';
import { Location } from '@angular/common';

@Component({
    selector: 'app-server-error',
    imports: [],
    templateUrl: './server-error.component.html',
    styleUrl: './server-error.component.css'
})
export class ServerErrorComponent {
  private router = inject(Router)
  private location = inject(Location)
  protected error: ApiError;
  protected showDetails = false;

  constructor() {
    const navigation = this.router.currentNavigation();
    this.error = navigation?.extras?.state?.['error'];    
  }

  datailsToggle() {
    this.showDetails = !this.showDetails;
  }


  goBack() {
    this.location.back();
  }
}
