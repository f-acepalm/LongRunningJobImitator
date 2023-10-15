import { NgModule } from '@angular/core';

import { TextConverterRoutingModule } from './text-converter-routing.module';
import { TextConverterComponent } from './text-converter.component';

@NgModule({
  imports: [
    TextConverterRoutingModule,
  ],
  declarations: [
    TextConverterComponent
  ],
})
export class TextConverterModule { }
