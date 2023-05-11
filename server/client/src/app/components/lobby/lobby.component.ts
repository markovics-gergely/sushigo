import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LoadingService } from 'src/app/services/loading.service';
import { LobbyHubService } from 'src/app/services/hubs/lobby-hub.service';
import { LobbyService } from 'src/app/services/lobby.service';
import { ILobbyViewModel, IPlayerViewModel } from 'src/shared/lobby.models';
import { IDeckItemViewModel } from 'src/shared/deck.models';
import { ShopService } from 'src/app/services/shop.service';
import { TokenService } from 'src/app/services/token.service';

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
    private shopService: ShopService,
    private tokenService: TokenService
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
    this._lobby = lobby;
    if (lobby) {
      this.shopService.getDeck(lobby.deckType).subscribe((deck: IDeckItemViewModel) => {
        this._deck = deck;
      });
    } else {
      this._deck = undefined;
    }
  }

  public get creator(): string | undefined {
    return this._lobby?.creatorUserName;
  }

  public get players(): IPlayerViewModel[] | undefined {
    return this._lobby?.players;
  }

  public get own(): IPlayerViewModel | undefined {
    return this._lobby?.players.find((p) => p.userName === this.tokenService.user?.name);
  }

  public get isCreator(): boolean {
    return this._lobby?.creatorUserName === this.tokenService.user?.name;
  }

  public get validCount(): boolean {
    return Boolean(
      this._lobby &&
      this._deck &&
      this._lobby.players.length >= this._deck.minPlayer &&
      this._lobby.players.length <= this._deck.maxPlayer
    );
  }

  public leave(): void {
    if (!this.own || !this._lobby) return;
    this.lobbyService.leave({ playerId: this.own.id, lobbyId: this._lobby.id });
  }

  public removePlayer(player: IPlayerViewModel): void {
    this.lobbyService.removePlayer(player.id);
  }

  public ready(): void {
    if (!this.own) return;
    this.loadingService.start();
    this.lobbyService.ready({ playerId: this.own.id, ready: !this.own.ready }).subscribe({
      next: (lobby: ILobbyViewModel) => { this.processLobby(lobby); }
    }).add(() => {
      this.loadingService.stop();
    });
  }
}
