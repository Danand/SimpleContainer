# SimpleContainer contribution tips
* use separate `feature/your-feature` branch for each pull request
* use [template](PULL_REQUEST_TEMPLATE.md) for pull requests
* you can generate proposed changes list by running `./scripts/pr-msg.sh`
* run `./scripts/generate-metas.sh` after adding/removing any file at `SimpleContainer.Unity/Assets/SimpleContainer`
* .NET Core solution consists of:
  * `SimpleContainer.Unity\Assets\SimpleContainer\SimpleContainer.Core.csproj` – core classes
  * `SimpleContainer.Tests\SimpleContainer.Tests.csproj` - .NET Core project tests
* notice that core classes are shared both by Unity and .NET Core project
* you can work both with Unity and .NET Core solution
  * `./SimpleContainer.sln` – .NET Core solution, includes NUnit test project
  * `./SimpleContainer.Unity/` – Unity project folder, contains shared code of core classes and Unity-specific implementation
* use local folder or local repository for development and debugging the UPM package ([how](README.md#from-local-path))