import { Component } from '@angular/core';
import { ApiClientService } from './api-client.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  response: string | undefined;

  constructor(private apiClientService: ApiClientService){}

  ngOnInit(): void {
    this.response = 'test';
    // this.apiClientService.getData().subscribe(data =>{
    //   console.log(data)
    //   this.response = data;
    // });
  }
}
