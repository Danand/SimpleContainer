# SimpleContainer.Unity.Example
This is example Unity project using **SimpleContainer**.

## Directory structure
Classes contained in these folders are separated into different assemblies ([explanation below](#inject-attribute)): 
* `Assets/Scripts/Dependent` – needy classes, where injection must be performed into
* `Assets/Scripts/Installers` – installers, where dependencies are registered

## Code
### Resolving lifestyles
Currently two lifestyles are presented:
* `Scope.Transient` (new instance for each dependency)
* `Scope.Singleton` (the only instance for all dependencies)

### Constructor injection
The preferred ones injection. Used only for POCOs, not `MonoBehaviour`, but it's the point.  
No extra attributes required . Just define the constructor with dependencies as parameters. So you may re-use constructor without any DI-container.  
See example [here](Assets/Scripts/Dependent/Loggers/LoggerDefault.cs#L13)

### Property injection
Less preferred, but still good kind of injection. Can be used at `MonoBehaviour` too.  
Injectable property must be `public`, along with public `{ get; set; }` methods. It's the best practice to let your classes be injected without DI-container.  
Requires `InjectAttribute` defined in implementations' assembly ([why](#inject-attribute)).  
See example [here](Assets/Scripts/Dependent/UIManager.cs#L30)

### Inject attribute
Needy classes should not being dependent on a DI-container, because it could be replaced someday by another DI-container or plain old constructor injection at the very composition root. So `Inject` attribute must be defined in the same assembly as dependencies.  
See example [here](Assets/Scripts/Installers/MainInstaller.cs#L19)

## Credits
Font used: **Orbitron** – https://github.com/theleagueof/orbitron
