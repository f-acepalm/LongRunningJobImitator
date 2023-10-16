import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TextConverterRequest, textConverterResponse } from './models/text-converter.interface';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TextConverterService {
  private basePath = environment.apiUrl + "/TextConverter"

  constructor(private http: HttpClient) { }
  
  startProcessing(requset: TextConverterRequest): Observable<textConverterResponse> {
    return this.http.post<textConverterResponse>(this.basePath + '/start', requset);
  }
}
