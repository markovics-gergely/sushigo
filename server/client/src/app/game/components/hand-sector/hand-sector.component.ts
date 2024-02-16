import { trigger, transition, style, animate } from '@angular/animations';
import { Component } from '@angular/core';
import { HandService } from '../../services/hand.service';
import { GameService } from '../../services/game.service';
import { GamePermissionService } from '../../services/game-permission.service';
import { IBoardViewModel, IHandViewModel, ISelectedCardInfo } from '../../models/game.models';
import { isEqual } from 'lodash';

@Component({
  selector: 'app-hand-sector',
  templateUrl: './hand-sector.component.html',
  styleUrls: ['./hand-sector.component.scss'],
  animations: [
    trigger('slideInOut', [
      transition(':enter', [
        style({ transform: 'translateY(-120%)' }),
        animate('200ms {{delay}}ms ease-in', style({ transform: 'translateY(0%)' })),
      ], { params: { delay: 0 } }),
      transition(':leave', [
        style({ transform: 'translateY(0%)' }),
        animate('200ms ease-out', style({ transform: 'translateY(120%)' })),
      ], { params: { delay: 0 } }),
    ]),
  ],
})
export class HandSectorComponent {
  protected hand: IHandViewModel | undefined;
  protected board: IBoardViewModel | undefined;

  protected showMode = ["hand"];

  constructor(private gameService: GameService, private handService: HandService, private gamePermissionService: GamePermissionService) {
    this.handService.handEventEmitter.subscribe((hand: IHandViewModel | undefined) => {
      if (!isEqual(this.hand, hand)) {
        this.hand = hand;
      }
    });
    this.handService.selectedCardEventEmitter.subscribe((info: ISelectedCardInfo | undefined) => {
      if (info?.switchTo) {
        this.showMode = [info.switchTo];
      }
    });
    this.gameService.gameEventEmitter.subscribe(() => {
      this.board = this.gameService.board;
    });
  }

  protected get anyInHand() {
    return this.gamePermissionService.anyCanPlayFromHand(this.hand?.cards ?? []);
  }

  protected get anyInBoard() {
    return this.gamePermissionService.anyCanPlayFromBoard(this.board?.cards ?? []);
  }

}
