import { Injectable } from '@angular/core';
import { NbToastrService } from '@nebular/theme';
import { TranslationService } from './translation.service';

@Injectable({ providedIn: 'root' })
export class NotificationService {

    constructor(
        private toastrService: NbToastrService,
        private t: TranslationService,
    ) {}

    successCreate(): void {
        this.toastrService.success(this.t.get('notifications.recordCreated'), this.t.get('notifications.success'));
    }

    successUpdate(): void {
        this.toastrService.success(this.t.get('notifications.recordUpdated'), this.t.get('notifications.success'));
    }

    successDelete(count: number = 1): void {
        const msg = count === 1
            ? this.t.get('notifications.recordDeleted')
            : this.t.get('notifications.recordsDeleted');
        this.toastrService.success(msg, this.t.get('notifications.success'));
    }

    successExport(): void {
        this.toastrService.success(this.t.get('notifications.exportSuccess'), this.t.get('notifications.success'));
    }

    success(message: string): void {
        this.toastrService.success(message, this.t.get('notifications.success'));
    }

    error(message?: string): void {
        this.toastrService.danger(message || this.t.get('notifications.genericError'), this.t.get('notifications.error'));
    }

    warning(message: string): void {
        this.toastrService.warning(message, this.t.get('notifications.warning'));
    }

    info(message: string): void {
        this.toastrService.info(message, this.t.get('notifications.info'));
    }
}
