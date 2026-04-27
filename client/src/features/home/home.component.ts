import { Component, Input, signal } from '@angular/core';
import { RegisterComponent } from "../../features/account/register/register.component";
import { User } from '../../types/user';

@Component({
    selector: 'app-home',
    imports: [RegisterComponent],
    templateUrl: './home.component.html',
    styleUrl: './home.component.css'
})
export class HomeComponent {
  // protected registerMode = false;
  protected registerMode = signal(true);

  // registerToggle() {
  //   this.registerMode = !this.registerMode;
  // }

  // cancelRegisterMode(event: boolean) {
  //   this.registerMode = event;
  // }

  showRegister(value: boolean) {
    this.registerMode.set(value);
  }


}
