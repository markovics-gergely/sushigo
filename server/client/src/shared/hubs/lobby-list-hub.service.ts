import { Injectable, Injector } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ILobbyItemViewModel } from 'src/shared/models/lobby.models';
import { LobbyService } from '../../app/services/lobby.service';
import { HubService } from '../services/abstract/hub.service';

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
