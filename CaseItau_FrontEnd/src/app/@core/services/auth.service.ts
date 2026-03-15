import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthService {

    private readonly TOKEN_KEY = 'jwt_token';
    private readonly EXPIRATION_KEY = 'jwt_expiration';

    constructor(private router: Router) {}

    saveSession(token: string, expiration: string): void {
        localStorage.setItem(this.TOKEN_KEY, token);
        localStorage.setItem(this.EXPIRATION_KEY, expiration);
    }

    isAuthenticated(): boolean {
        const token = localStorage.getItem(this.TOKEN_KEY);
        const expiration = localStorage.getItem(this.EXPIRATION_KEY);

        if (!token || !expiration) {
            return false;
        }

        const expirationDate = new Date(expiration);
        if (expirationDate <= new Date()) {
            this.logout();
            return false;
        }

        return true;
    }

    logout(): void {
        localStorage.removeItem(this.TOKEN_KEY);
        localStorage.removeItem(this.EXPIRATION_KEY);
        this.router.navigate(['/auth/login']);
    }
}
