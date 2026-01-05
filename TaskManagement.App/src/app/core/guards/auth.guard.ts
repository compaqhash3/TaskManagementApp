import { inject, PLATFORM_ID } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';
import { AppConstants } from '../constants/app.constants';

export const authGuard: CanActivateFn = () => {
  const platformId = inject(PLATFORM_ID);
  const router = inject(Router);

  // Prevent SSR crash
  if (!isPlatformBrowser(platformId)) {
    return false;
  }

  const isLoggedIn = !!localStorage.getItem(AppConstants.STORAGE_KEYS.AUTH_KEY);

  if (!isLoggedIn) {
    router.navigate(['/login']);
    return false;
  }

  return true;
};
