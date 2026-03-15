import { ValidatorFn } from '@angular/forms';

export interface FormFieldConfig {
  name: string;              // nome do campo no DTO de envio (ex: 'Codigo')
  label: string;             // rótulo exibido (ex: 'Código')
  type: 'text' | 'number' | 'select' | 'textarea' | 'password';
  required?: boolean;
  minLength?: number;
  maxLength?: number;
  mask?: 'digits';           // 'digits' = só números
  placeholder?: string;
  options?: { value: any; label: string }[];  // para select
  defaultValue?: any;
  halfWidth?: boolean;       // lado-a-lado
  dataKey?: string;          // chave no objeto de dados (para preencher no edit). Se não informado, usa name.
  disabled?: boolean;        // campo desabilitado no modo edit
  validators?: ValidatorFn[];         // custom validators
  validationMessage?: string;         // mensagem de erro customizada
}