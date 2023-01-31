import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-clientes',
  templateUrl: './clientes.component.html',
  styleUrls: ['./clientes.component.css']
})
export class ClientesComponent {
  formulario: any;
  titulo !: string;
  ngOnInit(): void {
    this.titulo = "Teste";
    this.formulario = new FormGroup({
      nome: new FormControl(null),
      cpf: new FormControl(null)
    })
  }
}
