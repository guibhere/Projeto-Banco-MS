import { Component, TemplateRef } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { TipoConta } from 'src/app/models/TipoConta';
import { TiposContaService } from 'src/app/services/tipos-conta.service';

@Component({
  selector: 'app-tipos-conta',
  templateUrl: './tipos-conta.component.html',
  styleUrls: ['./tipos-conta.component.css']
})
export class TiposContaComponent {
  formulario: any;
  titulo !: string;
  listaTiposConta !: TipoConta[];
  visibilidadeTable: boolean = true;
  visibilidadeFormulario: boolean = false;
  atualizartipoConta: boolean = false;
  codigotipoConta!: number;
  descricaotipoConta!: string;
  modalRef!: BsModalRef;

  constructor(private tipocontaService: TiposContaService, private modalService: BsModalService) { }
  ngOnInit(): void {
    this.tipocontaService.GetAll().subscribe(result => {
      this.listaTiposConta = result.dados
    });
  }
  ExibirFormCadastro(): void {
    this.visibilidadeTable = false;
    this.visibilidadeFormulario = true;
    this.titulo = "Cadastro de Tipo Conta";
    this.formulario = new FormGroup({
      codigo_Tipo_Conta: new FormControl(null),
      descricao: new FormControl(null)
    })
  }
  Voltar(): void {
    this.visibilidadeFormulario = false;
    this.visibilidadeTable = true;
    this.atualizartipoConta = false;
  }
  EnviarFormulario(): void {
    if (this.atualizartipoConta)
      this.EnviarFormAtualizar();
    else {
      const tipoconta: TipoConta = this.formulario.value;
      this.tipocontaService.Add(tipoconta).subscribe(resultado => {
        this.visibilidadeFormulario = false;
        this.visibilidadeTable = true;
        alert('Agencia cadastrada com sucesso!')
        this.tipocontaService.GetAll().subscribe(result => {
          this.listaTiposConta = result.dados
        });
      });
    }
  }
  ExibirFormAtualizacao(tipocontaCodigo: number): void {
    this.visibilidadeFormulario = true;
    this.visibilidadeTable = false;
    this.atualizartipoConta = true;

    this.tipocontaService.GetId(tipocontaCodigo).subscribe(result => {
      this.titulo = `Atualizar ${result.codigo_Tipo_Conta}`;

      this.formulario = new FormGroup({
        codigo_Tipo_Conta: new FormControl(result.codigo_Tipo_Conta),
        descricao: new FormControl(result.descricao)
      });
    });
  }
  EnviarFormAtualizar(): void {
    const tipoconta: TipoConta = this.formulario.value;
    this.tipocontaService.Update(tipoconta).subscribe(resultado => {
      this.visibilidadeFormulario = false;
      this.visibilidadeTable = true;
      this.atualizartipoConta = false;
      alert('Agencia atualizada com sucesso!')
      this.tipocontaService.GetAll().subscribe(result => {
        this.listaTiposConta = result.dados
      });
    });
  }
  ExibirConfirmacaoExclusao(codigotipoCOnta: number, descricao: string, conteudoModal: TemplateRef<any>): void {
    this.modalRef = this.modalService.show(conteudoModal);
    this.descricaotipoConta = descricao;
    this.codigotipoConta = codigotipoCOnta;
  }
  Excluir(codigoTipoConta: number): void {
    this.tipocontaService.Delete(codigoTipoConta).subscribe(resposta => {
      this.modalRef.hide();
      alert('Agencia removida com sucesso!')
      this.tipocontaService.GetAll().subscribe(result => {
        this.listaTiposConta = result.dados
      });

    });
  }

}
