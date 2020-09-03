import { TestBed } from '@angular/core/testing';

import { PersonLastIdService } from './person-last-id.service';

describe('PersonLastIdService', () => {
  let service: PersonLastIdService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PersonLastIdService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
