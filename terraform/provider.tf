provider "aws" {
  region = var.regionDefault
}

provider "kubernetes" {
  host = var.host
  exec {
    api_version = "client.authentication.k8s.io/v1beta1"
    args = ["eks", "get-token", "--cluster-name", var.projectName]
    command = "aws"
  }
}