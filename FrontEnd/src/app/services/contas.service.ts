import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Conta } from '../models/Conta';

const httpOptions = {
  headers: new HttpHeaders({
    "Content-Type": "Application/json"
  })
}
@Injectable({
  providedIn: 'root'
})
export class ContasService {
  url = "http://localhost/Conta"
  constructor(private httpClient: HttpClient) { }

  Add(tipoconta: Conta): Observable<any> {
    const urlParam = `${this.url}/Cadastrar`;
    return this.httpClient.post<Conta>(urlParam, tipoconta, httpOptions);
  }
}
