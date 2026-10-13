# Tarefas

| ID | Tarefa | Origem | Depende de | Concluída quando |
|---|---|---|---|---|
| T-01 | Criar projeto e estrutura de pastas | Constituição | | Projeto compila |
| T-02 | Criar migration do banco de dados | D-01, D-02 | T-01 | Entity e banco criados |
| T-03 | Criar endpoint para criação de senhas | RF-01 até RF-05, RN-01 até RN-07 | T-02 | Endpoint funciona |
| T-04 | Criar endpoint para recuperação de senhas | RF-06, RN-08, RN-09 | T-02, T-03 | Endpoint funciona |
| T-05 | Testar endpoints | Plano | T-03, T-04 | Testes passam |
