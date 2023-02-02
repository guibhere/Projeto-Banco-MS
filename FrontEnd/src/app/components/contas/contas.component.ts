import { TiposContaService } from './../../services/tipos-conta.service';
import { AgenciasService } from './../../services/agencias.service';
import { ContasService } from './../../services/contas.service';
import { Component } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Conta } from 'src/app/models/Conta';
import { ClientesService } from 'src/app/services/clientes.service';

@Component({
  selector: 'app-contas',
  templateUrl: './contas.component.html',
  styleUrls: ['./contas.component.css']
})
export class ContasComponent {
    formulario: any;
    titulo !: string;
    listaContas !: Conta[];
    visibilidadeTable: boolean = true;
    visibilidadeFormulario: boolean = false;
    atualizarCliente: boolean = false;
    nomeCliente!: string;
    cpfCliente!: string;
    modalRef!: BsModalRef;


    constructor(
      private clienteService: ClientesService,
      private modalService: BsModalService,
      private contaService: ContasService,
      private agenciaService: AgenciasService,
      private tipocontaService: TiposContaService) { }
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
