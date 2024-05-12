import { inject } from "@angular/core";
import { HttpErrorResponse, HttpInterceptorFn } from "@angular/common/http";
import { catchError, switchMap } from "rxjs";
import { AuthService } from "../../services/auth/auth.service";

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService : AuthService = inject(AuthService)
  let accessToken = authService.getToken()?.accessToken?? '';

  req = req.clone({setHeaders: {
    Authorization : `Bearer ${accessToken}`
  }})

  return next(req).pipe(
    catchError((error : HttpErrorResponse) => {
    if (error.status === 401 && !req.url.includes('auth')){
        const token = authService.getToken()
        if (token)
          return authService.refreshToken(token)
            .pipe(
              switchMap((response) => {
                req = req.clone({setHeaders: {
                  Authorization : `Bearer ${response.accessToken}`
                }})
                return next(req);
              })
            )
        else
          authService.logOut()
      }
     throw error
  }))
}