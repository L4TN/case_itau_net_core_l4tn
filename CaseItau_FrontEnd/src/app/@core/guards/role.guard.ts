import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router, UrlTree } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { UserProfile } from '../models/user-profile.type';

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {

    constructor(
        private authService: AuthService,
        private router: Router,
    ) {}

    canActivate(route: ActivatedRouteSnapshot): boolean | UrlTree {
        const user = this.authService.getCurrentUser();
        if (!user) {
            return this.router.createUrlTree(['/auth/login']);
        }

        const allowedRoles: UserProfile[] = route.data['roles'] || [];
        if (allowedRoles.length === 0 || allowedRoles.includes(user.profile as UserProfile)) {
            return true;
        }

        if (user.profile === 'interno') {
            return this.router.createUrlTree(['/pages/admin/dashboard']);
        }
        return this.router.createUrlTree(['/pages/client/dashboard']);
    }
}
