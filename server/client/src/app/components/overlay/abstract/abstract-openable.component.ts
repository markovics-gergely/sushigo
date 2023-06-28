export abstract class AbstractOpenableComponent {
    private _open: boolean = false;
    public get open(): boolean {
        return this._open;
    }
    public switchOpen(): void {
        this._open = !this._open;
    }

    protected getCloseStyle(items: Array<any>): { [klass: string]: any; } {
      return this._open ? { right: "0px" } : { right: `-${items.length * 50 + 2}px` };
    }
}