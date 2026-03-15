import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

export interface FundoResponse {
  Id: number;
  CdFundo: string;
  NmFundo: string;
  NrCnpj: string;
  IdTipoFundo: number;
  NmTipoFundo: string;
  VlrPatrimonio: number | null;
}

export interface CreateFundoRequest {
  Codigo: string;
  Nome: string;
  Cnpj: string;
  TipoFundoId: number;
}

export interface UpdateFundoRequest {
  Nome: string;
  Cnpj: string;
  TipoFundoId: number;
}

@Injectable({ providedIn: 'root' })
export class FundoApiService {
  private readonly url = `${environment.apiUrl}/fundo`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<FundoResponse[]> {
    return this.http.get<FundoResponse[]>(this.url);
  }

  getByCodigo(codigo: string): Observable<FundoResponse> {
    return this.http.get<FundoResponse>(`${this.url}/${codigo}`);
  }

  create(dto: CreateFundoRequest): Observable<FundoResponse> {
    return this.http.post<FundoResponse>(this.url, dto);
  }

  update(codigo: string, dto: UpdateFundoRequest): Observable<FundoResponse> {
    return this.http.put<FundoResponse>(`${this.url}/${codigo}`, dto);
  }

  delete(codigo: string): Observable<void> {
    return this.http.delete<void>(`${this.url}/${codigo}`);
  }
}
