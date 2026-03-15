import { Injectable } from '@angular/core';
import translations from '../../../assets/i18n/pt-BR.json';

@Injectable({ providedIn: 'root' })
export class TranslationService {

    private data: Record<string, any> = translations;

    /**
     * Get a translated string by dot-notation key.
     * Supports interpolation: t.get('dialog.deleteMultiple', { count: 5 })
     */
    get(key: string, params?: Record<string, any>): string {
        let value = this.resolve(key);
        if (params) {
            Object.keys(params).forEach(k => {
                value = value.replace(new RegExp(`\\{${k}\\}`, 'g'), String(params[k]));
            });
        }
        return value;
    }

    /**
     * Get an entire section as object (e.g., t.section('grid') for ag-grid localeText)
     */
    section(key: string): any {
        const parts = key.split('.');
        let current: any = this.data;
        for (const part of parts) {
            if (current && current[part] !== undefined) {
                current = current[part];
            } else {
                return {};
            }
        }
        return current;
    }

    private resolve(key: string): string {
        const parts = key.split('.');
        let current: any = this.data;
        for (const part of parts) {
            if (current && current[part] !== undefined) {
                current = current[part];
            } else {
                console.warn(`[i18n] Missing key: ${key}`);
                return key;
            }
        }
        return typeof current === 'string' ? current : key;
    }
}
