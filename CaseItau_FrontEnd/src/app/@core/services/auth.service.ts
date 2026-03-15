import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthService {

    private readonly TOKEN_KEY = 'jwt_token';

    constructor(private router: Router) {}

    isAuthenticated(): boolean {
        const token = localStorage.getItem(this.TOKEN_KEY);
        return !!token;
    }

    logout(): void {
        localStorage.removeItem(this.TOKEN_KEY);
        this.router.navigate(['/auth/login']);
    }
}
