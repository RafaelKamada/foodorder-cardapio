provider "aws" {
  region = var.regionDefault
}

provider "kubernetes" {
  host = var.host
  cluster_ca_certificate = var.cluster_ca_certificate
  exec {
    api_version = "client.authentication.k8s.io/v1beta1"
    args = ["eks", "get-token", "--cluster-name", var.projectName]
    command = "aws"
  }
}