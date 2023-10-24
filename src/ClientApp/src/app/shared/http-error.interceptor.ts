import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { ApiError } from './api-error.interface';

@Injectable()
  
  @Injectable()
  export class HttpErrorInterceptor implements HttpInterceptor {
  
    constructor() { }
  
    intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
      return next.handle(request)
        .pipe(
          catchError((errorResponse: HttpErrorResponse) => {
            const apiError: ApiError = {
              message: errorResponse.error.message,
              statusCode: errorResponse.status
            };

            console.error('HttpErrorInterceptor:',
              apiError.statusCode,
              apiError.message || '');
  
            return throwError(() => apiError);
          })
        );
    }
  }
  