import { TestBed } from '@angular/core/testing';

import { GamePermissionService } from './game-permission.service';

describe('GamePermissionService', () => {
  let service: GamePermissionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GamePermissionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
