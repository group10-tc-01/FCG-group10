Funcionalidade: DepositToWalletUseCase

@regressivo @aceite
Cenario: Admin tenta depositar em carteira inexistente
	Dado um administrador que tenta depositar em uma carteira que nao existe
	Quando o administrador tenta realizar o deposito
	Entao deve ocorrer um erro de carteira nao encontrada
