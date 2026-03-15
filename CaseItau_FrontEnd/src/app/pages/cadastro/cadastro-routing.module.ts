import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FundoComponent } from './fundo/fundo.component';
import { TipoFundoComponent } from './tipo-fundo/tipo-fundo.component';

const routes: Routes = [
  { path: 'fundo', component: FundoComponent },
  { path: 'tipo-fundo', component: TipoFundoComponent },
  { path: '', redirectTo: 'fundo', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CadastroRoutingModule {}
