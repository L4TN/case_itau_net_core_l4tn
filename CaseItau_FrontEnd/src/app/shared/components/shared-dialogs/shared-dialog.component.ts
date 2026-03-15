import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NbDialogRef, NbCardModule, NbInputModule, NbButtonModule, NbIconModule, NbSelectModule } from '@nebular/theme';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { FormFieldConfig } from '../../../@core/models/form-field.models';
import { TranslationService } from '../../../@core/services/translation.service';
import { TranslatePipe } from '../../../@core/pipes/translate.pipe';

@Component({
  selector: 'app-shared-dialog',
  templateUrl: './shared-dialog.component.html',
  styleUrls: ['./shared-dialog.component.scss'],
  standalone: true,
  imports: [CommonModule, NbCardModule, NbInputModule, NbButtonModule, NbIconModule, NbSelectModule, ReactiveFormsModule, TranslatePipe],
})
export class SharedDialogComponent implements OnInit {
  mode: 'form' | 'delete' | 'export' = 'form';
  title: string = '';
  fields: FormFieldConfig[] = [];
  data: any = null;
  count: number = 0;
  total: number = 0;
  buttonStatus: string = 'primary';

  form!: FormGroup;
  isEditMode = false;

  constructor(
    private dialogRef: NbDialogRef<SharedDialogComponent>,
    private fb: FormBuilder,
    private t: TranslationService,
  ) {}

  ngOnInit(): void {
    if (this.mode === 'form') {
      this.isEditMode = !!this.data;
      this.buildForm();
    }
  }

  private buildForm(): void {
    const group: { [key: string]: any } = {};

    this.fields.forEach(field => {
      const validators = [];
      if (field.required) {
        validators.push(Validators.required);
      }
      if (field.minLength) {
        validators.push(Validators.minLength(field.minLength));
      }
      if (field.maxLength) {
        validators.push(Validators.maxLength(field.maxLength));
      }
      if (field.validators) {
        validators.push(...field.validators);
      }

      const key = field.dataKey || field.name;
      const initialValue = this.isEditMode && this.data[key] !== undefined
        ? this.data[key]
        : (field.defaultValue ?? '');

      group[field.name] = [{ value: initialValue, disabled: this.isEditMode && field.disabled }, validators];
    });

    this.form = this.fb.group(group);
  }

  onInput(event: Event, field: FormFieldConfig): void {
    if (field.mask === 'digits') {
      const input = event.target as HTMLInputElement;
      input.value = input.value.replace(/\D/g, '');
      if (field.maxLength && input.value.length > field.maxLength) {
        input.value = input.value.substring(0, field.maxLength);
      }
      this.form.get(field.name)?.setValue(input.value);
    }
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.dialogRef.close(this.form.getRawValue());
  }

  isInvalid(fieldName: string): boolean {
    const control = this.form.get(fieldName);
    return !!(control && control.invalid && control.touched);
  }

  getErrorMessage(field: FormFieldConfig): string {
    const control = this.form.get(field.name);
    if (!control || !control.errors) return '';
    if (control.errors['required']) return field.label + ' é obrigatório.';
    if (control.errors['minlength']) return field.label + ' deve ter no mínimo ' + field.minLength + ' caracteres.';
    if (control.errors['maxlength']) return field.label + ' deve ter no máximo ' + field.maxLength + ' caracteres.';
    if (field.validationMessage) return field.validationMessage;
    return field.label + ' inválido.';
  }

  cancel(): void {
    if (this.mode === 'form') {
      this.dialogRef.close(null);
    } else {
      this.dialogRef.close(false);
    }
  }

  confirm(): void {
    this.dialogRef.close(true);
  }

  get dialogTitle(): string {
    switch (this.mode) {
      case 'form':
        return this.isEditMode
          ? this.t.get('dialog.editPrefix') + ' - ' + this.title
          : this.t.get('dialog.newPrefix') + ' - ' + this.title;
      case 'delete':
        return this.t.get('dialog.confirmDelete');
      case 'export':
        return this.t.get('dialog.exportCsv');
    }
  }
}
