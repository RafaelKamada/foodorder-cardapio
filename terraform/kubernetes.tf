resource "kubernetes_deployment" "api_cardapio" {
  metadata {
    name = "api-cardapio-deployment"
    labels = {
      app = "api-cardapio"
    }
  }

  spec {
    replicas = 2

    selector {
      match_labels = {
        app = "api-cardapio"
      }
    }
 
    template {
      metadata {
        labels = {
          app = "api-cardapio"
        }
      }

      spec {
        container {
          name  = "api-cardapio-container"
          image = "japamanoel/foodorder_cardapio:latest"

          port {
            container_port = 9001
          }

          env {
            name  = "ASPNETCORE_URLS"
            value = "http://0.0.0.0:9001"
          }

          env {
            name = "MONGODB_CONNECTION"
            value = "mongodb://${var.mongodb_endpoint}:${var.mongodb_port}/${var.mongodb_database}"
          }
        }
      }
    }
  }
}

resource "kubernetes_service" "api_cardapio" {
  metadata {
    name = "api-cardapio-svc"
    labels = {
      app = "api-cardapio"
    }
  }

  spec {
    selector = {
      app = "api-cardapio"
    }

    port {
      port        = 80
      target_port = 9001
      node_port   = 30081
    }

    type = "LoadBalancer"
  }

  depends_on = [
    kubernetes_deployment.api_cardapio
  ]
}