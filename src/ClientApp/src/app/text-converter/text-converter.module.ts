import { NgModule } from '@angular/core';

import { TextConverterRoutingModule } from './text-converter-routing.module';
import { TextConverterComponent } from './text-converter.component';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDividerModule } from '@angular/material/divider';
import { MatToolbarModule } from '@angular/material/toolbar';
import {MatIconModule} from '@angular/material/icon';
import { CommonModule } from '@angular/common';

@NgModule({
  imports: [
    MatButtonModule,
    TextConverterRoutingModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDividerModule,
    MatToolbarModule,
    MatIconModule,
    CommonModule
  ],
  declarations: [
    TextConverterComponent
  ],
})
export class TextConverterModule { }
