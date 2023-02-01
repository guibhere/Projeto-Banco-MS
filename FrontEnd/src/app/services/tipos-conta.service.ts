import { getLocaleCurrencyCode } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TipoConta } from '../models/TipoConta'

const httpOptions = {
  headers: new HttpHeaders({
    "Content-Type": "Application/json"
  })
}
@Injectable({
  providedIn: 'root'
})
export class TiposContaService {
  url = "http://localhost/TipoConta"
  constructor(private httpClient: HttpClient) { }

  GetAll(): Observable<any> {
    const urlParam = `${this.url}/Consultar/Lista`;
    return this.httpClient.get<any>(urlParam, httpOptions);
  }

  GetId(codigo: number): Observable<TipoConta> {
    const urlParam = `${this.url}/Consultar/${codigo}`;
    return this.httpClient.get<TipoConta>(urlParam);
  }

  Add(tipoconta: TipoConta): Observable<any> {
    const urlParam = `${this.url}/Cadastrar`;
    return this.httpClient.post<TipoConta>(urlParam, tipoconta, httpOptions);
  }

  Update(tipoconta: TipoConta): Observable<any> {
    const urlParam = `${this.url}/Atualizar`;
    return this.httpClient.patch<TipoConta>(urlParam, tipoconta, httpOptions);
  }

  Delete(codigo: number): Observable<any> {
    const urlParam = `${this.url}/Excluir/${codigo}`;
    return this.httpClient.delete<number>(urlParam, httpOptions);
  }
}
