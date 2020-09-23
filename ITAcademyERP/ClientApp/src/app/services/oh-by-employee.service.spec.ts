import { TestBed } from '@angular/core/testing';

import { OhByEmployeeService } from './oh-by-employee.service';

describe('OhByEmployeeService', () => {
  let service: OhByEmployeeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OhByEmployeeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
