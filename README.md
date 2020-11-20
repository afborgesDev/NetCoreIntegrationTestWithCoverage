
- [EN](#en)
- [PT-br](#pt-br)

---

## EN

**A practical example around code coverage for integration tests.**

This is an example about how to get code coverage from integration tests.  
In this case more specific using a messaging environment.  

Tech

- RabbitMQ
- .Net Core 3.1
- dotCover (To collect coverage) download [here](https://www.jetbrains.com/help/dotcover/Running_Coverage_Analysis_from_the_Command_LIne.html)
- 


Steps to reproduce:

1) Change the file [LocalExecute.ps1](.\LocalExecute.ps1) to reflect your folder structure.
2) provide a RabbitMQ running on LocalHost (or change the configuration on appsettings.json).
3) Configure the dotCover as a alias on your machine or change the reference for dotCover inside the script to match your DotCover executable.
4) Execute the LocalExecute.ps1


The expected result:
- A folder `coverage` with a file `report.html` should appear on the current folder.

*Execution Plan*
```
- dotnet restore .\EntryPointProcessorSolution.sln
- dotnet build .\EntryPoint.Processor\EntryPoint.Processor.csproj -c Debug
- dotnet build .\EntryPoint.Integration.Test\EntryPoint.Integration.Test.csproj
- dotCover start configuration
- dotCover start watch target application
- dotnet test .\EntryPoint.Integration.Test\ --no-build
- dotCover end the session and generate the report
```

---

## PT-br

**Exemplo pratico a respeito de cobertura de código em testes integrados**

Este é um exemplo sobre como coletar cobertura de código em testes integrados.
Olhando para um ambiente mais específico de mensageria.

Tecnologias 

- RabbitMQ
- .Net Core 3.1
- dotCover (Para coletar a cobertura) baixar [aqui](https://www.jetbrains.com/help/dotcover/Running_Coverage_Analysis_from_the_Command_LIne.html)
  


Passos para reproduzir:

1) Modifique o arquivo [LocalExecute.ps1](.\LocalExecute.ps1) para refletir a sua estrutura de pastas.
2) Uma instancia rodando do RabbitMQ no localhost (ou modifique o arquivo appsettings.json para receber as configurações da conexão).
3) Configure o DotCover como um alias no windows ou modifique o Script Configure para coincidir com o seu executável do DotCover.
4) Execute o LocalExecute.ps1


O resultado esperado:
- Uma pasta `coverage` com o arquivo `report.html` deve aparecer na pasta corrente com o resultado.

*Plano de execução*
```
- dotnet restore .\EntryPointProcessorSolution.sln
- dotnet build .\EntryPoint.Processor\EntryPoint.Processor.csproj -c Debug
- dotnet build .\EntryPoint.Integration.Test\EntryPoint.Integration.Test.csproj
- dotCover inicia as configurações
- dotCover começa a observar a aplicação
- dotnet test .\EntryPoint.Integration.Test\ --no-build
- dotCover finaliza a sessão e gera o relatório
```