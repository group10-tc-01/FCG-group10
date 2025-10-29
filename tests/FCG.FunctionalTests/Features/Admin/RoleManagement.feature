Funcionalidade: RoleManagementUseCase

@regressivo @aceite
Cenario: Alteracao de perfil de administrador
	Dado um usuario que precisa ter seu perfil de admin alterado
	Quando o admin altera o perfil de administrador do usuario
	Entao o perfil do usuario deve ser alterado com sucesso
