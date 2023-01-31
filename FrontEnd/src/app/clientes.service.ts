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

  GetAll(): Observable<Cliente[]> {
    return this.httpClient.get<Cliente[]>(this.url)
  }

  GetId(idCliente: number): Observable<Cliente> {
    const urlParam = `${this.url}/${idCliente}`;
    return this.httpClient.get<Cliente>(urlParam)
  }

  AddCliente(cliente: Cliente): Observable<any> {
    const urlParam = `${this.url}/Cliente/Cadastrar`;
    return this.httpClient.post<Cliente>(urlParam,cliente,httpOptions);
  }

  UpdateCliente(cliente: Cliente): Observable<any> {
    return this.httpClient.patch<Cliente>(this.url,cliente,httpOptions);
  }

  DeleteCliente(idCliente: number): Observable<any>{
    const urlParam = `${this.url}/${idCliente}`;
    return this.httpClient.delete<number>(urlParam,httpOptions);
  }




}
