import { Component, OnInit } from '@angular/core';
import { NbMenuItem } from '@nebular/theme';
import { MenuService } from '../@core/services/menu.service';

@Component({
  selector: 'ngx-pages',
  styleUrls: ['pages.component.scss'],
  template: `
    <ngx-one-column-layout>
      <nb-menu [items]="menu"></nb-menu>
      <router-outlet></router-outlet>
    </ngx-one-column-layout>
  `,
})
export class PagesComponent implements OnInit {

  menu: NbMenuItem[] = [];

  constructor(private menuService: MenuService) {}

  ngOnInit(): void {
    this.menu = this.menuService.getMenuItems();
  }
}
