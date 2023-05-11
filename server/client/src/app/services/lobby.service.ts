import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject, Observable, Subscription, of, switchMap } from 'rxjs';
import { environment } from 'src/environments/environment';
import {
  ICreateLobbyDTO,
  IJoinLobbyDTO,
  ILobbyItemViewModel,
  ILobbyViewModel,
  IPlayerViewModel,
} from 'src/shared/lobby.models';
import { CreateLobbyComponent } from '../components/dialog/create-lobby/create-lobby.component';
import { JoinLobbyComponent } from '../components/dialog/join-lobby/join-lobby.component';
import { TokenService } from './token.service';
import { LoadingService } from './loading.service';

@Injectable({
  providedIn: 'root',
})
export class LobbyService {
  private readonly baseUrl: string = `${environment.baseUrl}/lobby`;
  private _lobbies: ILobbyItemViewModel[] = [];
  private _lobby: ILobbyViewModel | undefined;
  private _lobbyEventEmitter = new BehaviorSubject<ILobbyViewModel | undefined>(undefined);

  constructor(
    private client: HttpClient,
    private dialog: MatDialog,
    private loadingService: LoadingService
    ) {}

  public loadLobbies(): Subscription {
    return this.client
      .get<ILobbyItemViewModel[]>(this.baseUrl)
      .subscribe((lobbies: ILobbyItemViewModel[]) => {
        this._lobbies = lobbies;
      });
  }

  public loadLobby(lobbyId: string): void {
    this.loadingService.loading = true;
    this._lobbyEventEmitter.next(undefined);
    this.client
      .get<ILobbyViewModel>(`${this.baseUrl}/${lobbyId}`)
      .subscribe((lobby: ILobbyViewModel) => {
        this._lobbyEventEmitter.next(lobby);
      }).add(() => {
        this.loadingService.loading = false;
      });
  }

  public get lobbyEventEmitter(): BehaviorSubject<ILobbyViewModel | undefined> {
    return this._lobbyEventEmitter;
  }

  public get lobbies(): ILobbyItemViewModel[] {
    return this._lobbies;
  }

  public createLobby(lobby: ICreateLobbyDTO): Subscription {
    return this.client
      .post<ILobbyItemViewModel>(this.baseUrl, lobby)
      .subscribe(this.addLobby);
  }

  public getLobby(lobbyId: string): Observable<ILobbyViewModel> {
    return this.client.get<ILobbyViewModel>(`${this.baseUrl}/${lobbyId}}`);
  }

  public joinLobby(joinLobby: IJoinLobbyDTO): Observable<ILobbyViewModel> {
    return this.client.post<ILobbyViewModel>(`${this.baseUrl}/join`, joinLobby);
  } 

  public addLobby(lobby: ILobbyItemViewModel): void {
    console.log(lobby);
    
    if (this._lobbies.some((l) => l.id === lobby.id)) {
      return;
    }
    this._lobbies.unshift(lobby);
  }

  public removeLobby(lobbyId: string): void {
    this._lobbies = this._lobbies.filter((l) => l.id !== lobbyId);
  }

  public addPlayer(player: IPlayerViewModel) {
    this._lobby?.players.push(player);
  }

  public removePlayer(playerId: string) {
    if (!this._lobby) return;
    this._lobby.players = this._lobby.players.filter((p) => p.id !== playerId);
  }

  public startLobbyCreate(): Observable<ILobbyViewModel | undefined> {
    const dialogRef = this.dialog.open(CreateLobbyComponent, {
      width: '40%',
      data: {},
    });
    return dialogRef.afterClosed().pipe(
      switchMap((result: ICreateLobbyDTO | undefined) => {
        if (result) {
          return this.client.post<ILobbyViewModel>(this.baseUrl, result);
        } else return of(undefined);
      })
    );
  }

  public startJoinLobby(lobby: ILobbyItemViewModel): Observable<ILobbyViewModel | undefined> {
    const dialogRef = this.dialog.open(JoinLobbyComponent, {
      width: '40%',
      data: lobby,
    });
    return dialogRef.afterClosed().pipe(
      switchMap((result: IJoinLobbyDTO | undefined) => {
        if (result) {
          return this.joinLobby(result);
        } else return of(undefined);
      }
    ));
  }
}
