import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';

console.log('[main] Bootstrapping AppComponent...');
bootstrapApplication(AppComponent, appConfig)
  .then(() => console.log('[main] Bootstrap success'))
  .catch((err) => {
    console.error('[main] Bootstrap error:', err);
  });
