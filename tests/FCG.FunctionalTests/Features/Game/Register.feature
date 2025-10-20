Funcionalidade: RegisterGameUseCase

@regressivo @aceite
Cenario: Criacao de novo jogo por admin
	Dado a criacao de um novo jogo
	Quando o admin envia uma requisicao de criacao de jogo com dados validos
	Entao o jogo deve ser criado com sucesso
