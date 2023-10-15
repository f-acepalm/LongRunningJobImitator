import { Component } from '@angular/core';
import { TextConverterService } from './text-converter.service';
import { BehaviorSubject } from 'rxjs';
import { TextConverterRequest } from './models/text-converter.interface';

@Component({
  selector: 'text-converter',
  templateUrl: './text-converter.component.html',
  styleUrls: ['./text-converter.component.scss']
})
export class TextConverterComponent {
  inputText: string = '';
  outputText$ = new BehaviorSubject('');

  constructor(private readonly service: TextConverterService) {}

  submitText(): void {
    const requset: TextConverterRequest = {
      text: this.inputText
    };

    this.service.startProcessing(requset).subscribe(result => {
      this.outputText$.next(result.result);
    });
  }
}
