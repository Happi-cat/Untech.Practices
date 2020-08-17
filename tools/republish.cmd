dotnet tool uninstall --global DependencyDotNet
dotnet tool uninstall --global Adr
dotnet pack
dotnet tool install --global --add-source ./packages DependencyDotNet
dotnet tool install --global --add-source ./packages Adr