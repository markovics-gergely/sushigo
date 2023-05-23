import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable, of, switchMap } from 'rxjs';
import { environment } from 'src/environments/environment';
import {
  IEditUserDTO,
  ILoginUserDTO,
  IRegisterUserDTO,
  IUser,
  IUserViewModel,
} from 'src/shared/user.models';
import { EditUserComponent } from '../components/dialog/edit-user/edit-user.component';
import { LoadingService } from './loading.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  /** Route of the user related endpoints */
  private readonly baseUrl: string = `${environment.baseUrl}/user`;

  constructor(
    private client: HttpClient,
    private dialog: MatDialog,
    private loadingService: LoadingService
  ) { }

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

  public startEdit(dto: IEditUserDTO): Observable<IUserViewModel | undefined> {
    const dialogRef = this.dialog.open(EditUserComponent, {
      width: '40%',
      data: dto,
    });
    return dialogRef.afterClosed().pipe(
      switchMap((result: IEditUserDTO | undefined) => {
        if (result) {
          this.loadingService.start();
          return this.client.put<IUserViewModel>(
            `${this.baseUrl}/edit`,
            this.getFormData(result)
          ).pipe(
            switchMap((user: IUserViewModel) => {
              this.loadingService.stop();
              return of(user);
            })
          );
        } else return of(undefined);
      })
    );
  }

  public get user(): Observable<IUserViewModel> {
    return this.client.get<IUserViewModel>(`${this.baseUrl}`);
  }

  /**
   * Generate form data from object
   * @param obj Object to transform
   * @returns FormData generated
   */
  private getFormData(obj: any): FormData {
    return Object.keys(obj).reduce((formData, key) => {
      formData.append(key, obj[key]);
      return formData;
    }, new FormData());
  }
}
