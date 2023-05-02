import { Injectable, Injector } from '@angular/core';
import { HubService } from './abstract/hub.service';
import { environment } from 'src/environments/environment';
import { LobbyService } from './lobby.service';
import { ActivatedRoute } from '@angular/router';
import { LoadingService } from './loading.service';

@Injectable({
  providedIn: 'root',
})
export class LobbyHubService extends HubService {
  protected override baseUrl: string = `${environment.baseUrl}/lobby-hubs/lobby-hub`;

  constructor(
    injector: Injector,
    private lobbyService: LobbyService,
    private route: ActivatedRoute,
    private loadingService: LoadingService
  ) {
    super(injector);
  }

  protected override addListeners(): void {
    this.hubConnection?.on('AddPlayer', this.lobbyService.addPlayer);
    this.hubConnection?.on('RemovePlayer', this.lobbyService.removePlayer);
  }
  protected override onHubConnected?(): void {}
  protected override onStartConnection?(): void {}
}
