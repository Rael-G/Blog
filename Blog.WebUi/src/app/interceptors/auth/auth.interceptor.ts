import { inject } from "@angular/core";
import { HttpInterceptorFn, HttpRequest } from "@angular/common/http";
import { AuthService } from "../../services/auth/auth.service";

export const addTokenToRequest = (req: HttpRequest<any>, token: string): HttpRequest<any> => {
  return req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`,
    },
    withCredentials: true
  });
};

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService: AuthService = inject(AuthService)
  let accessToken = authService.getToken()?.accessToken ?? '';

  return next(addTokenToRequest(req, accessToken));
}