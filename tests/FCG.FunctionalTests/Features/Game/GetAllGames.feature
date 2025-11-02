Funcionalidade: GetAllGamesUseCase

@regressivo @aceite
Cenario: Listagem de todos os jogos com paginacao
	Dado que existem jogos cadastrados no sistema
	Quando o usuario solicita a listagem de jogos com paginacao
	Entao o sistema deve retornar a lista de jogos paginada com sucesso

@regressivo @aceite
Cenario: Listagem de jogos com filtro por nome
	Dado que existem jogos cadastrados no sistema
	Quando o usuario solicita a listagem de jogos filtrando por nome
	Entao o sistema deve retornar apenas os jogos que correspondem ao filtro de nome

@regressivo @aceite
Cenario: Listagem de jogos com filtro por categoria
	Dado que existem jogos cadastrados no sistema
	Quando o usuario solicita a listagem de jogos filtrando por categoria
	Entao o sistema deve retornar apenas os jogos da categoria especificada

@regressivo @aceite
Cenario: Listagem de jogos com filtro por faixa de preco
	Dado que existem jogos cadastrados no sistema
	Quando o usuario solicita a listagem de jogos filtrando por faixa de preco
	Entao o sistema deve retornar apenas os jogos dentro da faixa de preco

@regressivo @aceite
Cenario: Listagem de jogos com multiplos filtros
	Dado que existem jogos cadastrados no sistema
	Quando o usuario solicita a listagem de jogos com multiplos filtros aplicados
	Entao o sistema deve retornar apenas os jogos que atendem a todos os filtros