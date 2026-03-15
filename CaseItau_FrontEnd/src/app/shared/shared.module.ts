import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { NbDialogModule, NbCardModule, NbInputModule, NbSpinnerModule, NbIconModule, NbButtonModule } from '@nebular/theme';

@NgModule({
  declarations: [
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NbDialogModule,
    NbCardModule,
    NbInputModule,
    NbSpinnerModule,
    NbIconModule,
    NbButtonModule,
  ],
  exports: [
    CommonModule,
    ReactiveFormsModule,
    NbDialogModule,
    NbCardModule,
    NbInputModule,
    NbSpinnerModule,
    NbIconModule,
    NbButtonModule,
  ],
})
export class SharedModule {}
