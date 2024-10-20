import { inject } from '@angular/core';
import { HttpRequest, HttpHandlerFn, HttpEvent } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { AccountService } from '@app/_services';

export function errorInterceptor(
  request: HttpRequest<any>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  console.log('test22');
  const accountService = inject(AccountService);
  return next(request).pipe(
    catchError((err) => {
      if ([401, 403].includes(err.status) && accountService.userValue) {
        // auto logout if 401 or 403 response returned from api
        accountService.logout();
      }

      const error = err.error?.message || err.statusText;
      console.error(err);
      return throwError(() => error);
    })
  );
}
