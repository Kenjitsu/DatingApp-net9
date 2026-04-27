import { Component, inject, OnInit, output, signal } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { AccountService } from '../../../core/services/account.service';
// import { DatePickerComponent } from "../../app/_forms/date-picker/date-picker.component";
import { Router } from '@angular/router';
import { RegisterCreds } from '../../../types/user';
import { JsonPipe } from '@angular/common';
import { TextInputComponent } from "../../../shared/text-input/text-input.component";

@Component({
    selector: 'app-register',
    imports: [ReactiveFormsModule, JsonPipe ,TextInputComponent],
    templateUrl: './register.component.html',
    styleUrl: './register.component.css'
})
export class RegisterComponent {
  protected creds = {} as RegisterCreds;
  private accountService = inject(AccountService);
  private fb = inject(FormBuilder);
  private router = inject(Router);
  cancelRegister = output<boolean>();
  protected credentialsForm: FormGroup;
  protected profileForm: FormGroup;
  protected currentStep = signal(1);
  validationErrors: string[] | undefined;

  constructor() {
    this.credentialsForm = this.fb.group({
      email: [ '', [ Validators.required, Validators.email ] ],
      displayName: [ '', Validators.required ],
      password: [ '', [ Validators.required, Validators.minLength(4), Validators.maxLength(8) ] ],
      confirmPassword: [ '', [ Validators.required, this.matchValues('password') ] ],
    });

    this.profileForm = this.fb.group({
      gender: [ '', Validators.required ],
      dateOfBirth: [ '', Validators.required ],
      city: [ '', Validators.required ],
      country: [ '', Validators.required ],
      
    })

    this.credentialsForm.controls[ 'password' ].valueChanges.subscribe(() => {
      this.credentialsForm.controls[ 'confirmPassword' ].updateValueAndValidity();
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      const parent = control.parent;
      if (!parent) return null;

      const matchValue = parent.get(matchTo)?.value;
      return control.value === matchValue ? null : { passwordMissmatch: true };
    }
  }

  nextStep() {
    if (this.credentialsForm.valid) {
      this.currentStep.update(prevStep => prevStep + 1);
    }
  }

  prevStep() {
    this.currentStep.update(prevStep => prevStep - 1); 
  }

  getMaxDate() {
    const today = new Date();

    today.setFullYear(today.getFullYear() - 18);
    return today.toISOString().split('T')[0];
  }

  register() {
    if (this.profileForm.valid && this.credentialsForm.valid) {
      const formData = { ...this.credentialsForm.value, ...this.profileForm.value };
      console.log('Form data: ', formData);
    }
    // const dob = this.getDateOnly(this.registerForm.get('dateOfBirth')?.value);
    // this.registerForm.patchValue({ dateOfBirth: dob });
    // this.accountService.register(this.registerForm.value).subscribe({
    //   next: (_) => this.router.navigateByUrl('/members'),
    //   error: (error) => this.validationErrors = error,
    // })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

  private getDateOnly(dob: string | undefined) {
    if (!dob) return;

    return new Date(dob).toISOString().slice(0, 10);
  }
}
