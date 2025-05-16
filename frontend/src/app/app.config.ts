import { APP_INITIALIZER, ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { HttpClient, provideHttpClient } from '@angular/common/http';
import { InitService } from './core/services/init.service';
import { lastValueFrom } from 'rxjs';

function initApp(initService: InitService){
  return () => lastValueFrom(initService.init());
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(),
    {
      provide: APP_INITIALIZER,
      useFactory: initApp,
      multi: true,
      deps: [InitService]
    }
  ]
};
