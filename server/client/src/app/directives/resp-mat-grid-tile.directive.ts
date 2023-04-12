import { Directive, Input, OnInit } from '@angular/core';
import { MatGridTile } from '@angular/material/grid-list';
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
  private _rowmap: IResponsiveColumnsMap = {};
  private _cols: number = 12;
  private _rows: number = 1;

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

  @Input()
  public set rowspan(value: number) {
    this._rows = value;
  }
  @Input()
  public set xsr(value: number) {
    this._rowmap.xs = value;
  }
  @Input()
  public set smr(value: number) {
    this._rowmap.sm = value;
  }
  @Input()
  public set mdr(value: number) {
    this._rowmap.md = value;
  }
  @Input()
  public set lgr(value: number) {
    this._rowmap.lg = value;
  }
  @Input()
  public set xlr(value: number) {
    this._rowmap.xl = value;
  }

  constructor(private grid: MatGridTile, private media: MediaObserver) {}

  ngOnInit(): void {
    this.initializeColsCount();
    this.media.asObservable().subscribe((changes: MediaChange[]) => {
      this.grid.colspan = this.getActual(changes[0]?.mqAlias as keyof IResponsiveColumnsMap, this._colmap, this._cols);
      this.grid.rowspan = this.getActual(changes[0]?.mqAlias as keyof IResponsiveColumnsMap, this._rowmap, this._rows);
    });
  }

  private getActual(col: keyof IResponsiveColumnsMap, map: IResponsiveColumnsMap, defaultCol: number): number {
    const index = Object.keys(map).indexOf(col);
    let actual = defaultCol;
    Object.keys(map).slice(0, index + 1).forEach((key: string) => {
      actual = map[key as keyof IResponsiveColumnsMap] ?? actual;
    });
    return actual;
  }

  private initializeColsCount(): void {
    const colType = Object.keys(this._colmap).find(
      (mqAlias: string): boolean => {
        return this.media.isActive(mqAlias);
      }
    ) as keyof IResponsiveColumnsMap;
    
    this.grid.colspan = this.getActual(colType, this._colmap, this._cols);
    this.grid.rowspan = this.getActual(colType, this._rowmap, this._rows);
  }
}
