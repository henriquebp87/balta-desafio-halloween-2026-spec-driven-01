# Plano técnico

## Contexto

- Implementa a especificação do gerador de senhas fortes, respeitando a constituição do projeto.

## Arquitetura

- API HTTP em Minimal API, um arquivo de endpoints por recurso.
- Endpoint só valida entrada, chama o serviço e traduz o resultado em resposta.
- Validações e regras de negócio ficam centralizadas na camada de serviços.
- Camada de persistência em EF Core com SQLite.

## Decisões

- D-01: SQLite como armazenamento, por ser leve e não requerer configuração.
- D-02: EF Core para ORM, com migrations para evolução do schema.
- D-03: GUIDs devem ser gerados pelo sistema utilizando `Guid.NewGuid()`, não pelo usuário.
- D-04: Senhas devem ser geradas pelo sistema utilizando `Random.NextBytes()`, não pelo usuário.

## Modelo de dados

- Entidade `Password` com os campos: `Id` (GUID), `Guid` (GUID), `Password` (string), `CreatedAt` (DateTime).

## Contratos

- POST `/api/v1/password`:
  - Recebe: `length` (int).
  - Retorna: `guid` e `password`.

- GET `/api/v1/password/{guid}`:
  - Recebe: `guid`.
  - Retorna: `guid` e `password`.

## Riscos

- Crescimento da tabela de senhas sem política de limpeza periódica. Fora do escopo neste momento.
- Senhas armazenadas e retornadas em plain text. Fora do escopo neste momento.