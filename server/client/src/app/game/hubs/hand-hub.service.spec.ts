import { TestBed } from '@angular/core/testing';

import { HandHubService } from './hand-hub.service';

describe('HandHubService', () => {
  let service: HandHubService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HandHubService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
