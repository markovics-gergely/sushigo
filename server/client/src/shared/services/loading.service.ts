import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  /** Flag to display loading screen */
  private _counter: number = 0;

  public start() {
    this._counter++;
  }

  public stop() {
    this._counter--;
  }

  public get load() {
    return this._counter > 0;
  }
}
