dotnet restore .\EntryPointProcessorSolution.sln
dotnet build .\EntryPoint.Processor\EntryPoint.Processor.csproj -c Debug
dotnet build .\EntryPoint.Integration.Test\EntryPoint.Integration.Test.csproj
dotCover.exe cover --targetExecutable="<yourPath>\EntryPoint.Processor\bin\Debug\netcoreapp3.1\EntryPoint.Processor.exe" --output=coverage/report.html --reportType=HTML --startInstance="1" --LogFile=someLogFile.log --Filters=""
dotCover send --Instance="1" --Command="Cover" --Timeout=10
dotnet test .\EntryPoint.Integration.Test\ --no-build
dotCover send --Instance="1" --Command="GetSnapshotAndKillChildren"
