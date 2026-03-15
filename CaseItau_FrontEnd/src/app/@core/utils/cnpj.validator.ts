import { AbstractControl, ValidationErrors } from '@angular/forms';

export function isValidCnpj(cnpj: string): boolean {
  if (!cnpj) return false;

  cnpj = cnpj.replace(/\D/g, '');

  if (cnpj.length !== 14) return false;

  if (/^(\d)\1{13}$/.test(cnpj)) return false;

  const digits = cnpj.split('').map(Number);

  const weights1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
  let sum = 0;
  for (let i = 0; i < 12; i++) {
    sum += digits[i] * weights1[i];
  }
  const check1 = sum % 11 < 2 ? 0 : 11 - (sum % 11);
  if (digits[12] !== check1) return false;

  const weights2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
  sum = 0;
  for (let i = 0; i < 13; i++) {
    sum += digits[i] * weights2[i];
  }
  const check2 = sum % 11 < 2 ? 0 : 11 - (sum % 11);
  return digits[13] === check2;
}

export function cnpjValidator(control: AbstractControl): ValidationErrors | null {
  if (!control.value) return null;
  return isValidCnpj(control.value) ? null : { cnpjInvalid: true };
}
