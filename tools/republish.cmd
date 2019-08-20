dotnet tool uninstall --global DependencyDotNet
dotnet pack
dotnet tool install --global --add-source ./packages DependencyDotNet