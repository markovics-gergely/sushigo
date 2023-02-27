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
    private router: Router
  ) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const token = this.tokenService.token;
    const authReq = request.clone({
      setHeaders: { Authorization: 'Bearer ' + token },
    });
    return next.handle(authReq)
      .pipe(
        catchError((err: HttpErrorResponse) => {
          if (err.status === 401) {
            this.snackService.openSnackBar(err.error, 'OK');
            this.tokenService.clearCookies();
            this.router.navigate(['login']);
          }
          else if (err.status === 403) {
            this.snackService.openSnackBar(err.error, 'OK');
            this.tokenService.clearCookies();
          }
          else if (err.status === 404) {
            this.snackService.openSnackBar(err.error, 'OK');
          }
          throw err;
        })
      );
  }
}
