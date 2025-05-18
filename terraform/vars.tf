variable "regionDefault" {
  default = "us-east-1"
}

variable "projectName" {
  default = "EKS-FOOD-ORDER-CARDAPIO"
}

variable "cluster_ca_certificate" {
  # Você precisa preencher com o valor real do certificado do seu cluster EKS
}

variable "host" {
  # Você precisa preencher com o endpoint do seu cluster EKS
}

variable "docker_image" {
  description = "URL da imagem Docker do seu serviço"
  type        = string
  default     = "japamanoel/foodorder_cardapio:latest"
}

variable "mongodb_endpoint" {
  description = "Endpoint do MongoDB"
  default     = "a778906e4964c4bb9ab2e474132b8502-b8b4ef86e905c84a.elb.us-east-1.amazonaws.com"
}

variable "mongodb_port" {
  description = "Porta do MongoDB"
  default     = 27017
}

variable "mongodb_database" {
  description = "Nome do banco de dados MongoDB"
  default     = "FoodOrder_Cardapio"
}