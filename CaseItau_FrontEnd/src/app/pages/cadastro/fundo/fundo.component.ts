import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { forkJoin } from 'rxjs';
import { ColDef } from 'ag-grid-community';
import { AgGridTableComponent } from '../../../shared/components/ag-grid-table/ag-grid-table.component';
import { FormFieldConfig } from '../../../@core/models/form-field.models';
import { FundoApiService, FundoResponse } from '../../../@core/services/api/fundo-api.service';
import { TipoFundoApiService } from '../../../@core/services/api/tipo-fundo-api.service';
import { NotificationService } from '../../../@core/services/notification.service';
import { cnpjValidator } from '../../../@core/utils/cnpj.validator';

@Component({
  selector: 'ngx-fundo',
  template: `
    <app-ag-grid-table
      title="Fundos"
      [columnDefs]="columnDefs"
      [rowData]="rowData"
      [loading]="loading"
      [formFields]="formFields"
      (onAdd)="onAdd($event)"
      (onEdit)="onEdit($event)"
      (onDeleteSelected)="onDelete($event)">
    </app-ag-grid-table>
  `,
  standalone: true,
  imports: [CommonModule, AgGridTableComponent],
})
export class FundoComponent implements OnInit {
  @ViewChild(AgGridTableComponent) grid!: AgGridTableComponent;

  columnDefs: ColDef[] = [];
  formFields: FormFieldConfig[] = [];
  rowData: FundoResponse[] = [];
  loading: boolean = true;

  constructor(
    private fundoApi: FundoApiService,
    private tipoFundoApi: TipoFundoApiService,
    private notification: NotificationService,
  ) {}

  ngOnInit(): void {
    this.columnDefs = [
      { field: 'Id', headerName: 'Id', width: 80, checkboxSelection: true, headerCheckboxSelection: true },
      { field: 'CdFundo', headerName: 'Código', width: 120 },
      { field: 'NmFundo', headerName: 'Nome', flex: 1 },
      { field: 'NrCnpj', headerName: 'CNPJ', width: 160 },
      { field: 'NmTipoFundo', headerName: 'Tipo Fundo', width: 150 },
      {
        field: 'VlrPatrimonio', headerName: 'Patrimônio', width: 160,
        valueFormatter: (params: any) => {
          if (params.value == null) return '—';
          return params.value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
        },
      },
    ];

    this.tipoFundoApi.getAll().subscribe(tipos => {
      this.formFields = [
        { name: 'Codigo', label: 'Código', type: 'text', required: true, placeholder: 'Ex: FUND001', dataKey: 'CdFundo', disabled: true },
        { name: 'Nome', label: 'Nome', type: 'text', required: true, placeholder: 'Nome do fundo', dataKey: 'NmFundo' },
        { name: 'Cnpj', label: 'CNPJ', type: 'text', required: true, placeholder: '00000000000000', maxLength: 14, mask: 'digits', halfWidth: true, dataKey: 'NrCnpj', validators: [cnpjValidator], validationMessage: 'CNPJ inválido.' },
        {
          name: 'TipoFundoId', label: 'Tipo Fundo', type: 'select', required: true, halfWidth: true, dataKey: 'IdTipoFundo',
          options: tipos.map(t => ({ value: t.Id, label: t.NmTipoFundo })),
        },
      ];
    });

    this.loadData();
  }

  loadData(): void {
    this.loading = true;
    this.fundoApi.getAll().subscribe(data => {
      this.rowData = data;
      this.loading = false;
      if (this.grid) this.grid.refreshData(data);
    });
  }

  onAdd(data: any): void {
    this.fundoApi.create(data).subscribe(() => {
      this.loadData();
      this.notification.successCreate();
    });
  }

  onEdit(event: { original: FundoResponse; updated: any }): void {
    this.fundoApi.update(event.original.CdFundo, {
      Nome: event.updated.Nome,
      Cnpj: event.updated.Cnpj,
      TipoFundoId: event.updated.TipoFundoId,
    }).subscribe(() => {
      this.loadData();
      this.notification.successUpdate();
    });
  }

  onDelete(rows: FundoResponse[]): void {
    forkJoin(rows.map(r => this.fundoApi.delete(r.CdFundo))).subscribe(() => {
      this.loadData();
      this.notification.successDelete(rows.length);
    });
  }
}
