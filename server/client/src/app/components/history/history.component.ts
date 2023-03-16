import { Component, OnInit } from '@angular/core';
import { HistoryService } from 'src/app/services/history.service';
import { IHistoryViewModel } from 'src/shared/history.models';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.scss'],
})
export class HistoryComponent implements OnInit {
  private _history: IHistoryViewModel[] = [];

  constructor(private historyService: HistoryService) {}

  ngOnInit(): void {
    this.historyService.history.subscribe({
      next: (history: IHistoryViewModel[]) => {
        this._history = history;
      },
      error: (error: any) => {
        console.error(error);
      }
    });
  }

  public get history(): IHistoryViewModel[] {
    return this._history;
  }
}
