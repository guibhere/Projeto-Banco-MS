import { Agencia } from "./Agencia";
import { Cliente } from "./Cliente";
import { TipoConta } from "./TipoConta";

export class Conta {
  saldo !: number;
  codigo_Tipo_Conta!: number;
  digito !: string;
  numeroConta !: string;
  numero_Agencia !: string;
  agencia !: Agencia;
  tipocoonta !: TipoConta;
  cliente !: Cliente;
}
