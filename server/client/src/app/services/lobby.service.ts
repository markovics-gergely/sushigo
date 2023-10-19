import { HttpHeaders } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject, Observable, Subscription, of, switchMap } from 'rxjs';
import {
  ICreateLobbyDTO,
  IJoinLobbyDTO,
  ILobbyItemViewModel,
  ILobbyViewModel,
  IPlayerReadyDTO,
  IPlayerViewModel,
  IRemovePlayerDTO,
} from 'src/shared/lobby.models';
import { CreateLobbyComponent } from '../components/dialog/create-lobby/create-lobby.component';
import { JoinLobbyComponent } from '../components/dialog/join-lobby/join-lobby.component';
import { LoadingService } from './loading.service';
import { ConfirmService } from 'src/app/services/confirm.service';
import { Router } from '@angular/router';
import { DeckType } from 'src/shared/deck.models';
import { EditLobbyComponent } from '../components/dialog/edit-lobby/edit-lobby.component';
import { BaseServiceService } from './abstract/base-service.service';

@Injectable({
  providedIn: 'root',
})
export class LobbyService extends BaseServiceService {
  protected override readonly basePath: string = 'lobby';
  private _lobbiesEventEmitter = new BehaviorSubject<ILobbyItemViewModel[]>([]);
  private _lobbyEventEmitter = new BehaviorSubject<ILobbyViewModel | undefined>(undefined);

  constructor(
    injector: Injector,
    private dialog: MatDialog,
    private loadingService: LoadingService,
    private confirmService: ConfirmService,
    private router: Router
    ) { super(injector); }

  public loadLobbies(): Subscription {
    this._lobbiesEventEmitter.next([]);
    return this.client
      .get<ILobbyItemViewModel[]>(this.baseUrl)
      .subscribe((lobbies: ILobbyItemViewModel[]) => {
        this._lobbiesEventEmitter.next(lobbies ?? []);
      });
  }

  public loadLobby(lobbyId: string): void {
    this.loadingService.start();
    this._lobbyEventEmitter.next(undefined);
    this.client
      .get<ILobbyViewModel>(`${this.baseUrl}/${lobbyId}`)
      .subscribe((lobby: ILobbyViewModel) => {
        this._lobbyEventEmitter.next(lobby);
      }).add(() => {
        this.loadingService.stop();
      });
  }

  public get lobbyEventEmitter(): BehaviorSubject<ILobbyViewModel | undefined> {
    return this._lobbyEventEmitter;
  }

  public get lobbiesEventEmitter(): Observable<ILobbyItemViewModel[]> {
    return this._lobbiesEventEmitter;
  }

  public createLobby(lobby: ICreateLobbyDTO): Subscription {
    return this.client
      .post<ILobbyItemViewModel>(`${this.baseUrl}/create`, lobby)
      .subscribe(this.addLobby);
  }

  public getLobby(lobbyId: string): Observable<ILobbyViewModel> {
    return this.client.get<ILobbyViewModel>(`${this.baseUrl}/${lobbyId}`);
  }

  public joinLobby(joinLobby: IJoinLobbyDTO): Observable<ILobbyViewModel> {
    return this.client.post<ILobbyViewModel>(`${this.baseUrl}/join`, joinLobby);
  }

  public ready(dto: IPlayerReadyDTO): Observable<ILobbyViewModel> {
    return this.client.put<ILobbyViewModel>(`${this.baseUrl}/player/ready`, dto);
  }

  public leaveLobby(dto: IRemovePlayerDTO): Observable<ILobbyViewModel | undefined> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }), body: dto };
    return this.client.delete<ILobbyViewModel | undefined>(`${this.baseUrl}/player`, options);
  }

  public leave(dto: IRemovePlayerDTO, own: boolean) {
    this.confirmService
      .confirm(`lobby.${own ? 'delete' : 'leave'}`, '250px')
      .subscribe((result: boolean) => {
        if (result) {
          this.loadingService.start();
          this.leaveLobby(dto).subscribe(() => {
            this.router.navigate(['lobby']).catch(console.error);
          }).add(() => {
            this.loadingService.stop();
          });
        }
      });
  }

  public remove(dto: IRemovePlayerDTO) {
    this.confirmService
      .confirm('lobby.remove', '250px')
      .subscribe((result: boolean) => {
        if (result) {
          this.loadingService.start();
          this.leaveLobby(dto).subscribe((lobby: ILobbyViewModel | undefined) => {
            if (lobby) {
              this._lobbyEventEmitter.next(lobby);
            }
          }).add(() => {
            this.loadingService.stop();
          });
        }
      });
  }

  public addLobby(lobby: ILobbyItemViewModel): void {
    const lobbies = this._lobbiesEventEmitter.value;
    if (lobbies.some((l) => l.id === lobby.id)) {
      return;
    }
    lobbies.unshift(lobby);
    this._lobbiesEventEmitter.next(lobbies);
  }

  public removeLobby(lobbyId: string): void {
    const lobbies = this._lobbiesEventEmitter.value;
    this._lobbiesEventEmitter.next(lobbies.filter((l) => l.id !== lobbyId));
  }

  public addPlayer(player: IPlayerViewModel) {
    const lobby = this._lobbyEventEmitter.value;
    if (lobby?.players.every((p) => p.id !== player.id)) {
      lobby.players.push(player);
      lobby.event = 'addPlayer';
      this._lobbyEventEmitter.next(lobby);
    }
  }

  public removePlayer(playerId: string) {
    const lobby = this._lobbyEventEmitter.value;
    if (!lobby) return;
    lobby.players = lobby.players.filter((p) => p.id !== playerId);
    lobby.event = 'removePlayer';
    this._lobbyEventEmitter.next(lobby);
  }

  public startLobbyCreate(): Observable<ILobbyViewModel | undefined> {
    const dialogRef = this.dialog.open(CreateLobbyComponent, {
      width: '40%',
      data: {},
    });
    return dialogRef.afterClosed().pipe(
      switchMap((result: ICreateLobbyDTO | undefined) => {
        if (result) {
          return this.client.post<ILobbyViewModel>(`${this.baseUrl}/create`, result);
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
          return this.joinLobby({ ...result, id: lobby.id });
        } else return of(undefined);
      }
    ));
  }

  public startEditLobbyDeck(deck: DeckType, lobbyId: string): Observable<ILobbyViewModel | undefined> {
    const dialogRef = this.dialog.open(EditLobbyComponent, {
      width: '40%',
      data: deck,
    });
    return dialogRef.afterClosed().pipe(
      switchMap((result: { deckType: DeckType}) => {
        if (result) {
          return this.client.post<ILobbyViewModel>(`${this.baseUrl}/deck`, { deckType: result.deckType, lobbyId });
        } else return of(undefined);
      }
    ));
  }
}
