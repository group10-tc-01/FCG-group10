Funcionalidade: RefreshTokenUseCase

@regressivo @aceite
Cenario: Gerar novo token de acesso com sucesso
	Dado que o usuario deseja gerar um novo token de acesso
	Quando o usuario envia uma requisicao de refresh token
	Entao o usuario deve receber um novo token de acesso com sucesso

@regressivo @aceite
Cenario: Refresh token com token valido
	Dado que o usuario possui um refresh token valido
	Quando o usuario envia uma requisicao de refresh token com token valido
	Entao o sistema deve gerar um novo access token
	E o sistema deve gerar um novo refresh token
	E o sistema deve retornar o tempo de expiracao

@regressivo @aceite
Cenario: Refresh token com token invalido
	Dado que o usuario possui um refresh token invalido
	Quando o usuario envia uma requisicao de refresh token com token invalido
	Entao o sistema deve retornar erro de token invalido

@regressivo @aceite
Cenario: Refresh token com token expirado
	Dado que o usuario possui um refresh token expirado
	Quando o usuario envia uma requisicao de refresh token com token expirado
	Entao o sistema deve retornar erro de token expirado