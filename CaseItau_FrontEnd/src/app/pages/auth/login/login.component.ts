import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { NbAlertModule, NbInputModule, NbButtonModule, NbSpinnerModule } from '@nebular/theme';
import { environment } from '../../../../environments/environment';
import { AuthService } from '../../../@core/services/auth.service';

interface LoginResponse {
  Token: string;
  Expiration: string;
}

@Component({
  selector: 'ngx-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NbAlertModule,
    NbInputModule,
    NbButtonModule,
    NbSpinnerModule,
  ],
})
export class LoginComponent {

  usuario: string = '';
  senha: string = '';
  loading: boolean = false;
  error: string = '';

  constructor(
    private http: HttpClient,
    private router: Router,
    private authService: AuthService,
  ) {}

  login(): void {
    this.error = '';
    if (!this.usuario || !this.senha) {
      this.error = 'Usuário e senha são obrigatórios.';
      return;
    }

    this.loading = true;

    this.http.post<LoginResponse>(`${environment.apiUrl}/auth/login`, {
      Usuario: this.usuario,
      Senha: this.senha,
    }).subscribe({
      next: (res) => {
        this.authService.saveSession(res.Token, res.Expiration);
        this.router.navigate(['/pages/cadastro']);
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.Message || 'Credenciais inválidas.';
      },
    });
  }
}
