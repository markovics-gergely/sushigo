import { TestBed } from '@angular/core/testing';

import { PlayStrategyService } from './play-strategy.service';

describe('PlayStrategyService', () => {
  let service: PlayStrategyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PlayStrategyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
