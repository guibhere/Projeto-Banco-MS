import { TestBed } from '@angular/core/testing';

import { TiposContaService } from './tipos-conta.service';

describe('TiposContaService', () => {
  let service: TiposContaService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TiposContaService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
