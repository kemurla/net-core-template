import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RegisterModel, LoginModel } from '../../models/identity';
import { BaseService } from '../base.service';
import { catchError } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class UserService extends BaseService {
    constructor(private http: HttpClient) {
        super();
    }

    register(model: RegisterModel) {
        const body = JSON.stringify(model);
        const options = this.getDefaultHttpOptions();

        return this.http.post(`${this.baseURL}/account/register`, body, options)
            .pipe(catchError(this.handleError));
    }

    login(model: LoginModel) {
        const body = JSON.stringify(model);
        const options = this.getDefaultHttpOptions();

        return this.http.post(`${this.baseURL}/account/login`, body, options)
            .pipe(catchError(this.handleError));
    }
}
