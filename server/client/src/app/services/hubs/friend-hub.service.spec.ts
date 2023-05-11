import { TestBed } from '@angular/core/testing';

import { FriendHubService } from './friend-hub.service';

describe('FriendHubService', () => {
  let service: FriendHubService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FriendHubService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
