import { Component, OnInit } from '@angular/core';
import { LobbyListHubService } from 'src/app/services/lobby-list-hub.service';
import { LobbyService } from 'src/app/services/lobby.service';
import { ILobbyItemViewModel } from 'src/shared/lobby.models';

@Component({
  selector: 'app-lobby-list',
  templateUrl: './lobby-list.component.html',
  styleUrls: ['./lobby-list.component.scss']
})
export class LobbyListComponent implements OnInit {

  constructor(
    private lobbyService: LobbyService,
    private lobbyListHubService: LobbyListHubService
  ) { }

  ngOnInit(): void {
    this.lobbyListHubService.startConnection();
  }

  public get lobbies(): ILobbyItemViewModel[] {
    return this.lobbyService.lobbies;
  }
}
