import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IMessageDTO, IMessageViewModel } from 'src/shared/message.models';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  /** Route of the message related endpoints */
  private readonly baseUrl: string = `${environment.baseUrl}/message`;
  private _messageEventEmitter = new BehaviorSubject<IMessageViewModel | undefined>(undefined);

  constructor(private client: HttpClient) { }

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
