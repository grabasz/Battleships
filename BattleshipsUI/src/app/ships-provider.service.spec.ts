import { TestBed } from '@angular/core/testing';

import { ShipsProviderService } from './ships-provider.service';

describe('ShipsProviderService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ShipsProviderService = TestBed.get(ShipsProviderService);
    expect(service).toBeTruthy();
  });
});
