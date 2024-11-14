import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../../services/auth/auth.service';
import { inject } from '@angular/core';
import { catchError, EMPTY, switchMap } from 'rxjs';
import { addTokenToRequest } from '../auth/auth.interceptor';

export const refreshInterceptor: HttpInterceptorFn = (req, next) => {
  const authService: AuthService = inject(AuthService)

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && !req.url.includes('auth')) {
        const token = authService.getToken();
        if (token) {
          return authService.refreshToken(token).pipe(
            switchMap((response) => {
              return next(addTokenToRequest(req, response.accessToken));
            }),
            catchError((refreshError : HttpErrorResponse) => {
              if (refreshError.status === 401){
                authService.logOut();
                return EMPTY;
              }
              throw error;
            })
          );
        } else {
          authService.logOut();
         return EMPTY;
        }
      }
      throw error;
    })
  );
};
