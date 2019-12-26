import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { throwError } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export abstract class BaseService {
    constructor() { }

    protected baseURL: string = 'https://localhost:5001';

    protected handleError(error: HttpErrorResponse) {
        if (error.error instanceof ErrorEvent) {
            console.error('An error occurred:', error.error.message);
        } else {
            console.error(
                `Backend returned code ${error.status}, ` +
                `body was: ${error.error} ` +
                `url was: ${error.url}`);
        }

        return throwError(error);
    };

    protected getDefaultHttpOptions() {
        return {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
            })
        };
    }
}
