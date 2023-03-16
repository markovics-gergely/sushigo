import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IHistoryViewModel } from 'src/shared/history.models';

@Injectable({
  providedIn: 'root'
})
export class HistoryService {
  /** Route of the history related endpoints */
  private readonly baseUrl: string = `${environment.baseUrl}/history`;

  constructor(private client: HttpClient) { }

  public get history(): Observable<IHistoryViewModel[]> {
    return this.client.get<IHistoryViewModel[]>(`${this.baseUrl}`);
  }
}
