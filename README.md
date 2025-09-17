# 📚 auniv -  API de Universidades de Angola

**API em .NET Core que lista universidades de Angola com seus departamentos e cursos.**

[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![.NET](https://img.shields.io/badge/.NET-9-512BD4)](https://dotnet.microsoft.com/)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1)](https://www.mysql.com/)
---

## 📖 Descrição

Angola possui um sistema de ensino superior em expansão, com diversas instituições públicas e privadas espalhadas pelo território nacional. No entanto, estudantes angolanos enfrentam desafios, como:

- **Centralização de informação** → Dados sobre universidades, departamentos e cursos estão dispersos em múltiplas fontes.  
- **Falta de padronização** → Informações em formatos diferentes, dificultando comparações.  
- **Acesso limitado** → Muitas instituições ainda não têm presença digital robusta.  
- **Actualização irregular** → Informações sobre cursos, admissões e requisitos desactualizadas.  
- **Tomada de decisão difícil** → Futuros estudantes têm dificuldades em comparar instituições e cursos.  

O **auniv** busca resolver esses problemas oferecendo uma **API centralizada, padronizada e actualizada** com dados do ensino superior angolano.

---

## 🛠️ Tecnologias usadas

- **.NET 9**  
- **ASP.NET Core**  
- **Entity Framework Core (EF Core)**  
- **MySQL (via Pomelo.EntityFrameworkCore.MySql)**  
- **OpenAPI / Scalar para documentação da API**  
- **Visual Studio Code** (desenvolvimento actual, iniciado no Rider)

---

## 🚀 Como instalar e rodar
1. Clone o repositório:

    ```bash
    git clone https://github.com/Ddacosta-akirfa/auniv.git
    cd auniv
2. Restaure as dependências

    ```bash
    dotnet restore
2. Configure a string de conexão no **appsettings.json** para o seu banco de dados MySQL.
    ```json
    "ConnectionStrings": {         "DefaultConnection":"server=localhost;port=tua_porta;database=aunivdb;user=teu_usuario;password=tua_senhaa" }
2. Execute as migrações do banco de dados

    ```bash
    dotnet ef database update
2. Rode a aplicação

    ```bash
    dotnet run
2. Acesse a documentação da API em:

    ```bash
    https://localhost:7164/scalar

## 📁 Estrutura do Projecto
O projecto tem a seguinte estrutura:
``` 
auniv/
├── .idea/ # Configurações do JetBrains Rider/IDE
├── Auniv/ # Código-fonte principal da API (.NET Core)
│ ├── Controllers/ # Controladores da API (recebem requisições HTTP)
│ ├── Data/ # Contexto do banco de dados
│ │── Migrations/ # Migrações do Entity Framework Core
│ ├── Enums/ # Enumerações utilizadas no projeto
│ ├── Models/ # Modelos de dados e DTOs
│ │ ├── Dtos/ # Objetos de transferência de dados
│ │ └── Validacoes/ # Classes para validação de dados
│ ├── Properties/ # Configurações do projecto
│ ├── Routes/ # Definição de endpoints e rotas
│ ├── program.cs # Arquivo principal que inicializa a aplicação
│ └── auniv.http # Coleção de requisições HTTP (ex.: para teste com VS Code REST Client)
├── .gitignore # Arquivo que define o que será ignorado pelo Git
├── Auniv.sln # Arquivo de solução do Visual Studio (.sln)
├── LICENSE # Licença do projeto (GPL v3)
└── README.md # Documentação principal do projeto
```


## __📌 Funcionalidades principais__

### 🏢 Gestão de Universidades
* Listar todas as universidades - Retorna todas as instituições com informações completas
* Buscar universidade por ID - Obtém detalhes específicos de uma universidade
* Buscar universidade por sigla - Encontra uma universidade através da sua sigla institucional
* Buscar universidades por localização - Filtra universidades por província (com validação de províncias angolanas)
* Listar departamentos de uma universidade - Retorna todos os departamentos de uma instituição específica
* Criar nova universidade - Adiciona uma nova universidade com validações de sigla e localização
* Actualizar universidade (por ID ou sigla) - Modifica informações de uma universidade existente
* Excluir universidade (por ID ou sigla) - Remove uma universidade do sistema

### 🏫 Gestão de Departamentos
* Listar todos os departamentos - Retorna todos os departamentos com informações da universidade e cursos associados
* Buscar departamento por ID - Obtém detalhes específicos de um departamento
* Buscar departamentos por universidade (via ID ou sigla) - Lista departamentos de uma instituição específica
* Criar novo departamento - Adiciona um novo departamento com validações de universidade e duplicidade
* Actualizar departamento - Modifica informações de um departamento existente
* Excluir departamento - Remove um departamento do sistema

### 🎓 Gestão de Cursos
* Listar todos os cursos - Retorna todos os cursos disponíveis no sistema
* Buscar curso por ID - Obtém detalhes específicos de um curso através do seu identificador
* Excluir curso - Remove um curso do sistema (com validação de existência)

### ✅ Validações e Características Específicas
* Validação de províncias angolanas - Garante que apenas províncias válidas de Angola sejam utilizadas
* Controle de duplicidades - Impede a criação de universidades com siglas repetidas ou departamentos com nomes duplicados na mesma universidade
* Respostas padronizadas - Utiliza DTOs específicos para diferentes operações
* Tratamento de erros - Códigos de erro descritivos para diferentes cenários de falha


### 📅 Próximos passos

* 📊 Criar estatísticas de universidades (ex.: número de cursos, vagas)
* 🌍 Internacionalização (tradução para inglês/francês)
* 🛡️ Autenticação e autorização de usuários

### 👥 Como Contribuir
1. Faça um Fork do projecto
2. Crie uma branch para sua feature (git checkout -b feature/matriculas)
3. Commit suas mudanças (git commit -m 'Add endpoint matriculas')
4. Push para a branch (git push origin feature/matriculas)
5. Abra um Pull Request

### 📜 Licença

Este projeto está licenciado sob a **GPL v3**. Consulte o arquivo [LICENSE](./LICENSE)
 para mais detalhes.

### ✍️ Créditos

Desenvolvido por **Daniel Dacosta**.