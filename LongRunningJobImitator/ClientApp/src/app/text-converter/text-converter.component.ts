import { Component } from '@angular/core';
import { TextConverterService } from './text-converter.service';

@Component({
  selector: 'text-converter',
  templateUrl: './text-converter.component.html',
  styleUrls: ['./text-converter.component.scss']
})
export class TextConverterComponent {
  response: string | undefined;

  constructor(private service: TextConverterService){}

  ngOnInit(): void {
    this.response = 'test';
    // this.apiClientService.getData().subscribe(data =>{
    //   console.log(data)
    //   this.response = data;
    // });
  }
}
