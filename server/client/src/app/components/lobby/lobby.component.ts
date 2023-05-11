import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LoadingService } from 'src/app/services/loading.service';
import { LobbyHubService } from 'src/app/services/hubs/lobby-hub.service';
import { LobbyService } from 'src/app/services/lobby.service';
import { ILobbyViewModel, IPlayerViewModel } from 'src/shared/lobby.models';
import { IDeckItemViewModel } from 'src/shared/deck.models';
import { ShopService } from 'src/app/services/shop.service';

@Component({
  selector: 'app-lobby',
  templateUrl: './lobby.component.html',
  styleUrls: ['./lobby.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class LobbyComponent {
  private _lobby: ILobbyViewModel | undefined;
  private _deck: IDeckItemViewModel | undefined;

  constructor(
    private route: ActivatedRoute,
    private loadingService: LoadingService,
    private lobbyService: LobbyService,
    private shopService: ShopService
  ) { }

  ngOnInit(): void {
    this.lobbyService.lobbyEventEmitter.subscribe({
      next: (lobby: ILobbyViewModel | undefined) => { this.processLobby(lobby); }
    });
    this.route.params.subscribe((params) => {
      if (params['id']) {
        this.lobbyService.loadLobby(params['id']);
      }
    });
  }

  public get lobby(): ILobbyViewModel | undefined {
    return this._lobby;
  }

  public get deck(): IDeckItemViewModel | undefined {
    return this._deck;
  }

  private processLobby(lobby: ILobbyViewModel | undefined) {
    console.log(lobby);
    
    this._lobby = lobby;
    if (lobby) {
      this.shopService.getDeck(lobby.deckType).subscribe((deck: IDeckItemViewModel) => {
        this._deck = deck;
      });
    } else {
      this._deck = undefined;
    }
  }

  public get creator(): IPlayerViewModel | undefined {
    return this._lobby?.players.find((p) => p.userId === this._lobby?.creatorUserId);
  }

  public get players(): IPlayerViewModel[] | undefined {
    return this._lobby?.players;
  }
}
