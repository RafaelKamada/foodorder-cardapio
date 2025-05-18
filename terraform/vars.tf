variable "regionDefault" {
  default = "us-east-1"
}

variable "projectName" {
  default = "EKS-FOOD-ORDER-API"
}

variable "cluster_ca_certificate" {
  description = "Certificado do cluster EKS"
  default = "-----BEGIN CERTIFICATE-----\nMIIDBjCCAe2gAwIBAgIUbGdXDB0YkVUwRFFZSktvWklodmNOQUFFTEJRQXdGVEVUTUJFR0ExVUUK\nQXhNS2EzVmlaWEp1WlhSbGN6QWVGdzB5TlRBMU1UZ3lNREkyTWpOYUZ3MHpOVEExTVRZeU1ETXhN\nak5hTUJVeEF6RUFNQmdOVkJBTVRDbXQxWW1WeWJtVjBaWE13Z2dFaU1BMEdDU3FHU0liM0RRRUJB\nQVVBQTRJQkR3QXdnZ0VLQm9JQkFRR0RVdDZaR2M2V0pHcWxHYjY5WUJpa2FHQVVpdkxRbW4yK1Zh\nOGczeURGUGxGaXM3SXE3OVo4Snd5bgpQMDZvdDA5c1d2cEFyUXJYQ291NllCOXZydHpxc0RWbW96\nN0pNd0thMjcyd3RzYTNXenFxZUJqNGNMQ1RCWnArClE3Y2psUmpqWDVPUjdiZFdnenJPRXppZmJG\nZmFBbFNuM0hrUDlJd2plNUxxQW9kQjZDOTd3azJwbXhEZ25FZ08KM2s0dkNYeUhpbERkSkkzMHZG\nR3o4U283S0hPR3pXYWJJL3FZZitlWWhaT1VaTDMzMk9aeEtwV3UvL3lzY3VkeQpRdEdSV0pFU0tL\nSEs5TFJhd2FDejlPRnMzZzFsQWdNQkFBR2pXVEJYTUE0R0ExVWREZ0FXQkJUOTJ0dkUwVkpHSlBJ\ndElNN1BYZmZpRkNRNndqQVYKQmdOVkhSRUVEakFNZ2dwcmRXSmxjbTVsZEdWek1BMEdDU3FHU0li\nM0RRRUJDd1VBQTRJQkFRQWdldm96Uml0TQo1NUcyNy9uOFRPVUJmdmRONmpPYVdSRk1MeXJzbINK\nSlptdHd3T3AzQ1E5K0loSWxhSHcvSExrSUcyWU1rOUhyCk9Bd3NYeUFJbHRLaUdzakhnTUx6cVpG\nSDRycmRRU21zeTh6a0taS2hJZ2hrUW1Dbjd2b0k3SnJyRThYb3A2cVcKK3dLK2dwR2toL3JSd0Vp\nWU5jeHZkN1B1cUNrcFdTK3dFZ1BPNGh4bE9hUzdyWUNaKy9WWFAxc2dScDBTaGRLKwpwaERCc1dB\nTEREanFLOS90REdxcnJ6dzVWYVl1U1o2alJOVlM0TzV2UFhJeGNGZmh1c0tNZk5sZDYyb0tqSlMr\nClZKdEw1aEdLVm1DODNJdURabFhOdTNVMDNyeGN5NlFYVHo3Lzh6dll1WFRqck5GZTd5eFZBbi9P\nNFpvNmFocjcKdGRuNkRFRzNnT2NGCi0tLS0tRU5EIENFUlRJRklDQVRFLS0tLS0K\n-----END CERTIFICATE-----\n"
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