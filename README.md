# Sistema de Ponto

## Premissa

O "Sistema de Ponto" é uma aplicação para gerenciar colaboradores, incluindo funcionalidades como cadastro, registro de ponto e geração de relatórios mensais. O projeto utiliza ASP .NET Core para a API, Flutter Web para o front-end, e SQL Server como banco de dados.

*O que é preciso ter instalado para rodar o projeto ?*

- Visual Studio 2019
- .Net Core 
- SQL Server
- Flutter WEB
## Configuração do Projeto

  #### A arquitetura utilizada back-end nessa aplicação foi a Multipropósito , que conta com as camadas de:

  * Domain -  Contém as entidades e a lógica de negócio principal.
  * Data - Contém as interfaces dos repositórios e as implementações específicas para cada fonte de dados
  * Application - Contém os serviços de aplicação que utilizam os repositórios para implementar os casos de uso.
* API - Recebe as requisições, chama os serviços de aplicação e retorna as respostas.


#### Dependências:

Application depende de Domain e das interfaces dos repositórios definidas em Data.
Data depende de Domain e das bibliotecas específicas para acesso aos dados.
API depende de Application.

  #### A arquitetura utilizada frot-end nessa aplicação foi a clean arctecture com GETX pattern, que conta com as camadas de:

 * Apresentação: Contém os widgets da interface do usuário e a lógica de apresentação. Utiliza o GetX para gerenciar o estado da aplicação e as interações do usuário. Responsável por exibir os dados e capturar as ações do usuário, delegando o processamento para a camada de domínio.

 * Domain: Contém a lógica de negócio principal da aplicação, independente da interface do usuário. Define as entidades (modelos de dados) e os casos de uso (use cases), que encapsulam as regras e o comportamento do sistema. Interage com a camada de dados para acessar e persistir os dados.

 * Data : Responsável por acessar e gerenciar os dados da aplicação, seja de fontes locais (como armazenamento em cache) ou remotas (como a API back-end). Implementa os repositórios, que fornecem uma interface abstrata para acessar os dados, ocultando os detalhes de implementação da camada de domínio.




## Rodando o Projeto

Para iniciar o projeto com sucesso, siga os passos abaixo:



### 1. Download da Worktree

    git clone https://github.com/elysonsarmento/ChallengeAcoCearense


### 2. Definir Conexão com o Banco de Dados

1. Abra o arquivo `appsettings.json`.
2. Localize o campo `"ConnectionStrings": {"DataBase":"`.
3. Insira a string de conexão com o banco de dados SQL Server, incluindo o diretório e as credenciais de acesso para garantir as permissões necessárias.

### 3. Executar a Migration

1. Acesse o "Console de Gerenciador de Pacotes".
2. Execute os seguintes comandos:

```bash
Add-Migration initialDB
Update-Database
```

### 3. Rode o projeto Back-End
1. Abra o projeto no Visual Studio 2019.
2. Selecione o projeto da API ASP.NET Core como projeto de inicialização.
3. Pressione F5 ou clique no botão "Iniciar" na barra de ferramentas do Visual Studio.
O projeto será compilado e executado, e a API estará disponível em um endereço local (geralmente https://localhost:porta).
4. O Swagger será aberto automaticamente no navegador padrão no endereço https://localhost:porta/swagger.

### 4. Rode o projeto Front-End
1. Certifique-se de ter o Flutter SDK instalado e configurado em sua máquina.
Aprenda a baixar aqui: https://docs.flutter.dev/get-started/install/windows/web

2. Abra um terminal ou prompt de comando e navegue até o diretório do projeto Flutter Web.

3. Execute o seguinte comando para instalar as dependências do projeto:

```bash
flutter pub get
```

4. Execute o seguinte comando para iniciar o projeto em modo de desenvolvimento para web:

```bash
flutter run -d chrome 
```

5. O aplicativo Flutter Web será aberto automaticamente em seu navegador padrão (geralmente Chrome) no endereço http://localhost:porta.





# Referências

- [Joe Birch - Google GDE: Clean Architecture Course](https://caster.io/courses/android-clean-architecture)
- [Reso Coder - Flutter TDD Clean Architecture](https://www.youtube.com/playlist?list=PLB6lc7nQ1n4iYGE_khpXRdJkJEp9WOech)
- [Multiple Repositories inside a Single Solution for .NET](https://devblogs.microsoft.com/ise/dotnet-multi-repo/)
- [Guide to app architecture - Jetpack Guides](https://developer.android.com/jetpack/docs/guide#common-principles)
- [Start building Flutter web apps on Windows](https://docs.flutter.dev/get-started/install/windows/web)
