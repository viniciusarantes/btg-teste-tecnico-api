# README

## Teste Técnico - Desenvolvedor Backend

### Introdução

Esse repositório contém uma aplicação API REST para registros de usuários. A API foi desenvolvida em .NET 8 utilizando SQLite como banco de dados.
A proposta da API é registrar, consultar, editar e deletar um usuário na base de dados. Como implementação de performance, também foi utilizado
Memory Cache para armazenar dados já consultados e evitar diversas chamadas ao banco de dados que retornarão o mesmo resultado, apenas removendo 
o cache após a alteração, exclusão e adição de um novo registro.

### Tecnologias utilizadas
- .NET 8
- SQLite (Banco de dados local)
- MemoryCache (Padrão Cache para performance)
- xUnit e Moq (Testes unitários)
- FluentAssertion (Facilita o entendimento da escrita dos testes)
---
### Setup da Aplicação

Certifique que possua o SDK e o runtime do .NET 8 instalado.

1. Clone o repositório na sua máquina

```
git clone https://github.com/viniciusarantes/btg-teste-tecnico-api.git
cd btg-teste-tecnico
```

2. No diretório da aplicação, execute o projeto

```
dotnet run --project ApiUsuarioKRT/ApiUsuariosKRT.csproj
```
A API ficará disponível em `http://localhost:5266/swagger`

---

### Execução dos testes
1. Para executar os testes:

```
dotnet test ApiUsuarioKRT.Tests/ApiUsuarioKRT.Tests.csproj
```
---
### API Endpoints

- `GET /api/Usuario` - Lista os usuários cadastrados
- `GET /api/Usuario/<id>` - Consulta um usuário pelo id
- `POST /api/Usuario` - Registra um novo usuário
- `PATCH /api/Usuario/<id>` - Atualiza um registro de usuário pelo id
- `DELETE /api/Usuario/<id>` - Remove um usuário pelo id
