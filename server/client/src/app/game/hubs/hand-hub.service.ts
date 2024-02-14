import { Injectable, Injector } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IHandViewModel } from 'src/shared/models/game.models';
import { HubService } from 'src/shared/services/abstract/hub.service';
import { HandService } from '../services/hand.service';

@Injectable({
  providedIn: 'root'
})
export class HandHubService extends HubService {
  protected override readonly baseUrl: string = `${environment.baseUrl}/game-hubs/hand-hub`;

  constructor(injector: Injector, private handService: HandService) {
    super(injector);
  }

  protected override addListeners(): void {
    this.hubConnection?.on('RefreshHand', (hand: IHandViewModel) => {
      this.handService.refreshHand(hand);
    });
  }

  protected override onHubConnected?(): void {
    this.handService.loadHand();
  }

  protected override onStartConnection?(): void {}
}
