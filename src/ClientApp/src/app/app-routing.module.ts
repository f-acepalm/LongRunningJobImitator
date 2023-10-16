import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'text-converter',
    loadChildren: () => import('./text-converter/text-converter.module').then(m => m.TextConverterModule),
  },
  {
    path: '',
    loadChildren: () => import('./text-converter/text-converter.module').then(m => m.TextConverterModule),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
