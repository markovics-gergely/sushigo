import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LoadingService } from 'src/app/services/loading.service';
import { LobbyService } from 'src/app/services/lobby.service';
import { ILobbyViewModel, IPlayerViewModel } from 'src/shared/lobby.models';
import { IDeckItemViewModel } from 'src/shared/deck.models';
import { ShopService } from 'src/app/services/shop.service';
import { TokenService } from 'src/app/services/token.service';
import { GameService } from 'src/app/services/game.service';
import { isEqual } from 'lodash';

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
    private tokenService: TokenService,
    private gameService: GameService
  ) {}

  ngOnInit(): void {
    this.lobbyService.lobbyEventEmitter.subscribe({
      next: (lobby: ILobbyViewModel | undefined) => {
        this.processLobby(lobby);
      },
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
    if (lobby?.event === 'ready') {
      this._lobby?.players.forEach((p, i) => {
        p.ready = lobby.players[i].ready;
      });
      lobby.event = undefined;
      return;
    }
    if (lobby?.event === 'deckType') {
      if (!this._lobby) return;
      this._lobby?.players.forEach((p, i) => {
        p.ready = lobby.players[i].ready;
      });
      this._lobby.deckType = lobby.deckType;
      this.shopService
        .getDeck(lobby.deckType)
        .subscribe((deck: IDeckItemViewModel) => {
          this._deck = deck;
          this._deck.imageLoaded = true;
        });
      lobby.event = undefined;
      return;
    }
    if (isEqual(lobby, this._lobby)) return;
    this._lobby = lobby;
    if (lobby) {
      this.shopService
        .getDeck(lobby.deckType)
        .subscribe((deck: IDeckItemViewModel) => {
          this._deck = deck;
          this._deck.imageLoaded = true;
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
    return this._lobby?.players.find(
      (p) => p.userName === this.tokenService.user?.name
    );
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

  public get valid(): boolean {
    return Boolean(
      this.validCount && this._lobby?.players.every((p) => p.ready)
    );
  }

  public leave(): void {
    if (!this.own || !this._lobby) return;
    this.lobbyService.leave(
      { playerId: this.own.id, lobbyId: this._lobby.id },
      this.isCreator
    );
  }

  public remove(player: IPlayerViewModel) {
    if (
      !this._lobby ||
      !this.isCreator ||
      this._lobby.creatorUserName === player.userName
    )
      return;
    this.lobbyService.remove({ playerId: player.id, lobbyId: this._lobby.id });
  }

  public removePlayer(player: IPlayerViewModel): void {
    this.lobbyService.removePlayer(player.id);
  }

  protected readyProcess: boolean = false;
  public ready(): void {
    if (!this.own) return;
    this.readyProcess = true;
    this.lobbyService
      .ready({ playerId: this.own.id, ready: !this.own.ready })
      .subscribe({})
      .add(() => {
        this.readyProcess = false;
      });
  }

  public edit(): void {
    if (!this._lobby) return;
    this.lobbyService
      .startEditLobbyDeck(this._lobby.deckType, this._lobby.id)
      .subscribe();
  }

  public start(): void {
    if (!this._lobby) return;
    this.loadingService.start();
    this.gameService.createGame({
      name: this._lobby.name,
      deckType: this._lobby.deckType,
      players: this._lobby.players.map((p) => ({
        userId: p.userId,
        userName: p.userName,
        imagePath: p.imagePath,
      })),
    }).subscribe({}).add(() => {
      this.loadingService.stop();
    });
  }

  protected get timezone(): string {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
  }
}
