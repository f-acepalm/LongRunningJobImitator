import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CancelTextConversionRequest, StartTextConversionRequest, TextConverterResponse } from './models/text-converter.interface';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TextConverterService {
  private basePath = environment.apiUrl + "/TextConverter"

  constructor(private http: HttpClient) { }
  
  startProcessing(requset: StartTextConversionRequest): Observable<TextConverterResponse> {
    return this.http.post<TextConverterResponse>(this.basePath + '/start', requset);
  }

  cancelProcessing(requset: CancelTextConversionRequest): Observable<any> {
    return this.http.post(this.basePath + '/cancel', requset);
  }
}
