import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { memberDetailsResolver } from './member-details.resolver';

describe('memberDetailsResolver', () => {
  const executeResolver: ResolveFn<boolean> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => memberDetailsResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});
