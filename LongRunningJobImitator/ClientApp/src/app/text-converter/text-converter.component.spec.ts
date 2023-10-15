import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { TextConverterComponent } from './text-converter.component';

describe('TextConverter', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [RouterTestingModule],
    declarations: [TextConverterComponent]
  }));

  it('should create the app', () => {
    const fixture = TestBed.createComponent(TextConverterComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
