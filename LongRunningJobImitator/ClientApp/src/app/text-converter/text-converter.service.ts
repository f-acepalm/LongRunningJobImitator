import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { TextConverterRequest, textConverterResponse } from './models/text-converter.interface';

@Injectable({
  providedIn: 'root'
})
export class TextConverterService {
  // config = {
  //   ApiUrl: configurl.apiServer.url,
  // };
  private basePath = "https://localhost:7207/TextConverter"

  constructor(private http: HttpClient) { }
  
  startProcessing(requset: TextConverterRequest): Observable<textConverterResponse> {
    return this.http.post<textConverterResponse>(this.basePath + '/start', requset);
  }
}
