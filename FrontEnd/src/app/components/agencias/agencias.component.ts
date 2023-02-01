import { Component, TemplateRef } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Agencia } from 'src/app/models/Agencia';
import { AgenciasService } from 'src/app/services/agencias.service';

@Component({
  selector: 'app-agencias',
  templateUrl: './agencias.component.html',
  styleUrls: ['./agencias.component.css']
})
export class AgenciasComponent {
  formulario: any;
  titulo !: string;
  listaAgencias !: Agencia[];
  visibilidadeTable: boolean = true;
  visibilidadeFormulario: boolean = false;
  atualizarAgencia: boolean = false;
  numeroAgencia!: string;
  descricaoAgencia!: string;
  modalRef!: BsModalRef;


  constructor(private agenciaService: AgenciasService,
    private modalService: BsModalService) { }

  ngOnInit(): void {
    this.agenciaService.GetAll().subscribe(result => {
      this.listaAgencias = result.dados
    });
  }
  ExibirFormCadastro(): void {
    this.visibilidadeTable = false;
    this.visibilidadeFormulario = true;
    this.titulo = "Cadastro de Agencia";
    this.formulario = new FormGroup({
      numero_Agencia: new FormControl(null),
      descricao: new FormControl(null)
    })
  }
  Voltar(): void {
    this.visibilidadeFormulario = false;
    this.visibilidadeTable = true;
    this.atualizarAgencia = false;
  }
  EnviarFormulario(): void {
    if (this.atualizarAgencia)
      this.EnviarFormAtualizar();
    else {
      const agencia: Agencia = this.formulario.value;
      this.agenciaService.Add(agencia).subscribe(resultado => {
        this.visibilidadeFormulario = false;
        this.visibilidadeTable = true;
        alert('Agencia cadastrada com sucesso!')
        this.agenciaService.GetAll().subscribe(result => {
          this.listaAgencias = result.dados
        });
      });
    }
  }
  ExibirFormAtualizacao(agenciaNumero: string): void {
    this.visibilidadeFormulario = true;
    this.visibilidadeTable = false;
    this.atualizarAgencia = true;

    this.agenciaService.GetId(agenciaNumero).subscribe(result => {
      this.titulo = `Atualizar ${result.numero_Agencia}`;

      this.formulario = new FormGroup({
        numero_Agencia: new FormControl(result.numero_Agencia),
        descricao: new FormControl(result.descricao)
      });
    });
  }
  EnviarFormAtualizar(): void {
    const agencia: Agencia = this.formulario.value;
    this.agenciaService.Update(agencia).subscribe(resultado => {
      this.visibilidadeFormulario = false;
      this.visibilidadeTable = true;
      this.atualizarAgencia = false;
      alert('Agencia atualizada com sucesso!')
      this.agenciaService.GetAll().subscribe(result => {
        this.listaAgencias = result.dados
      });
    });
  }
  ExibirConfirmacaoExclusao(numero: string, descricao: string, conteudoModal: TemplateRef<any>): void {
    this.modalRef = this.modalService.show(conteudoModal);
    this.descricaoAgencia = descricao;
    this.numeroAgencia = numero;
  }
  Excluir(numero: string): void {
    this.agenciaService.Delete(numero).subscribe(resposta => {
      this.modalRef.hide();
      alert('Agencia removida com sucesso!')
      this.agenciaService.GetAll().subscribe(result => {
        this.listaAgencias = result.dados
      });

    });
  }
}
