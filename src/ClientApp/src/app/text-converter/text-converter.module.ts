import { NgModule } from '@angular/core';

import { TextConverterRoutingModule } from './text-converter-routing.module';
import { TextConverterComponent } from './text-converter.component';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDividerModule } from '@angular/material/divider';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { MatProgressBarModule } from '@angular/material/progress-bar';

@NgModule({
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    TextConverterRoutingModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDividerModule,
    MatToolbarModule,
    MatIconModule,
    CommonModule,
    MatProgressBarModule
  ],
  declarations: [
    TextConverterComponent
  ],
})
export class TextConverterModule { }
