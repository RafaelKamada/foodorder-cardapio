variable "regionDefault" {
  default = "us-east-1"
}

variable "projectName" {
  default = "EKS-FOOD-ORDER-API"
}

variable "cluster_ca_certificate" {
  description = "Certificado do cluster EKS"
  default = "LS0tLS1CRUdJTiBDRVJUSUZJQ0FURS0tLS0tCk1JSURCVENDQWUyZ0F3SUJBZ0lJYkRhNWYzUjJmZFV3RFFZSktvWklodmNOQVFFTEJRQXdGVEVUTUJFR0ExVUUKQXhNS2EzVmlaWEp1WlhSbGN6QWVGdzB5TlRBMU1UZ3lNREkyTWpOYUZ3MHpOVEExTVRZeU1ETXhNak5hTUJVeApFekFSQmdOVkJBTVRDbXQxWW1WeWJtVjBaWE13Z2dFaU1BMEdDU3FHU0liM0RRRUJBUVVBQTRJQkR3QXdnZ0VLCkFvSUJBUURIVlNwVlowWVZGemVHbGdrRm1jclAwUWdZU0orY1FYYy9ZMmtwQUpjdzN3MkJBSUl0UlUxUjB2Y1QKanBwNDY5UnY0MVpaR2M2V0pHcWxHYjY5WUJpa2FHQVVpdkxRbW4yK1ZhOGczeURGUGxGaXM3SXE3OVo4Snd5bgpQMDZvdDA5c1d2cEFyUXJYQ291NllCOXZydHpxc0RWbW96N0pNd0thMjcyd3RzYTNXenFxZUJqNGNMQ1RCWnArClE3Y2psUmpqWDVPUjdiZFdnenJPRXppZmJGZmFBbFNuM0hrUDlJd2plNUxxQW9kQjZDOTd3azJwbXhEZ25FZ08KM2s0dkNYeUhpbERkSkkzMHZGR3o4U283S0hPR3pXYWJJL3FZZitlWWhaT1VaTDMzMk9aeEtwV3UvL3lzY3VkeQpQdEdSV0pFU0tLSEs5TFJhd2FDejlPRnMzZzFsQWdNQkFBR2pXVEJYTUE0R0ExVWREd0VCL3dRRUF3SUNwREFQCkJnTlZIUk1CQWY4RUJUQURBUUgvTUIwR0ExVWREZ1FXQkJUOTJ0dkUwVkpHSlBJdElNN1BYZmZpRkNRNndqQVYKQmdOVkhSRUVEakFNZ2dwcmRXSmxjbTVsZEdWek1BMEdDU3FHU0liM0RRRUJDd1VBQTRJQkFRQWdldm96Uml0TQo1NUcyNy9uOFRPVUJmdmRONmpPYVdSRk1MeXJzbINKSlptdHd3T3AzQ1E5K0loSWxhSHcvSExrSUcyWU1rOUhyCk9Bd3NYeUFJbHRLaUdzakhnTUx6cVpGSDRycmRRU21zeTh6a0taS2hJZ2hrUW1Dbjd2b0k3SnJyRThYb3A2cVcKK3dLK2dwR2toL3JSd0VpWU5jeHZkN1B1cUNrcFdTK3dFZ1BPNGh4bE9hUzdyWUNaKy9WWFAxc2dScDBTaGRLKwpwaERCc1dBTEREanFLOS90REdxcnJ6dzVWYVl1U1o2alJOVlM0TzV2UFhJeGNGZmh1c0tNZk5sZDYyb0tqSlMrClZKdEw1aEdLVm1DODNJdURabFhOdTNVMDNyeGN5NlFYVHo3Lzh6dll1WFRqck5GZTd5eFZBbi9PNFpvNmFocjcKdGRuNkRFRzNnT2NGCi0tLS0tRU5EIENFUlRJRklDQVRFLS0tLS0K"
}

variable "host" {
  description = "Endpoint do cluster EKS"
  default = "https://419F9FEDB99AEA42B16464860A4ECDA8.gr7.us-east-1.eks.amazonaws.com"
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