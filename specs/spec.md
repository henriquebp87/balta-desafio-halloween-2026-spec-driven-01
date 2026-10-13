# Gerador de Senhas Fortes

## Problema

- Senhas fracas ainda são muito comuns e facilitam ataques de força bruta e adivinhação.

## Objetivo

- Implementar um gerador de senhas fortes que seguem um padrão pré-definido.

## Usuários

- Criador: Usuário que precisa gerar senhas fortes.
- Usuário: De posse de um GUID, pode recuperar a senha gerada.

## Histórias

- Como criador, quero poder gerar uma senha forte que siga um padrão pré-definido.
- Como usuário, quero poder recuperar a senha gerada de posse de um GUID.

## Requisitos funcionais

- RF-01: Toda senha gerada **deve ter pelo menos um caracter especial**.
- RF-02: Toda senha gerada **deve ter pelo menos 16 caracteres**.
- RF-03: Toda senha gerada **deve ter no máximo 64 caracteres**.
- RF-03: Toda senha gerada **não pode possuir espaços em branco**.
- RF-04: Toda senha gerada é armazenada no banco de dados.
- RF-05: Toda senha gerada retorna um GUID para ser encontrada posteriormente.
- RF-06: Ao solicitar uma senha pelo GUID, o pesquisador deve receber a senha gerada e o GUID.

## Regras de negócio

- RN-01: Toda senha gerada deve seguir um padrão pré-definido.
- RN-02: A senha gerada é composta por caracteres especiais, números e letras.
- RN-03: O GUID é gerado automaticamente pelo sistema.
- RN-04: A senha é armazenada de forma **não criptografada** no banco de dados.
- RN-05: O criador define o tamanho da senha, respeitando o requisito de ter pelo menos 16 caracteres.
- RN-06: Ao tentar criar uma senha com menos de 16 caracteres, deve retornar HTTP 400 (Bad Request) e uma mensagem de erro amigável.
- RN-07: Ao tentar criar uma senha com mais de 64 caracteres, deve retornar HTTP 400 (Bad Request) e uma mensagem de erro amigável.
- RN-08: Ao pesquisar por um GUID inexistente, deve retornar HTTP 404 (Not Found) e uma mensagem de erro amigável.
- RN-09: Ao pesquisar por um GUID existente, deve retornar HTTP 200 (OK) e o GUID juntamente com a senha gerada.

## Casos de borda

- Erro ao persistir a senha gerada no banco: Retornar HTTP 500 (Internal Server Error) e uma mensagem de erro amigável ao usuário.
- Erro ao gerar GUID: Retornar HTTP 500 (Internal Server Error) e uma mensagem de erro amigável ao usuário.
- Erro ao gerar senha: Retornar HTTP 500 (Internal Server Error) e uma mensagem de erro amigável ao usuário.

## Fora de escopo

- Cadastro e login de usuários.
- Criptografia e outros recursos de segurança.
- Edição de senha já gerada.

## Critérios de aceite

- Gerar uma senha com tamanho 16, composta por letras, números e caracteres especiais.
- Ao pesquisar por um GUID existente, retornar o GUID e a senha gerada.