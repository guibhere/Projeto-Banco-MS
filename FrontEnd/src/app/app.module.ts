import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {HttpClientModule} from  '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { ModalModule} from "ngx-bootstrap/modal";
import { ClientesComponent } from './components/clientes/clientes.component';
import { AgenciasComponent } from './components/agencias/agencias.component';
import { ClientesService } from './services/clientes.service';
import { TiposContaComponent } from './components/tipos-conta/tipos-conta.component';

@NgModule({
  declarations: [
    AppComponent,
    ClientesComponent,
    AgenciasComponent,
    TiposContaComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule,
    ModalModule.forRoot()
  ],
  providers: [HttpClientModule,ClientesService],
  bootstrap: [AppComponent]
})
export class AppModule { }
