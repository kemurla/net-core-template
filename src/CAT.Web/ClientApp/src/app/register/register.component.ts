import { Component, OnInit, OnDestroy } from '@angular/core';
import { UserService } from '../services/user/user.service';
import { RegisterModel } from '../models/identity';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { Router } from '@angular/router';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit, OnDestroy {
    model: RegisterModel;
    errorMessage: string = '';
    componentDestroyed$: Subject<boolean> = new Subject()

    constructor(private userService: UserService, private router: Router) { }

    ngOnInit(): void {
        this.model = new RegisterModel();
    }

    ngOnDestroy(): void {
        this.componentDestroyed$.next(true);
        this.componentDestroyed$.complete();
    }

    register() {
        this.userService.register(this.model)
            .pipe(takeUntil(this.componentDestroyed$))
            .subscribe(result => {
                this.router.navigate(['/login']);
            }, error => {
                this.errorMessage = error.error;
            });
    }
}
