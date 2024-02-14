import { Injectable, Injector } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { IMessageDTO, IMessageViewModel } from 'src/shared/models/message.models';
import { BaseServiceService } from '../../shared/services/abstract/base-service.service';

@Injectable({
  providedIn: 'root'
})
export class MessageService extends BaseServiceService {
  protected override readonly basePath = 'message';
  private _messageEventEmitter = new BehaviorSubject<IMessageViewModel | undefined>(undefined);

  constructor(injector: Injector) { super(injector); }

  public getMessages(lobbyId: string): Observable<IMessageViewModel[]> {
    return this.client.get<IMessageViewModel[]>(`${this.baseUrl}/${lobbyId}`);
  }

  public addMessage(message: IMessageDTO): Observable<IMessageViewModel> {
    return this.client.post<IMessageViewModel>(`${this.baseUrl}`, message);
  }

  public get messageEventEmitter(): BehaviorSubject<IMessageViewModel | undefined> {
    return this._messageEventEmitter;
  }
}
