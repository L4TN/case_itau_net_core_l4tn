import { Injectable } from '@angular/core';
import { NbMenuItem } from '@nebular/theme';

@Injectable({ providedIn: 'root' })
export class MenuService {

    getMenuItems(): NbMenuItem[] {
        return [
            {
                title: 'Cadastro',
                icon: 'folder-outline',
                children: [
                    { title: 'Fundo', icon: 'briefcase-outline', link: '/pages/cadastro/fundo' },
                    { title: 'Tipo Fundo', icon: 'layers-outline', link: '/pages/cadastro/tipo-fundo' },
                ],
            },
            {
                title: 'Consulta',
                icon: 'search-outline',
                children: [
                    { title: 'Movimentação', icon: 'swap-outline', link: '/pages/consulta/movimentacao' },
                    { title: 'Posição', icon: 'bar-chart-outline', link: '/pages/consulta/posicao' },
                ],
            },
        ];
    }
}
