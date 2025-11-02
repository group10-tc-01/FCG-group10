Funcionalidade: GetUserByIdUseCase

@regressivo @aceite
Cenario: Busca de usuario por ID
	Dado um usuario existente no sistema
	Quando o admin solicita os detalhes do usuario por ID
	Entao o sistema deve retornar os detalhes do usuario com sucesso
