Funcionalidade: LoginUseCase

@regressivo @aceite
Cenario: Login de usuario com sucesso
	Dado que o usuario deseja realizar o login
	Quando o usuario envia uma requisicao de login
	Entao o sistema deve autenticar o usuario com sucesso