import { TestBed } from '@angular/core/testing';

import { OhByClientService } from './oh-by-client.service';

describe('OhByClientService', () => {
  let service: OhByClientService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OhByClientService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
