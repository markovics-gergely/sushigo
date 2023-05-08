import { Injectable, Injector } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HubService } from './abstract/hub.service';
import { LobbyService } from './lobby.service';
import { ILobbyItemViewModel } from 'src/shared/lobby.models';

@Injectable({
  providedIn: 'root',
})
export class LobbyListHubService extends HubService {
  protected override readonly baseUrl: string = `${environment.baseUrl}/lobby-hubs/lobby-list-hub`;

  constructor(injector: Injector, private lobbyService: LobbyService) {
    super(injector);
  }

  protected override addListeners(): void {
    this.hubConnection?.on('AddLobby', (lobby: ILobbyItemViewModel) => {
      this.lobbyService.addLobby(lobby);
    });
    this.hubConnection?.on('RemoveLobby', (lobbyId: string) => {
      this.lobbyService.removeLobby(lobbyId);
    });
  }

  protected override onHubConnected(): void {
    this.lobbyService.loadLobbies().add(() => {
      this.connected = true;
      this.connecting = false;
    });
  }
  protected override onStartConnection?(): void {}
}
