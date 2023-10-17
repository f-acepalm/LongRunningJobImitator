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
  isLoading = false;
  messagesSubscription$: Subscription | undefined;
  doneSubscription$: Subscription | undefined;
  private currentJobId: string | undefined;

  constructor(
    private readonly service: TextConverterService,
    private readonly signalrService: SignalrService) {
      this.connectToSignalr();
      this.initializeSubscriptions();
    } 

  convertText(): void {
    this.cancelConversion();
    this.outputText$.next('');
    this.isLoading = true;
    const requset: StartTextConversionRequest = {
      text: this.inputText
    };

    // TODO: catch errors
    this.service.startProcessing(requset).subscribe(result => {
      this.signalrService.joinGroup(result.jobId).then(() => {
        this.currentJobId = result.jobId;
      }, (err) => {
        console.log(err);
      })
    });
  }

  cancelConversion(): void {
    if(this.currentJobId) {
      this.outputText$.next('Canceled!');
      this.isLoading = false;
      this.signalrService.leaveGroup(this.currentJobId);
      const requset: CancelTextConversionRequest = {
        jobId: this.currentJobId
      };

      this.currentJobId = undefined;
  
      this.service.cancelProcessing(requset).subscribe({
        next: () => { 
          console.log('Canceled');
        },
        error: (error) => console.error(error)
      });
    }
  }

  ngOnDestroy(): void {
    this.cancelConversion();
    this.messagesSubscription$?.unsubscribe();
    this.doneSubscription$?.unsubscribe();
  }

  private initializeSubscriptions() {
    this.messagesSubscription$ = this.signalrService.receivedMessage$.subscribe((message: string) => {
      console.log(message);
      this.outputText$.next(this.outputText$.value + message);
    });

    this.doneSubscription$ = this.signalrService.done$.subscribe(() => {
      console.log('Done!');
      this.outputText$.next(this.outputText$.value + '\n' + 'Done!');
      this.isLoading = false;

      if (this.currentJobId) {
        this.signalrService.leaveGroup(this.currentJobId);
      }
      this.currentJobId = undefined;
    });
  }

  private connectToSignalr() {
    this.signalrService.startConnection().then(() => {
      console.log('Connected!'); // TODO: remove
    }, (err) => {
      console.log(err);
    });
  }
}