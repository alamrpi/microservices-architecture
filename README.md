# 🛒 E-Shop Global - Microservices E-Commerce Platform

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat&logo=dotnet)
![React](https://img.shields.io/badge/React-18-61DAFB?style=flat&logo=react)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?style=flat&logo=docker)
![Kubernetes](https://img.shields.io/badge/Kubernetes-K8s-326CE5?style=flat&logo=kubernetes)
![Build Status](https://img.shields.io/badge/Build-Passing-brightgreen)

**E-Shop Global** is a full-stack, cloud-native e-commerce application built using **Event-Driven Microservices Architecture**. It leverages modern technologies like **.NET 8**, **React**, **RabbitMQ**, **gRPC**, and **YARP** to build a scalable and resilient system.

---

## 🏗️ Architecture Overview

The application follows the **Polyglot Persistence** approach and implements advanced patterns like **CQRS**, **DDD**, and **Vertical Slice Architecture**.

### 🧩 Microservices Breakdown

| Service | Technology Stack | Key Patterns & Features |
| :--- | :--- | :--- |
| **🛒 Catalog API** | .NET 9, PostgreSQL, EF Core | **Vertical Slice Architecture**, CQRS (MediatR), Minimal APIs. |
| **🧺 Basket API** | .NET 9, Redis, Marten (Doc DB) | **Cache-Aside Pattern**, Distributed Caching, **gRPC Client** for discounts. |
| **🏷️ Discount API** | .NET 9, gRPC | High-performance inter-service communication using **gRPC**. |
| **📦 Ordering API** | .NET 9, SQL Server, EF Core | **Clean Architecture**, **DDD** (Aggregates, Value Objects), MassTransit Consumer. |
| **🚪 API Gateway** | YARP (Reverse Proxy) | **Rate Limiting**, Route Transformation, SSL Termination. |
| **⚛️ Client App** | React (Vite), Bootstrap | SPA, Dockerized Development Environment. |

### 📡 Communication Style
* **Synchronous:** `Basket.API` communicates with `Discount.API` using **gRPC** for real-time price calculation.
* **Asynchronous:** `Basket.API` publishes checkout events to **RabbitMQ** (via MassTransit), which are consumed by `Ordering.API` for eventual consistency.

---

## 🚀 Getting Started (How to Run)

You can run the entire system (Frontend + Backend + Databases) with a single command using **Docker Compose**.

### Prerequisites
* [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running.

### Steps
1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/alamrpi/microservices-architecture.git](https://github.com/alamrpi/microservices-architecture.git)
    cd microservices-architecture
    ```

2.  **Run the application:**
    ```bash
    docker-compose up -d --build
    ```
    *(This will build the .NET images, the React image, and spin up Postgres, SQL Server, Redis, and RabbitMQ containers.)*

3.  **Access the Application:**

    | Service | URL | Description |
    | :--- | :--- | :--- |
    | **Frontend UI** | `http://localhost:5173` | The React Application (Storefront) |
    | **API Gateway** | `http://localhost:5000` | Single entry point for all APIs |
    | **RabbitMQ UI** | `http://localhost:15672` | User: `guest`, Pass: `guest` |
    | **Catalog Swagger** | `http://localhost:8080/swagger` | Direct access (if port mapped) |

---

## 🛠️ Technical Highlights & Skills Demonstrated

This project serves as a practical implementation of advanced software engineering concepts:

* **Design Patterns:** CQRS, Mediator, Proxy, Decorator, Repository, Unit of Work.
* **Architecture:** Clean Architecture (Onion), Vertical Slice, Event-Driven Architecture.
* **Domain-Driven Design (DDD):** Rich Domain Models, Aggregates, Domain Events, Value Objects.
* **DevOps:**
    * **Docker:** Multi-stage builds for optimized images.
    * **CI/CD:** GitHub Actions pipelines for automated testing and pushing images to Docker Hub.
    * **Kubernetes:** Manifest files (`Deployment`, `Service`) for orchestration.
* **Observability:** Structured Logging using **Serilog**, Health Checks.
* **Security:** Rate Limiting using .NET 8 Middleware.

---

## 📂 Project Structure

```text
E-Shop-Global/
├── src/
│   ├── ApiGateway/         # YARP Reverse Proxy
│   ├── client/             # React Frontend (Vite)
│   ├── Services/
│   │   ├── Basket.API/     # Basket Microservice
│   │   ├── Catalog.API/    # Catalog Microservice
│   │   ├── Discount.API/   # gRPC Service
│   │   ├── Ordering/       # Ordering Service (Clean Arch)
│   │       ├── Domain/
│   │       ├── Application/
│   │       ├── Infrastructure/
│   │       ├── API/
├── k8s/                    # Kubernetes Manifests
├── docker-compose.yml      # Docker Orchestration
└── .github/workflows/      # CI/CD Pipelines

