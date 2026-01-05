import { HttpInterceptorFn } from '@angular/common/http';
import { AppConstants } from '../constants/app.constants';

export const basicAuthInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem(AppConstants.STORAGE_KEYS.AUTH_KEY);

  // Skip auth header for login
  if (req.url.includes(`/${AppConstants.API_ENDPOINTS.AUTH}/login`) || !token) {
    return next(req);
  }

  const authReq = req.clone({
    setHeaders: {
      Authorization: `Basic ${token}`
    }
  });

  return next(authReq);
};
