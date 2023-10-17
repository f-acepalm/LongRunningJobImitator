import { Component } from '@angular/core';
import { TextConverterService } from './text-converter.service';
import { BehaviorSubject, Subscription } from 'rxjs';
import { CancelTextConversionRequest, StartTextConversionRequest } from './models/text-converter.interface';
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
  doneSubscription$: Subscription | undefined;
  canceledSubscription$: Subscription | undefined;
  private currentJobId: string | undefined;

  constructor(
    private readonly service: TextConverterService,
    private readonly signalrService: SignalrService) {
      this.signalrService.startConnection().then(() => {
        console.log('Connected!'); // TODO: remove
      }, (err) => {
        console.log(err);
      });

      this.messagesSubscription$ = this.signalrService.receivedMessage$.subscribe((message: string) => {
        console.log(message);
        this.outputText$.next(this.outputText$.value + message);
      });

      this.doneSubscription$ = this.signalrService.done$.subscribe(() => {
        console.log('Done!');
        this.outputText$.next(this.outputText$.value + '\n' + 'Done!');
        this.signalrService.leaveGroup(this.currentJobId!); // TODO: if?
        this.currentJobId = undefined;
      });

      this.canceledSubscription$ = this.signalrService.canceled$.subscribe(() => {
        console.log('Canceled!');
        this.outputText$.next('Canceled!');
        this.signalrService.leaveGroup(this.currentJobId!);
        this.currentJobId = undefined;
      });
    }

  submitText(): void {
    this.outputText$.next('');
    // TODO: cancelConversion
    const requset: StartTextConversionRequest = {
      text: this.inputText
    };

    // TODO: catch errors
    this.service.startProcessing(requset).subscribe(result => {
      this.signalrService.joinGroup(result.jobId).then(() => {
        this.currentJobId = result.jobId;
        // this.messagesSubscription$ = this.signalrService.receivedMessage$.subscribe((message: string) => {
        //   console.log(message);
        //   this.outputText$.next(this.outputText$.value + message);
        // });
      }, (err) => {
        console.log(err);
      })
    });
  }

  cancelConversion(): void {
    if(this.currentJobId) {
      const requset: CancelTextConversionRequest = {
        jobId: this.currentJobId
      };
  
      this.service.cancelProcessing(requset).subscribe({
        next: () => console.log('Canceled'),
        error: (error) => console.error(error)
      });
  
      // this.signalrService.leaveGroup(this.currentJobId);
      // this.currentJobId = undefined;
    }
  }

  ngOnDestroy(): void {
    this.messagesSubscription$?.unsubscribe();
    this.doneSubscription$?.unsubscribe();
    this.canceledSubscription$?.unsubscribe();
  }
}