Funcionalidade: LogoutUseCase

@regressivo @aceite
Cenario: Logout com usuario valido
	Dado que o usuario possui um token valido
	Quando o usuario envia uma requisicao de logout com userid valido
	Entao todos os refresh tokens do usuario devem ser revogados
	E o sistema deve retornar mensagem de sucesso