import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

export interface TipoFundoResponse {
  Id: number;
  NmTipoFundo: string;
}

@Injectable({ providedIn: 'root' })
export class TipoFundoApiService {
  private readonly url = `${environment.apiUrl}/tipofundo`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<TipoFundoResponse[]> {
    return this.http.get<TipoFundoResponse[]>(this.url);
  }
}
