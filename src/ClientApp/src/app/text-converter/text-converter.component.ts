import { Component } from '@angular/core';
import { TextConverterService } from './text-converter.service';
import { BehaviorSubject, Subscription } from 'rxjs';
import { TextConverterRequest } from './models/text-converter.interface';
import { SignalrService } from '../shared/signalr/signalr.service';

@Component({
  selector: 'text-converter',
  templateUrl: './text-converter.component.html',
  styleUrls: ['./text-converter.component.scss']
})
export class TextConverterComponent {
  inputText: string = '';
  outputText$ = new BehaviorSubject('');
  messagesSubscription$: Subscription | undefined; // TODO
  
  constructor(private readonly service: TextConverterService,
    private readonly signalrService: SignalrService) {}

  submitText(): void {
    const requset: TextConverterRequest = {
      text: this.inputText
    };

    this.service.startProcessing(requset).subscribe(result => {
      this.signalrService.startConnection().then(() => {
        this.signalrService.joinGroup(result.jobId).then(() => {
          this.signalrService.listen();
          this.outputText$.next('');
          this.messagesSubscription$ = this.signalrService.receivedMessage$.subscribe((message: string) => {
            console.log(message);
            this.outputText$.next(this.outputText$.value + message);
          });
        }, (err) => {
          console.log(err);
        })
      })

      
    });

    this.signalrService
  }

  ngOnDestroy(): void {
    this.messagesSubscription$?.unsubscribe();
  }
}