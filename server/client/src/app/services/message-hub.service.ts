import { Injectable, Injector } from '@angular/core';
import { HubService } from './abstract/hub.service';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MessageHubService extends HubService {
  protected override baseUrl: string = `${environment.baseUrl}/message-hubs/message-hub`;

  constructor(
    injector: Injector
  ) {
    super(injector);
  }

  protected override addListeners(): void {
    throw new Error('Method not implemented.');
  }
  protected override onHubConnected?(): void {
    throw new Error('Method not implemented.');
  }
  protected override onStartConnection?(): void {
    throw new Error('Method not implemented.');
  }
}
