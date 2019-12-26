import { Component, OnInit, OnDestroy } from '@angular/core';
import { LoginModel } from '../models/identity';
import { UserService } from '../services/user/user.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {

    model: LoginModel;
    errorMessage: string = '';
    componentDestroyed$: Subject<boolean> = new Subject()

    constructor(private userService: UserService) { }

    ngOnInit(): void {
        this.model = new LoginModel();
    }

    ngOnDestroy(): void {
        this.componentDestroyed$.next(true);
        this.componentDestroyed$.complete();
    }

    login() {
        this.userService.login(this.model)
            .pipe(takeUntil(this.componentDestroyed$))
            .subscribe(result => {
                console.log('login successful.')
            }, error => {
                this.errorMessage = error.error;
            });
    }
}
