import { HttpHeaders } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import {
  IEditUserDTO,
  ILoginUserDTO,
  IRegisterUserDTO,
  IUser,
  IUserViewModel,
} from 'src/shared/models/user.models';
import { EditUserComponent } from '../components/dialog/edit-user/edit-user.component';
import { LoadingService } from '../../shared/services/loading.service';
import { BaseServiceService } from '../../shared/services/abstract/base-service.service';
import { Subscription } from 'cypress/types/net-stubbing';

@Injectable({
  providedIn: 'root',
})
export class UserService extends BaseServiceService {
  protected override readonly basePath = 'user';
  private _userEventEmitter = new BehaviorSubject<IUserViewModel | undefined>(undefined);

  constructor(
    injector: Injector,
    private dialog: MatDialog,
    private loadingService: LoadingService
  ) { super(injector); }

  /**
   * Send registration request to the server
   * @param registerUserDTO User to registrate
   * @returns
   */
  public registration(registerUserDTO: IRegisterUserDTO): Observable<Object> {
    return this.client.post(`${this.baseUrl}/registration`, registerUserDTO);
  }

  /**
   * Send login request to the server
   * @param loginUserDTO User to log in with
   * @returns Response from the server
   */
  public login(loginUserDTO: ILoginUserDTO): Observable<IUser> {
    let headers = new HttpHeaders().set(
      'Content-Type',
      'application/x-www-form-urlencoded'
    );
    let body = new URLSearchParams();

    body.set('username', loginUserDTO.username);
    body.set('password', loginUserDTO.password);
    body.set('grant_type', environment.grant_type);
    body.set('client_id', environment.client_id);
    body.set('client_secret', environment.client_secret);

    return this.client.post<IUser>(`${this.baseUrl}/login`, body.toString(), {
      headers: headers,
    });
  }

  public startEdit() {
    const dialogRef = this.dialog.open(EditUserComponent, {
      width: '40%',
      data: {
        userName: this._userEventEmitter.value?.userName,
        firstName: this._userEventEmitter.value?.name.split(" ")[0],
        lastName: this._userEventEmitter.value?.name.split(" ")[1],
        avatar: undefined,
      },
    });
    dialogRef.afterClosed().subscribe((result: IEditUserDTO | undefined) => {
      if (result) {
        this.loadingService.start();
        this.client
          .put<IUserViewModel>(`${this.baseUrl}/edit`, this.getFormData(result))
          .subscribe((user: IUserViewModel) => {
            this.loadingService.stop();
            this._userEventEmitter.next(user);
          });
      }
    });
  }

  public refreshUser() {
    return this.client.get<IUserViewModel>(`${this.baseUrl}`).subscribe((user: IUserViewModel) => {
      this._userEventEmitter.next(user);
    });
  }

  public deleteUser() {
    return this.client.delete<IUserViewModel | undefined>(`${this.baseUrl}`).subscribe(() => {
      this._userEventEmitter.next(undefined);
    });
  }

  public get userEventEmitter(): Observable<IUserViewModel | undefined> {
    return this._userEventEmitter;
  }
}
