import { TestBed } from '@angular/core/testing';

import { TextConverterService } from './text-converter.service';

describe('TextConverterService', () => {
  let service: TextConverterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TextConverterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
