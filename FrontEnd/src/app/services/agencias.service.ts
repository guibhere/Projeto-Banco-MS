import { getLocaleCurrencyCode } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Agencia } from '../models/Agencia';

const httpOptions = {
  headers: new HttpHeaders({
    "Content-Type": "Application/json"
  })
}
@Injectable({
  providedIn: 'root'
})
export class AgenciasService {
  url = "http://localhost/Agencia"
  constructor(private httpClient: HttpClient) { }

  GetAll(): Observable<any> {
    const urlParam = `${this.url}/Consultar/Lista`;
    return this.httpClient.get<any>(urlParam, httpOptions);
  }

  GetId(numero: string): Observable<Agencia> {
    const urlParam = `${this.url}/Consultar/${numero}`;
    return this.httpClient.get<Agencia>(urlParam);
  }

  Add(agencia: Agencia): Observable<any> {
    const urlParam = `${this.url}/Cadastrar`;
    return this.httpClient.post<Agencia>(urlParam, agencia, httpOptions);
  }

  Update(agencia: Agencia): Observable<any> {
    const urlParam = `${this.url}/Atualizar`;
    return this.httpClient.patch<Agencia>(urlParam, agencia, httpOptions);
  }

  Delete(numero: string): Observable<any> {
    const urlParam = `${this.url}/Excluir/${numero}`;
    return this.httpClient.delete<number>(urlParam, httpOptions);
  }
}
