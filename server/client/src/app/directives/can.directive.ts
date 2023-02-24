import {
  Directive,
  ElementRef,
  Input,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { AclService } from '../services/acl.service';

@Directive({
  selector: '[can]',
})
export class CanDirective {
  private _roles: string[] = [];

  constructor(
    private aclService: AclService,
    private element: ElementRef,
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef
  ) {}

  @Input()
  set can(val: string | Array<string>) {
    this._roles = Array.isArray(val) ? val : [val];
    this.updateView();
  }

  /**
   * Update view to keep or delete current element.
   */
  private updateView() {
    if (this.aclService.hasRoles(this._roles)) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainer.clear();
    }
  }
}
