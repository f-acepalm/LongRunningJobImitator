import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TextConverterComponent } from './text-converter.component';

const routes: Routes = [
  {
    path: '',
    component: TextConverterComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TextConverterRoutingModule { }
