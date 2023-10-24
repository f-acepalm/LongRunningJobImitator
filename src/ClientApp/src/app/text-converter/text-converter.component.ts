import { Component } from '@angular/core';
import { TextConverterService } from './text-converter.service';
import { BehaviorSubject, Subscription } from 'rxjs';
import { CancelTextConversionRequest, StartTextConversionRequest } from './models/text-converter.interface';
import { SignalrService } from '../shared/signalr/signalr.service';
import { FormControl, Validators } from '@angular/forms';
import { ApiError } from '../shared/api-error.interface';

@Component({
  selector: 'text-converter',
  templateUrl: './text-converter.component.html',
  styleUrls: ['./text-converter.component.scss']
})
export class TextConverterComponent {
  outputText$ = new BehaviorSubject('');
  isLoading = false;
  messagesSubscription$: Subscription | undefined;
  doneSubscription$: Subscription | undefined;
  inputForm = new FormControl('', [Validators.required]);
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
      text: this.inputForm.value!
    };

    // TODO: catch errors
    this.service.startProcessing(requset).subscribe({
      next: result => {
        this.signalrService.joinGroup(result.jobId).then(() => {
          this.currentJobId = result.jobId;
        }, (err) => {
          console.log(err);
        })
      },
      error: (error: ApiError) => { 
        this.inputForm.setErrors({ 'submitError': error.message });
        this.isLoading = false;
      }
    });
  }

  getErrorMessage() {
    if (this.inputForm.hasError('required')) {
      return 'You must enter a value';
    }

    if (this.inputForm.hasError('submitError')) {
      return this.inputForm.getError('submitError');
    }

    return '';
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
        }
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