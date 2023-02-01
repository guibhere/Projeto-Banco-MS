import { Component, TemplateRef } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Cliente } from 'src/app/models/Cliente';
import { ClientesService } from 'src/app/services/clientes.service';

@Component({
  selector: 'app-clientes',
  templateUrl: './clientes.component.html',
  styleUrls: ['./clientes.component.css']
})
export class ClientesComponent {
  formulario: any;
  titulo !: string;
  listaClientes !: Cliente[];
  visibilidadeTable: boolean = true;
  visibilidadeFormulario: boolean = false;
  atualizarCliente: boolean = false;
  nomeCliente!: string;
  cpfCliente!: string;
  modalRef!: BsModalRef;


  constructor(private clienteService: ClientesService,
    private modalService: BsModalService) { }

  ngOnInit(): void {
    this.clienteService.GetAll().subscribe(result => {
      this.listaClientes = result.dados
    });
  }
  ExibirFormCadastro(): void {
    this.visibilidadeTable = false;
    this.visibilidadeFormulario = true;
    this.titulo = "Cadastro de cliente";
    this.formulario = new FormGroup({
      nome: new FormControl(null),
      cpf: new FormControl(null)
    })
  }
  Voltar(): void {
    this.visibilidadeFormulario = false;
    this.visibilidadeTable = true;
    this.atualizarCliente = false;
  }
  EnviarFormulario(): void {
    if (this.atualizarCliente)
      this.EnviarFormAtualizar();
    else {
      const cliente: Cliente = this.formulario.value;
      this.clienteService.AddCliente(cliente).subscribe(resultado => {
        this.visibilidadeFormulario = false;
        this.visibilidadeTable = true;
        alert('Cliente cadastrado com sucesso!')
        this.clienteService.GetAll().subscribe(result => {
          this.listaClientes = result.dados
        });
      });
    }
  }
  ExibirFormAtualizacao(clienteCpf: string): void {
    this.visibilidadeFormulario = true;
    this.visibilidadeTable = false;
    this.atualizarCliente = true;

    this.clienteService.GetId(clienteCpf).subscribe(result => {
      this.titulo = `Atualizar ${result.nome}`;

      this.formulario = new FormGroup({
        cpf: new FormControl(result.cpf),
        nome: new FormControl(result.nome)
      });
    });
  }
  EnviarFormAtualizar(): void {
    const cliente: Cliente = this.formulario.value;
    this.clienteService.UpdateCliente(cliente).subscribe(resultado => {
      this.visibilidadeFormulario = false;
      this.visibilidadeTable = true;
      this.atualizarCliente = false;
      alert('Cliente atualizado com sucesso!')
      this.clienteService.GetAll().subscribe(result => {
        this.listaClientes = result.dados
      });
    });
  }
  ExibirConfirmacaoExclusao(nome:string,cpf:string,conteudoModal: TemplateRef<any>): void
  {
    this.modalRef = this.modalService.show(conteudoModal);
    this.cpfCliente = cpf;
    this.nomeCliente = nome;
  }
  ExcluirCliente(cpf: string): void {
    this.clienteService.DeleteCliente(cpf).subscribe(resposta => {
      this.modalRef.hide();
      alert('Cliente removido com sucesso!')
      this.clienteService.GetAll().subscribe(result => {
        this.listaClientes = result.dados
      });

    });
  }
}
