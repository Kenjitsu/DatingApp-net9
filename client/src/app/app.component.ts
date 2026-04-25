import { Component, inject } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { NavComponent } from "../layout/nav/nav.component";
// import { NgxSpinnerComponent } from 'ngx-spinner';

@Component({
    selector: 'app-root',
    imports: [RouterOutlet, NavComponent],
    templateUrl: './app.component.html',
    styleUrl: './app.component.css'
})
export class AppComponent {
  protected router = inject(Router);


}
