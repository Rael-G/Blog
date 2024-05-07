import { isPlatformBrowser } from "@angular/common";
import { PLATFORM_ID, inject } from "@angular/core";
import { HttpInterceptorFn } from "@angular/common/http";

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const platformId = inject(PLATFORM_ID);

  const token = isPlatformBrowser(platformId)? localStorage.getItem('accessToken')?? '' : ''
    req = req.clone({setHeaders: {
      Authorization : `Bearer ${token}`
    }})
  
  return next(req);
};