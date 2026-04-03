\# 🏍️ Real-Time Courier Telemetry \& Logistics System



This repository presents a real-time logistics and courier tracking simulation, strictly architected upon \*\*Clean Architecture\*\* principles. Designed to integrate seamlessly into a microservices ecosystem, the system demonstrates advanced distributed data management, asynchronous geospatial telemetry, and event-driven communication.



\## 🚀 Architectural Highlights \& Core Features



\* \*\*Domain-Centric Design (Clean Architecture):\*\* Ensures high maintainability, testability, and strict separation of concerns by decoupling the core domain business logic from infrastructural dependencies and external frameworks.

\* \*\*Asynchronous Real-Time Telemetry (SignalR):\*\* Facilitates bi-directional, low-latency WebSocket communication, enabling instantaneous geospatial updates to connected clients without the overhead of HTTP polling.

\* \*\*Optimized Dual-Write Persistence Strategy:\*\* \* \*\*Relational Persistence (PostgreSQL):\*\* Utilized as the primary source of truth for immutable and historical records, including courier profiles and metadata.

&#x20; \* \*\*In-Memory Geospatial Caching (Redis):\*\* Employs Redis Geo-Spatial data structures to process high-throughput, ephemeral location vectors (latitude/longitude), drastically reducing database I/O load.

\* \*\*Autonomous Event-Driven Simulation:\*\* Integrates a robust `.NET BackgroundService` (Worker) that autonomously computes dynamic coordinate vectors and generates mock geospatial telemetry for active entities at defined temporal intervals.

\* \*\*Reactive Client Interface:\*\* A highly responsive, decoupled Single Page Application (SPA) built with Angular, utilizing RxJS for reactive state management and Leaflet.js for dynamic cartographic visualization.



\## 🛠️ Technology Stack



\*\*Backend (API \& Core Infrastructure):\*\*

\* .NET (C#) / ASP.NET Core Web API

\* Entity Framework Core (Code-First Approach)

\* PostgreSQL (Relational Database)

\* StackExchange.Redis (Distributed Cache \& Geo-Spatial Queries)

\* SignalR (Real-Time WebSocket Protocol)



\*\*Frontend (Client Application):\*\*

\* Angular (Standalone Components paradigm)

\* Tailwind CSS (Utility-first styling)

\* Leaflet.js (Interactive mapping)

\* RxJS (Reactive programming)



\*\*DevOps \& Tooling:\*\*

\* Docker \& Docker Compose (Containerization \& Orchestration)

\* Redis Insight / DBeaver (Database Administration)



\## 📂 Repository Structure



```text

📦 Logistics-Tracking-System

&#x20;┣ 📂 Backend           # .NET Clean Architecture Solution (Domain, Application, Infrastructure, Presentation)

&#x20;┣ 📂 Frontend          # Angular SPA (Interactive Dashboard \& Spatial Visualization)

&#x20;┣ 📜 docker-compose.yml # Container orchestration for PostgreSQL and Redis services

&#x20;┗ 📜 README.md

