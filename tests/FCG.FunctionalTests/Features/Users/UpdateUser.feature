Funcionalidade: UpdateUserUseCase

@regressivo @aceite
Cenario: Atualizacao de senha de usuario
	Dado um usuario existente que deseja atualizar sua senha
	Quando o usuario envia uma requisicao de atualizacao com nova senha valida
	Entao a senha do usuario deve ser atualizada com sucesso
