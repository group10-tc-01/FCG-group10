Funcionalidade: GetAllUsersUseCase

@regressivo @aceite
Cenario: Listagem de todos os usuarios
	Dado que existem usuarios cadastrados no sistema
	Quando o admin solicita a listagem de todos os usuarios
	Entao o sistema deve retornar a lista de usuarios com sucesso
