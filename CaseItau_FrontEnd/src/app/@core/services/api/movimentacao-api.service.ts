import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

export interface PosicaoFundoResponse {
  Id: number;
  IdFundo: number;
  DtPosicao: string;
  VlrPatrimonio: number;
}

export interface MovimentacaoResponse {
  Id: number;
  IdFundo: number;
  DtMovimentacao: string;
  VlrMovimentacao: number;
}

export interface CreateMovimentacaoRequest {
  Valor: number;
}

@Injectable({ providedIn: 'root' })
export class MovimentacaoApiService {
  private readonly url = `${environment.apiUrl}/movimentacao`;

  constructor(private http: HttpClient) {}

  movimentar(codigoFundo: string, dto: CreateMovimentacaoRequest): Observable<PosicaoFundoResponse> {
    return this.http.post<PosicaoFundoResponse>(`${this.url}/${codigoFundo}`, dto);
  }

  getEvolucaoPatrimonial(codigoFundo: string, cacheRedis: boolean = false): Observable<PosicaoFundoResponse[]> {
    const params = new HttpParams().set('cacheRedis', cacheRedis);
    return this.http.get<PosicaoFundoResponse[]>(`${this.url}/${codigoFundo}/evolucao-patrimonial`, { params });
  }

  getMovimentacoes(codigoFundo: string, cacheRedis: boolean = false): Observable<MovimentacaoResponse[]> {
    const params = new HttpParams().set('cacheRedis', cacheRedis);
    return this.http.get<MovimentacaoResponse[]>(`${this.url}/${codigoFundo}`, { params });
  }
}
