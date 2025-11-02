Funcionalidade: DepositToWalletUseCase

@regressivo @aceite
Cenario: Admin deposita valor na carteira de um usuario
	Dado um administrador que deseja depositar valor na carteira de um usuario
	Quando o administrador realiza o deposito
	Entao o saldo da carteira deve ser atualizado com sucesso

@regressivo @aceite
Cenario: Admin tenta depositar em carteira inexistente
	Dado um administrador que tenta depositar em uma carteira que nao existe
	Quando o administrador tenta realizar o deposito
	Entao deve ocorrer um erro de carteira nao encontrada
