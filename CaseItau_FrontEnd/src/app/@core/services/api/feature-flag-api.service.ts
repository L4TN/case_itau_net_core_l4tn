import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

export interface FeatureFlagResponse {
  Id: number;
  Chave: string;
  Habilitado: boolean;
  Descricao: string;
  JsonConfig: string | null;
}

@Injectable({ providedIn: 'root' })
export class FeatureFlagApiService {
  private readonly url = `${environment.apiUrl}/featureflag`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<FeatureFlagResponse[]> {
    return this.http.get<FeatureFlagResponse[]>(this.url);
  }

  getByChave(chave: string): Observable<FeatureFlagResponse> {
    return this.http.get<FeatureFlagResponse>(`${this.url}/${chave}`);
  }

  isEnabled(chave: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.url}/${chave}/enabled`);
  }

  toggle(chave: string, habilitado: boolean): Observable<FeatureFlagResponse> {
    return this.http.put<FeatureFlagResponse>(`${this.url}/${chave}/toggle?habilitado=${habilitado}`, {});
  }
}
