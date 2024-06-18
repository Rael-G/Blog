import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../../services/auth/auth.service';
import { inject } from '@angular/core';
import { catchError, switchMap } from 'rxjs';
import { addTokenToRequest } from '../auth/auth.interceptor';

export const refreshInterceptor: HttpInterceptorFn = (req, next) => {
  const authService: AuthService = inject(AuthService)

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && !req.url.includes('auth')) {
        const token = authService.getToken()
        if (token)
          return authService.refreshToken(token)
            .pipe(
              switchMap((response) => {
                return next(addTokenToRequest(req, response.accessToken))
              })
            )
        else
          authService.logOut()
      }
      throw error
    }))
};
