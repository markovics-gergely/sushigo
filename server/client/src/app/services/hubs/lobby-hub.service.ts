import { Injectable, Injector } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ActivatedRoute, Router } from '@angular/router';
import { LoadingService } from '../loading.service';
import { LobbyService } from '../lobby.service';
import { HubService } from './abstract/hub.service';
import { ILobbyViewModel, IPlayerViewModel } from 'src/shared/lobby.models';
import { IMessageViewModel } from 'src/shared/message.models';
import { MessageService } from '../message.service';

@Injectable({
  providedIn: 'root',
})
export class LobbyHubService extends HubService {
  protected override baseUrl: string = `${environment.baseUrl}/lobby-hubs/lobby-hub`;

  constructor(
    injector: Injector,
    private lobbyService: LobbyService,
    private route: ActivatedRoute,
    private router: Router,
    private messageService: MessageService
  ) {
    super(injector);
    route.params.subscribe((params) => {
      if (params['id']) {
        this.baseUrl = `${environment.baseUrl}/lobby-hubs/lobby-hub?lobby=${params['id']}`;
      }
    });
  }

  protected override addListeners(): void {
    this.hubConnection?.on('AddPlayer', (player: IPlayerViewModel) => { this.lobbyService.addPlayer(player); });
    this.hubConnection?.on('RemovePlayer', (playerId: string) => { this.lobbyService.removePlayer(playerId); });
    this.hubConnection?.on('PlayerReady', (lobby: ILobbyViewModel) => { this.lobbyService.lobbyEventEmitter.next({ ...lobby, event: 'ready' }); });
    this.hubConnection?.on('RemoveLobby', () => { this.router.navigate(['lobby']); });
    this.hubConnection?.on('AddMessage', (message: IMessageViewModel) => { this.messageService.messageEventEmitter.next(message); });
    this.hubConnection?.on('UpdateDeckType', (lobby: ILobbyViewModel) => { this.lobbyService.lobbyEventEmitter.next({ ...lobby, event: 'deckType' }); });
  }
  protected override onHubConnected?(): void {
    this.route.params.subscribe((params) => {
      if (params['id']) {
        this.lobbyService.loadLobby(params['id']);
      }
    });
  }
  protected override onStartConnection?(): void {}
}
