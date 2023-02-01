import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AgenciasComponent } from './components/agencias/agencias.component';
import { ClientesComponent } from './components/clientes/clientes.component';

const routes: Routes = [
  { path: 'clientes', component: ClientesComponent },
  { path: 'agencias', component: AgenciasComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
