import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ICreateLobbyDTO, ILobbyItemViewModel, ILobbyViewModel, IPlayerViewModel } from 'src/shared/lobby.models';

@Injectable({
  providedIn: 'root'
})
export class LobbyService {
  private readonly baseUrl: string = `${environment.baseUrl}/lobby`;
  private _lobbies: ILobbyItemViewModel[] = [];
  private _lobby: ILobbyViewModel | undefined;

  constructor(
    private client: HttpClient
  ) { }

  public loadLobbies(): Subscription {
    return this.client.get<ILobbyItemViewModel[]>(this.baseUrl).subscribe((lobbies: ILobbyItemViewModel[]) => {
      this._lobbies = lobbies;
    });
  }

  public loadLobby(lobbyId: string): Subscription {
    this._lobby = undefined;
    return this.client.get<ILobbyViewModel>(`${this.baseUrl}/${lobbyId}`).subscribe((lobby: ILobbyViewModel) => {
      this._lobby = lobby;
    });
  }

  public get lobby(): ILobbyViewModel | undefined {
    return this._lobby;
  }

  public get lobbies(): ILobbyItemViewModel[] {
    return this._lobbies;
  }

  public createLobby(lobby: ICreateLobbyDTO): Subscription {
    return this.client.post<ILobbyItemViewModel>(this.baseUrl, lobby).subscribe(this.addLobby);
  }

  public getLobby(lobbyId: string): Observable<ILobbyViewModel> {
    return this.client.get<ILobbyViewModel>(`${this.baseUrl}/${lobbyId}}`);
  }

  public addLobby(lobby: ILobbyItemViewModel): void {
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
}
