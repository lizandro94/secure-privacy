import {
  HttpRequest,
  HttpResponse,
  HttpHandlerFn,
  HttpEvent,
} from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { delay, materialize, dematerialize } from 'rxjs/operators';

export function fakeBackendProvider(
  request: HttpRequest<any>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  const { url, method, body } = request;

  // array in local storage for registered users
  const usersKey = 'angular-tutorial-users';
  let users: any[] = JSON.parse(localStorage.getItem(usersKey)!) || [];
  return handleRoute();

  function handleRoute() {
    switch (true) {
      case url.endsWith('/users/authenticate') && method === 'POST':
        return authenticate();
      case url.endsWith('/users/register') && method === 'POST':
        return register();
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

  function register() {
    const user = body;

    if (users.find((x) => x.username === user.username)) {
      return error('Username "' + user.username + '" is already taken');
    }

    user.id = users.length ? Math.max(...users.map((x) => x.id)) + 1 : 1;
    users.push(user);
    localStorage.setItem(usersKey, JSON.stringify(users));
    return ok();
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
