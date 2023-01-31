import { getLocaleCurrencyCode } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Cliente } from './Cliente';

const httpOptions = {
  headers: new HttpHeaders({
    "Content-Type": "Application/json"
  })
}
@Injectable({
  providedIn: 'root'
})
export class ClientesService {
  url = "http://localhost"
  constructor(private httpClient: HttpClient) { }

  GetAll(): Observable<any> {
    const urlParam = `${this.url}/Cliente/Consultar/Lista`;
    return this.httpClient.get<any>(urlParam,httpOptions);
  }

  GetId(cpf: string): Observable<Cliente> {
    const urlParam = `${this.url}/Cliente/Consultar/${cpf}`;
    return this.httpClient.get<Cliente>(urlParam);
  }

  AddCliente(cliente: Cliente): Observable<any> {
    const urlParam = `${this.url}/Cliente/Cadastrar`;
    return this.httpClient.post<Cliente>(urlParam,cliente,httpOptions);
  }

  UpdateCliente(cliente: Cliente): Observable<any> {
    const urlParam = `${this.url}/Cliente/Atualizar`;
    return this.httpClient.patch<Cliente>(urlParam,cliente,httpOptions);
  }

  DeleteCliente(cpf: string): Observable<any>{
    const urlParam = `${this.url}/Cliente/Excluir/${cpf}`;
    return this.httpClient.delete<number>(urlParam,httpOptions);
  }




}
