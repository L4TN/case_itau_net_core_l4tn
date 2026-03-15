import { Pipe, PipeTransform } from '@angular/core';
import { TranslationService } from '../services/translation.service';

@Pipe({
    name: 'translate',
    standalone: true,
})
export class TranslatePipe implements PipeTransform {

    constructor(private t: TranslationService) {}

    transform(key: string, params?: Record<string, any>): string {
        return this.t.get(key, params);
    }
}
