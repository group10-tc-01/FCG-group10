Funcionalidade: CreateUserByAdminUseCase

@regressivo @aceite
Cenario: Admin cria usuario com perfil de usuario comum
	Dado um administrador que deseja criar um novo usuario com perfil comum
	Quando o administrador cria o usuario
	Entao o usuario deve ser criado com sucesso com perfil de usuario comum

@regressivo @aceite
Cenario: Admin cria usuario com perfil de administrador
	Dado um administrador que deseja criar um novo usuario com perfil de administrador
	Quando o administrador cria o usuario administrador
	Entao o usuario deve ser criado com sucesso com perfil de administrador

@regressivo @aceite
Cenario: Admin tenta criar usuario com email duplicado
	Dado um administrador que tenta criar um usuario com email ja existente
	Quando o administrador tenta criar o usuario
	Entao deve ocorrer um erro de email duplicado
