import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IHistoryViewModel } from 'src/shared/history.models';
import { BaseServiceService } from './abstract/base-service.service';

@Injectable({
  providedIn: 'root'
})
export class HistoryService extends BaseServiceService {
  protected override readonly basePath: string = 'user/history';

  public get history(): Observable<IHistoryViewModel[]> {
    return this.client.get<IHistoryViewModel[]>(`${this.baseUrl}?bypass=true`);
  }
}
