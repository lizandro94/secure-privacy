import {
  HttpRequest,
  HttpResponse,
  HttpHandlerFn,
  HttpEvent,
} from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { delay, materialize, dematerialize } from 'rxjs/operators';

let users = [
  {
    id: 1,
    firstName: 'Jason',
    lastName: 'Watmore',
    username: 'test',
    password: 'test',
  },
];

export function fakeBackendProvider(
  request: HttpRequest<any>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  const { url, method, headers, body } = request;
  console.log('test');
  return handleRoute();

  function handleRoute() {
    console.log('test');
    switch (true) {
      case url.endsWith('/users/authenticate') && method === 'POST':
        return authenticate();
      default:
        // pass through any requests not handled above
        return next(request);
    }
  }

  // route functions

  function authenticate() {
    const { username, password } = body;
    const user = users.find(
      (x) => x.username === username && x.password === password
    );
    if (!user) return error('Username or password is incorrect');
    return ok({
      ...basicDetails(user),
      token: 'fake-jwt-token',
    });
  }

  // helper functions

  function ok(body?: any) {
    return of(new HttpResponse({ status: 200, body })).pipe(delay(500));
  }

  function error(message: string) {
    return throwError(() => ({ error: { message } })).pipe(
      materialize(),
      delay(500),
      dematerialize()
    );
  }

  function basicDetails(user: any) {
    const { id, username, firstName, lastName } = user;
    return { id, username, firstName, lastName };
  }
}
