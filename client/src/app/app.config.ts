import { ApplicationConfig, importProvidersFrom, inject, provideAppInitializer, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core';
import { provideRouter, withViewTransitions } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { errorInterceptor } from '../core/interceptors/error.interceptor';
import { jwtInterceptor } from '../core/interceptors/jwt.interceptor';
// import { NgxSpinnerModule } from 'ngx-spinner';
// import { loadingInterceptor } from '../core/interceptors/loading.interceptor';
import { InitService } from '../core/services/init-service';
import { lastValueFrom } from 'rxjs';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(routes, withViewTransitions()),
    provideHttpClient(withInterceptors([ errorInterceptor, jwtInterceptor])),
    provideAppInitializer(async () => {
      const initService = inject(InitService);

      return new Promise<void>((resolve) => {
        setTimeout(async () => {
          try {
            return lastValueFrom(initService.init())
          } finally {
            const splash = document.getElementById('initial-splash');

            if (splash) {
              splash.remove();
            }

            resolve();
          }
        }, 500)
      })
    })
    // provideAnimations(),
    // importProvidersFrom(NgxSpinnerModule)
  ]
};
