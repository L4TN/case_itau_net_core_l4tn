import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NbAuthModule } from '@nebular/auth';
import { NbSecurityModule, NbRoleProvider } from '@nebular/security';
import { of as observableOf } from 'rxjs';

import { throwIfAlreadyLoaded } from './module-import-guard';
import {
  AnalyticsService,
  LayoutService,
  SeoService,
} from './utils';

export class NbSimpleRoleProvider extends NbRoleProvider {
  getRole() {
    return observableOf('user');
  }
}

@NgModule({
  imports: [CommonModule],
  exports: [NbAuthModule],
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    throwIfAlreadyLoaded(parentModule, 'CoreModule');
  }

  static forRoot(): ModuleWithProviders<CoreModule> {
    return {
      ngModule: CoreModule,
      providers: [
        ...NbAuthModule.forRoot({
          strategies: [],
          forms: {},
        }).providers,
        ...NbSecurityModule.forRoot({
          accessControl: {
            user: {
              view: '*',
              create: '*',
              edit: '*',
              remove: '*',
            },
          },
        }).providers,
        { provide: NbRoleProvider, useClass: NbSimpleRoleProvider },
        AnalyticsService,
        LayoutService,
        SeoService,
      ],
    };
  }
}
