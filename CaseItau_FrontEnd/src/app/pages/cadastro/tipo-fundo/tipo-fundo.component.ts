import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ColDef } from 'ag-grid-community';
import { AgGridTableComponent } from '../../../shared/components/ag-grid-table/ag-grid-table.component';
import { TipoFundoApiService, TipoFundoResponse } from '../../../@core/services/api/tipo-fundo-api.service';

@Component({
  selector: 'ngx-tipo-fundo',
  template: `
    <app-ag-grid-table
      title="Tipos de Fundo"
      [columnDefs]="columnDefs"
      [rowData]="rowData"
      [formFields]="[]">
    </app-ag-grid-table>
  `,
  standalone: true,
  imports: [CommonModule, AgGridTableComponent],
})
export class TipoFundoComponent implements OnInit {
  columnDefs: ColDef[] = [];
  rowData: TipoFundoResponse[] = [];

  constructor(private tipoFundoApi: TipoFundoApiService) {}

  ngOnInit(): void {
    this.columnDefs = [
      { field: 'Id', headerName: 'Id', width: 80 },
      { field: 'NmTipoFundo', headerName: 'Nome', flex: 1 },
    ];

    this.tipoFundoApi.getAll().subscribe(data => {
      this.rowData = data;
    });
  }
}
