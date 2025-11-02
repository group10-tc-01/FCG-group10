Funcionalidade: CreatePromotionUseCase

@regressivo @aceite
Cenario: Criacao de nova promocao por admin
	Dado a criacao de uma nova promocao
	Quando o admin envia uma requisicao de criacao de promocao com dados validos
	Entao a promocao deve ser criada com sucesso
