import { Component, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { LoadingService } from 'src/app/services/loading.service';
import { LobbyService } from 'src/app/services/lobby.service';
import { ILobbyItemViewModel, ILobbyViewModel } from 'src/shared/lobby.models';

@Component({
  selector: 'app-lobby-list',
  templateUrl: './lobby-list.component.html',
  styleUrls: ['./lobby-list.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class LobbyListComponent {

  constructor(
    private lobbyService: LobbyService,
    private router: Router,
    private loadingService: LoadingService,
  ) { }

  public get lobbies(): ILobbyItemViewModel[] {
    return this.lobbyService.lobbies;
  }

  public create() {
    this.lobbyService.startLobbyCreate().subscribe({
      next: (lobby: ILobbyViewModel | undefined) => {
        if (!lobby) return;
        this.router.navigate([`/lobby/${lobby.id}`]);
      }
    });
  }

  public join(lobby: ILobbyItemViewModel) {
    this.loadingService.start();
    this.lobbyService.startJoinLobby(lobby).subscribe({
      next: (lobby: ILobbyViewModel | undefined) => {
        if (!lobby) return;
        this.router.navigate([`lobby/${lobby.id}`]);
      }
    }).add(() => {
      this.loadingService.stop();
    });
  }
}
