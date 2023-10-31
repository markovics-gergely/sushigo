import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { Router } from '@angular/router';
import { SnackService } from '../services/snack.service';
import { TokenService } from '../services/token.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(
    private tokenService: TokenService,
    private snackService: SnackService,
  ) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const token = this.tokenService.token;
    const authReq = token ? request.clone({
      setHeaders: { Authorization: 'Bearer ' + token },
    }) : request;

    return next.handle(authReq)
      .pipe(
        catchError((err: HttpErrorResponse) => {
          if (err.status === 401 || err.status === 403) {
            this.tokenService.clearCookies();
          }
          const error = err.error?.title || this.snakeToText(err.error?.error_description) || err.message;
          this.snackService.openSnackBar(`${error} (${err.status})`, 'OK');
          throw err;
        })
      );
  }

  private snakeToText(snake: string): string | undefined {
    return snake?.split('_').map(s => s.charAt(0).toUpperCase() + s.slice(1)).join(' ');
  }
}
