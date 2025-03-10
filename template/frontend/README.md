# Enterprise Angular Application

## Project Overview

This project follows enterprise-level architecture patterns for Angular applications. It is structured to promote maintainability, scalability, and follows the principles of Clean Architecture and Domain-Driven Design.

## Arquitetura em Camadas (Layered Architecture)

Este projeto implementa uma arquitetura em camadas (Layered Architecture) seguindo os princípios do Domain-Driven Design (DDD) e Clean Architecture para garantir manutenibilidade, testabilidade e separação de responsabilidades.

### Camadas da Aplicação

1. **Camada de Apresentação (Presentation Layer)**
   - Componentes Angular
   - Rotas e navegação
   - Formulários e validação

2. **Camada de Domínio (Domain Layer)**
   - Serviços de domínio com lógica de negócio
   - Modelos de domínio (entities)
   - Gestão de estado da aplicação

3. **Camada de Dados (Data Layer)**
   - Serviços de API
   - DTOs (Data Transfer Objects)
   - Interceptadores HTTP

4. **Camada de Infraestrutura (Infrastructure Layer)**
   - Configuração da aplicação
   - Serviços de sistema (cache, storage, etc.)
   - Utilitários e helpers

### Fluxo de Dados

```
UI Components <-> Domain Services <-> API Services <-> HTTP Client <-> Backend API
```

## Padrões de Design Implementados

### Repository Pattern (via API Services)
Os serviços de API funcionam como repositories, abstraindo a fonte de dados e fornecendo uma interface consistente para acesso.

### Mapper Pattern
Conversão entre modelos de API (DTOs) e modelos de domínio é feita através de funções de mapeamento dedicadas.

### Dependency Injection
Serviços são injetados onde necessário seguindo o princípio de inversão de dependência.

### Observer Pattern (via RxJS)
Fluxos de dados assíncronos são gerenciados usando Observables do RxJS.

### Facade Pattern
Serviços de domínio encapsulam a complexidade das operações e fornecem uma interface simplificada.

## Project Structure

```
src/
├── app/
│   ├── core/                  # Core module for singleton services
│   │   ├── api/               # API communication layer
│   │   │   ├── config/        # API configuration
│   │   │   ├── models/        # API request/response models (DTOs)
│   │   │   ├── services/      # API services (repositories)
│   │   │   └── index.ts       # Barrel file for exports
│   │   ├── guards/            # Route guards
│   │   ├── interceptors/      # HTTP interceptors
│   │   ├── models/            # Shared data models
│   │   ├── services/          # Core services
│   │   └── core.module.ts     # Core module definition
│   ├── features/              # Feature modules
│   │   └── users/             # Example feature module
│   │       ├── components/    # Feature components
│   │       ├── models/        # Feature-specific models
│   │       ├── services/      # Feature-specific domain services
│   │       └── users.module.ts # Feature module definition
│   ├── shared/                # Shared module for common components
│   │   ├── components/        # Shared components
│   │   ├── directives/        # Custom directives
│   │   ├── pipes/             # Custom pipes
│   │   └── shared.module.ts   # Shared module definition
│   ├── app.component.ts       # Root component
│   ├── app.module.ts          # Root module
│   └── app-routing.module.ts  # Root routing module
├── assets/                    # Static assets
│   ├── images/
│   └── styles/
├── environments/              # Environment configuration
│   ├── environment.ts
│   └── environment.prod.ts
└── main.ts                    # Application entry point
```

## Core Features

### API Layer Architecture

A aplicação implementa uma arquitetura de API limpa e organizada:

1. **Request/Response Models (DTOs)**
   - Interfaces tipadas para comunicação com a API
   - Organização em namespaces (Auth, Users, etc.)
   - Separação clara entre DTOs e modelos de domínio

2. **API Services (Repositories)**
   - Serviço base com funcionalidade HTTP comum
   - Serviços específicos por domínio
   - Tratamento centralizado de erros e lógica de retry

3. **Configuration**
   - Configuração centralizada de endpoints da API
   - Configurações específicas por ambiente
   - Constantes para tipos de conteúdo e headers

4. **Domain Services**
   - Implementam a lógica de negócio
   - Gerenciam o estado e cache
   - Realizam mapeamento entre DTOs e modelos de domínio

### Interceptors

1. **AuthInterceptor**
   - Adiciona headers de autorização automaticamente às requisições HTTP
   - Injeta tokens bearer para requisições autenticadas
   - Gerencia o refresh do token quando expirado

2. **ErrorInterceptor**
   - Fornece tratamento centralizado de erros para requisições HTTP
   - Trata diferentes tipos de erro (4xx, 5xx) de forma consistente
   - Gerencia erros de autenticação (401, 403)
   - Mostra notificações amigáveis usando componentes Material

3. **LoadingInterceptor**
   - Mostra automaticamente um indicador de carregamento durante requisições HTTP
   - Rastreia requisições em andamento com um contador
   - Barra de progresso no topo para feedback visual

### Services

1. **ApiBaseService**
   - Serviço base para comunicações HTTP
   - Padroniza chamadas à API com tipagem adequada
   - Implementa retries e timeouts para requisições
   - Gerencia preparação e transformação de headers

2. **AuthService**
   - Gerencia autenticação de usuários
   - Fornece login, logout e estado de autenticação
   - Armazena tokens de usuário com segurança
   - Gerencia expiração e refresh de tokens

### Guards

1. **AuthGuard**
   - Protege rotas contra acesso não autorizado
   - Redireciona para login quando necessário
   - Trabalha em conjunto com o AuthService

### Models

1. **Domain Models**
   - Entidades de negócio principais
   - Independentes das representações da API
   - Usados em toda a aplicação

2. **Request/Response Models**
   - Representam estruturas de comunicação com a API
   - Fortemente tipados para verificação em tempo de compilação
   - Organizados em namespaces por recurso

## Getting Started

1. Clone o repositório
2. Instale as dependências:
   ```bash
   npm install
   ```
3. Inicie o servidor de desenvolvimento:
   ```bash
   npm start
   ```

## Development Guidelines

1. **Architecture**
   - Siga a estrutura de pastas estabelecida
   - Crie módulos de features para domínios separados
   - Use o módulo compartilhado para componentes comuns
   - Mantenha a comunicação com API na camada api

2. **Code Style**
   - Use interfaces TypeScript para todos os modelos de dados
   - Documente o código com comentários JSDoc
   - Siga o guia de estilo Angular

3. **Dependency Injection**
   - Injete dependências através de construtores
   - Use a propriedade providedIn quando possível

4. **Error Handling**
   - Use o ErrorInterceptor para erros HTTP
   - Forneça mensagens de erro significativas

## Best Practices

1. **State Management**
   - Use serviços com RxJS para estado simples
   - Considere NgRx para gerenciamento de estado complexo

2. **Performance**
   - Carregue módulos de feature de forma lazy
   - Use detecção de mudanças OnPush para componentes
   - Implemente padrões de unsubscribe adequados

3. **Testing**
   - Escreva testes unitários para serviços e componentes
   - Use TestBed para testes Angular
   - Faça mock de dependências adequadamente

4. **API Communication**
   - Use modelos tipados para requests/responses
   - Trate erros de forma consistente
   - Transforme respostas da API em modelos de domínio
