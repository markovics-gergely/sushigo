import { TestBed } from '@angular/core/testing';

import { LobbyListHubService } from './lobby-list-hub.service';

describe('LobbyListHubService', () => {
  let service: LobbyListHubService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LobbyListHubService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
