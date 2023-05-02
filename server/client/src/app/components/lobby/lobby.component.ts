import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LoadingService } from 'src/app/services/loading.service';
import { LobbyHubService } from 'src/app/services/lobby-hub.service';
import { LobbyService } from 'src/app/services/lobby.service';
import { ILobbyViewModel } from 'src/shared/lobby.models';

@Component({
  selector: 'app-lobby',
  templateUrl: './lobby.component.html',
  styleUrls: ['./lobby.component.scss']
})
export class LobbyComponent {

  constructor(
    private route: ActivatedRoute,
    private loadingService: LoadingService,
    private lobbyService: LobbyService,
    private lobbyHubService: LobbyHubService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      if (params['id']) {
        this.loadingService.loading = true;
        this.lobbyService.loadLobby(params['id']).add(() => {
          this.loadingService.loading = false;
        });
      }
    });
  }

  public get lobby(): ILobbyViewModel | undefined {
    return this.lobbyService.lobby;
  }

}
