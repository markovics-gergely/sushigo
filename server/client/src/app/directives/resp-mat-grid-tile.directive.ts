import { Directive, Input, OnInit } from '@angular/core';
import { MatGridList, MatGridTile } from '@angular/material/grid-list';
import { MediaObserver, MediaChange } from '@angular/flex-layout';

export interface IResponsiveColumnsMap {
  xs?: number;
  sm?: number;
  md?: number;
  lg?: number;
  xl?: number;
}

@Directive({
  selector: '[colspan]',
})
export class RespMatGridTileDirective implements OnInit {
  private _colmap: IResponsiveColumnsMap = {};
  private _cols: number = 12;

  @Input()
  public set colspan(value: number) {
    this._cols = value;
  }

  @Input()
  public set xs(value: number) {
    this._colmap.xs = value;
  }

  @Input()
  public set sm(value: number) {
    this._colmap.sm = value;
  }

  @Input()
  public set md(value: number) {
    this._colmap.md = value;
  }

  @Input()
  public set lg(value: number) {
    this._colmap.lg = value;
  }

  @Input()
  public set xl(value: number) {
    this._colmap.xl = value;
  }

  constructor(private grid: MatGridTile, private media: MediaObserver) {}

  ngOnInit(): void {
    this.initializeColsCount();

    this.media.asObservable().subscribe((changes: MediaChange[]) => {
      this.grid.colspan =
        this._colmap[changes[0]?.mqAlias as keyof IResponsiveColumnsMap] ??
        this._cols;
    });
  }

  private initializeColsCount(): void {
    const colType = Object.keys(this._colmap).find(
      (mqAlias: string): boolean => {
        return this.media.isActive(mqAlias);
      }
    ) as keyof IResponsiveColumnsMap;

    this.grid.colspan = this._colmap[colType] ?? this._cols;
  }
}
