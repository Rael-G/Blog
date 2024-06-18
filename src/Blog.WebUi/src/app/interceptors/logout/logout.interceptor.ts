import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError } from 'rxjs';
import { AuthService } from '../../services/auth/auth.service';
import { inject } from '@angular/core';

export const logoutInterceptor: HttpInterceptorFn = (req, next) => {
  const authService: AuthService = inject(AuthService)

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && req.url.includes('regen-token')) {
        authService.logOut()
      }
      throw error
    }))
};
