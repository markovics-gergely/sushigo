import { TestBed } from '@angular/core/testing';

import { GameSelectService } from './game-select.service';

describe('GameSelectService', () => {
  let service: GameSelectService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GameSelectService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
