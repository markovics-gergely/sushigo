import {
  Directive,
  ElementRef,
  Input,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { AppRole } from 'src/shared/user.models';
import { AclService } from '../services/acl.service';
import { TokenService } from '../services/token.service';

@Directive({
  selector: '[can]',
})
export class CanDirective {
  private _roles: (AppRole | '*' | '!')[] = [];

  constructor(
    private aclService: AclService,
    private element: ElementRef,
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    tokenService: TokenService
  ) {
    tokenService.userEventEmitter.subscribe(() => {
      viewContainer.clear();
      this.updateView();
    });
  }

  @Input()
  set can(val: (AppRole | '*') | Array<(AppRole | '*' | '!')>) {
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
