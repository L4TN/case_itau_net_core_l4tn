import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NbCardModule, NbSelectModule, NbCheckboxModule } from '@nebular/theme';
import { NgxEchartsModule } from 'ngx-echarts';
import { AgGridModule } from 'ag-grid-angular';
import * as echarts from 'echarts';
import { ConsultaComponent } from './consulta.component';
import { MovimentacaoComponent } from '../movimentacao/movimentacao.component';

const routes: Routes = [
  { path: '', redirectTo: 'movimentacao', pathMatch: 'full' },
  { path: 'movimentacao', component: MovimentacaoComponent },
  { path: 'posicao', component: ConsultaComponent },
];

@NgModule({
  declarations: [ConsultaComponent],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild(routes),
    MovimentacaoComponent,
    NbCardModule,
    NbSelectModule,
    NbCheckboxModule,
    NgxEchartsModule.forRoot({ echarts }),
    AgGridModule,
  ],
})
export class ConsultaModule {}
