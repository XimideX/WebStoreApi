import { CatalogoComponent } from './catalogo/catalogo.component';
import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CadastroComponent } from './cadastro/cadastro.component';
import { HomeComponent } from './home/home.component';


const routes: Routes = [
{path: 'home', component: HomeComponent},
{path: 'cadastro', component: CadastroComponent},
{path: 'catalogo',component:CatalogoComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
export const routingComponets=[HomeComponent,CadastroComponent]
