# Order-Tracking-System

Microservices with gRPC communication

## Customer workflow ##

- Creates an account
- Chooses the order
- Waiting for courier to be assigned by admin

## Courier workflow ##

- Creates an account
- Chooses order to deliver
- Removes order from delivery list

# Microservice summary #

| Info         | Application microservice | Identity microservice |
| :----:       |          :----:          |         :----:        |
| Architecture |Clean with vertical slices|         N-Layer       |
| Domain model |        Rich domain       |       Anemic domain   |
| Communication|external[^1] + internal[^2]|        internal[^2]  |     

## Technology stack ##

**Application microservice**

- .NET 7
- Entity Framework
- PostgreSQL
- MediatR + ResultType
- FastEndpoints
- FluentValidation
- FluentAssertions
- Bogus
- Moq
- VaultSharp
- Respawn + TestContainers


**Identity microservice**

- TBD

[^1]: External communication stands for interaction with client-side
[^2]: Internal communication stands for interaction within project ecosystem
