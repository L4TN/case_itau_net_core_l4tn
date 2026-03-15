import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ColDef } from 'ag-grid-community';
import { NbCardModule, NbSelectModule, NbInputModule, NbButtonModule, NbIconModule, NbCheckboxModule } from '@nebular/theme';
import { AgGridModule } from 'ag-grid-angular';
import { FundoApiService, FundoResponse } from '../../@core/services/api/fundo-api.service';
import { MovimentacaoApiService, MovimentacaoResponse } from '../../@core/services/api/movimentacao-api.service';
import { FeatureFlagApiService } from '../../@core/services/api/feature-flag-api.service';
import { NotificationService } from '../../@core/services/notification.service';

@Component({
  selector: 'ngx-movimentacao',
  templateUrl: './movimentacao.component.html',
  styleUrls: ['./movimentacao.component.scss'],
  standalone: true,
  imports: [CommonModule, FormsModule, NbCardModule, NbSelectModule, NbInputModule, NbButtonModule, NbIconModule, NbCheckboxModule, AgGridModule],
})
export class MovimentacaoComponent implements OnInit {
  fundos: FundoResponse[] = [];
  selectedFundoCodigo: string = '';
  valor: number | null = null;
  cacheRedis: boolean = true;
  cacheRedisFeatureEnabled: boolean = false;
  movimentacoes: MovimentacaoResponse[] = [];

  columnDefs: ColDef[] = [
    { field: 'Id', headerName: 'Id', width: 80 },
    {
      field: 'DtMovimentacao', headerName: 'Data', flex: 1, sort: 'desc',
      valueFormatter: (p: any) => p.value ? new Date(p.value).toLocaleDateString('pt-BR') + ' ' + new Date(p.value).toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' }) : '',
    },
    {
      field: 'VlrMovimentacao', headerName: 'Valor (R$)', flex: 1,
      valueFormatter: (p: any) => p.value != null ? 'R$ ' + p.value.toLocaleString('pt-BR', { minimumFractionDigits: 2 }) : '',
      cellStyle: (p: any) => ({ color: p.value >= 0 ? '#28a745' : '#dc3545', fontWeight: 'bold' }),
    },
  ];

  defaultColDef: ColDef = { sortable: true, filter: true, resizable: true };

  getRowStyle = (params: any) => ({
    'background-color': params.node.rowIndex % 2 === 0 ? '#f0f0f0' : '#ffffff',
  });

  constructor(
    private fundoApi: FundoApiService,
    private movimentacaoApi: MovimentacaoApiService,
    private featureFlagApi: FeatureFlagApiService,
    private notification: NotificationService,
  ) {}

  ngOnInit(): void {
    this.fundoApi.getAll().subscribe(data => this.fundos = data);
    this.featureFlagApi.isEnabled('cache_redis').subscribe({
      next: (enabled) => this.cacheRedisFeatureEnabled = enabled,
      error: () => this.cacheRedisFeatureEnabled = false,
    });
  }

  onFundoChange(codigo: string): void {
    this.selectedFundoCodigo = codigo;
    this.movimentacoes = [];
  }

  consultar(): void {
    this.loadMovimentacoes();
  }

  movimentar(): void {
    if (!this.selectedFundoCodigo || !this.valor) return;

    this.movimentacaoApi.movimentar(this.selectedFundoCodigo, { Valor: this.valor }).subscribe({
      next: () => {
        this.notification.success('Movimentação realizada com sucesso.');
        this.valor = null;
        this.loadMovimentacoes();
      },
      error: (err) => {
        this.notification.error(err.error?.message || 'Erro ao movimentar.');
      },
    });
  }

  private loadMovimentacoes(): void {
    if (!this.selectedFundoCodigo) return;
    const useCache = this.cacheRedisFeatureEnabled && this.cacheRedis;
    this.movimentacaoApi.getMovimentacoes(this.selectedFundoCodigo, useCache).subscribe(data => {
      this.movimentacoes = data;
    });
  }
}
