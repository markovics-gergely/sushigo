import { Injectable, Injector } from '@angular/core';
import { Observable, of } from 'rxjs';
import { IHistoryViewModel } from 'src/shared/history.models';
import { BaseServiceService } from './abstract/base-service.service';

@Injectable({
  providedIn: 'root'
})
export class HistoryService extends BaseServiceService {
  protected override readonly basePath: string = 'history';

  constructor(injector: Injector) { super(injector); }

  public get history(): Observable<IHistoryViewModel[]> {
    return of([]);
    //return this.client.get<IHistoryViewModel[]>(`${this.baseUrl}`);
  }
}
