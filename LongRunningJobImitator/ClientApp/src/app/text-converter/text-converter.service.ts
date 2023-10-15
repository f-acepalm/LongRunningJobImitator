import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TextConverterService {
  // config = {
  //   ApiUrl: configurl.apiServer.url,
  // };
  constructor(private http: HttpClient) { }
  
  getData(): Observable<string> {
    return this.http.get<string>('https://localhost:4015/WeatherForecast');
  }
}
