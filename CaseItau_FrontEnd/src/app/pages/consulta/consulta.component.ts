import { Component, OnInit } from '@angular/core';
import { ColDef } from 'ag-grid-community';
import { EChartsOption } from 'echarts';
import { FundoApiService, FundoResponse } from '../../@core/services/api/fundo-api.service';
import { MovimentacaoApiService, PosicaoFundoResponse } from '../../@core/services/api/movimentacao-api.service';
import { FeatureFlagApiService } from '../../@core/services/api/feature-flag-api.service';

@Component({
  selector: 'ngx-consulta',
  templateUrl: './consulta.component.html',
  styleUrls: ['./consulta.component.scss'],
})
export class ConsultaComponent implements OnInit {
  fundos: FundoResponse[] = [];
  selectedFundoCodigo: string = '';
  selectedFundoNome: string = '';
  cacheRedis: boolean = true;
  cacheRedisFeatureEnabled: boolean = false;
  posicoes: PosicaoFundoResponse[] = [];
  chartOptions: EChartsOption = {};

  columnDefs: ColDef[] = [
    { field: 'Id', headerName: 'Id', width: 80 },
    {
      field: 'DtPosicao', headerName: 'Data', flex: 1, sort: 'desc',
      valueFormatter: (p: any) => p.value ? new Date(p.value).toLocaleDateString('pt-BR') : '',
    },
    {
      field: 'VlrPatrimonio', headerName: 'Patrimônio (R$)', flex: 1,
      valueFormatter: (p: any) => p.value != null ? 'R$ ' + p.value.toLocaleString('pt-BR', { minimumFractionDigits: 2 }) : '',
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
    const fundo = this.fundos.find(f => f.CdFundo === codigo);
    this.selectedFundoNome = fundo ? fundo.NmFundo : '';
    this.loadEvolucao();
  }

  private loadEvolucao(): void {
    const useCache = this.cacheRedisFeatureEnabled && this.cacheRedis;
    this.movimentacaoApi.getEvolucaoPatrimonial(this.selectedFundoCodigo, useCache).subscribe(data => {
      this.posicoes = data;
      this.buildChart(data);
    });
  }

  private buildChart(posicoes: PosicaoFundoResponse[]): void {
    const dates = posicoes.map(p => new Date(p.DtPosicao).toLocaleDateString('pt-BR'));
    const values = posicoes.map(p => p.VlrPatrimonio);

    this.chartOptions = {
      tooltip: {
        trigger: 'axis',
        formatter: (params: any) => {
          const p = params[0];
          return `${p.name}<br/>Patrimônio: R$ ${p.value.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}`;
        },
      },
      xAxis: {
        type: 'category',
        data: dates,
        axisLabel: { rotate: 45 },
      },
      yAxis: {
        type: 'value',
        axisLabel: {
          formatter: (val: number) => 'R$ ' + (val / 1000).toFixed(0) + 'k',
        },
      },
      series: [{
        name: 'Patrimônio',
        type: 'bar',
        data: values,
        itemStyle: {
          color: '#3366ff',
          borderRadius: [4, 4, 0, 0],
        },
        emphasis: {
          itemStyle: { color: '#598bff' },
        },
      }],
      grid: {
        left: '3%',
        right: '4%',
        bottom: '15%',
        containLabel: true,
      },
    };
  }
}
