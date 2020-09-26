import { Directive } from '@angular/core';
import { NG_VALIDATORS, Validator, AbstractControl, ValidationErrors, ValidatorFn, FormGroup } from '@angular/forms';


export const validatePassword: ValidatorFn = (control: FormGroup): ValidationErrors | null => {
  const password = control.get('password');
  const vPassword = control.get('vpassword');

  let result = null;

  if (password && !password.value)
    result = null;
  else if (password && vPassword && password.value !== vPassword.value) {
    result = { notSame: true };
    vPassword.setErrors({ notSame: true });
  }

  return result;
};


@Directive({
  selector: '[appPasswordValidator]',
  providers: [{ provide: NG_VALIDATORS, useExisting: PasswordValidatorDirective, multi: true }]
})
export class PasswordValidatorDirective implements Validator {
  validate(control: AbstractControl): ValidationErrors {
    return validatePassword(control);
  }
}

