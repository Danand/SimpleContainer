# SimpleContainer.Unity.Example
This is example Unity project using **SimpleContainer**.

## Structure
* `Assets/Scripts/Dependent` – needy classes, where injection must be performed
* `Assets/Scripts/Installers` – installers, where dependencies are registered

Notice that dependent classes are not `using` DI-container namespaces at all. Only installers should do that.

## Credits
Font used: **Orbitron** – https://github.com/theleagueof/orbitron