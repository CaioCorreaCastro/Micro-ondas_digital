# Micro-ondas Digital

Descrição: Projeto de um micro-ondas digital desenvolvido em C#, com interface desktop, regras de negócio separadas em camadas e integração com Web API.

Linguagens / Frameworks / Tecnologias:
- Linguagem: C#
- Framework: .NET 8 (projetos modernos) e .NET Framework 4.7.2 (legado)
- Ferramentas: Visual Studio

Como instalar e usar

Pré-requisitos:
- Git
- Visual Studio (recomendado) com suporte a .NET 8 e .NET Framework 4.7.2
- .NET SDK 8 (se desejar compilar/rodar via linha de comando)

Passos:
1. Clone o repositório:

   git clone https://github.com/CaioCorreaCastro/Micro-ondas_digital.git

2. Abra a solução no Visual Studio e execute (F5) ou, para projetos .NET 8, use a linha de comando dentro da pasta do projeto:

   dotnet build
   dotnet run

Uso:
- A interface permite configurar tempo e potência. Inicie o ciclo e acompanhe o temporizador.

Sugestão: perfil de inicialização (rodar API e Interface juntos)

Se desejar executar a API e a interface ao mesmo tempo durante o desenvolvimento, crie um perfil de inicialização no Visual Studio:

1. No Visual Studio, vá em Solution Explorer e clique com o botão direito na solução (nível mais alto).
2. Selecione "Set Startup Projects..." (Definir projetos de inicialização).
3. Marque "Multiple startup projects" e defina a ação "Start" para os projetos da API e da Interface.
4. Salve. Agora ao executar a solução (F5) ambos os projetos iniciarão juntos.

Observação

This is a challenge by [Coodesh](https://coodesh.com/)
