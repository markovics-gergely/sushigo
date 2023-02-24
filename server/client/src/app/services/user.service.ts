import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ILoginUserDTO, IRegisterUserDTO } from 'src/shared/user.models';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  /** Route of the user related endpoints */
  private readonly baseUrl: string = `${environment.baseUrl}/user`;

  constructor(private client: HttpClient, private tokenService: TokenService) {}

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
  public login(loginUserDTO: ILoginUserDTO): Observable<Object> {
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

    return this.client.post(
      `${environment.baseUrl}/connect/token`,
      body.toString(),
      { headers: headers }
    );
  }

  /**
   * Refresh stored token with refresh token
   * @returns
   */
  public refresh(): Observable<Object> {
    let headers = new HttpHeaders().set(
      'Content-Type',
      'application/x-www-form-urlencoded'
    );
    let body = new URLSearchParams();

    const token = this.tokenService.refreshToken1;
    body.set('refresh_token', token);
    body.set('grant_type', 'refresh_token');
    body.set('client_id', environment.client_id);
    body.set('client_secret', environment.client_secret);

    return this.client.post(`${this.baseUrl}/refresh`, body.toString(), {
      headers: headers,
    });
  }
}
