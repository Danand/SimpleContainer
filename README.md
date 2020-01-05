# SimpleContainer
The point is the usage of DI-container in a proper way.

## How to install

### From remote repository
Add in `Packages/manifest.json` to `dependencies`:
```javascript
"com.danand.simplecontainer": "https://github.com/Danand/SimpleContainer.git#0.6.9-package-unity"
```

### From local repository
Add in `Packages/manifest.json` to `dependencies`:
```javascript
"com.danand.simplecontainer": "file:D:/repos/SimpleContainer/SimpleContainer.Unity/Assets"
```

## How to use
1. Add `UnityProjectRoot` component to any object in hierarchy.
2. Implement your own `MonoInstaller` and add it as component too.
3. Reference `MonoInstaller` to `UnityProjectRoot`.
4. Run.

## Best practices
1. **Do not** use `DontDestroyOnLoad()` to pass container through different scenes. **Use** `LoadSceneMode.Single` for "immortal" objects (e.g. managers, services, etc.), and `LoadSceneMode.Single` for presentation layer (e.g. gameplay, UI, etc.). Quite good alternative is to use separate `UnityProjectRoot` per each loaded scene.
2. Prefer constructor injection over any other. Yeah, that means less usage of `MonoBehaviour` dependencies.
3. **Do not** put dependencies on container. It will be perfect, if you did not have any `using SimpleContainer;` in your needy classes.
4. **Use** custom `Inject` attribute (example below).
5. You may move your installers to a separate assembly (via `.asmdef`) for stronger container usage protection.

## Example installer
```csharp
public sealed class GameInstaller : MonoInstaller
{
    public SettingsRepositoryAsset settingsRepository;
    public MainMenuLayout mainMenuLayout;
    public DailyGoalLayout dailyGoalLayout;

    public override void Install(Container container)
    {
	// CUSTOM ATTRIBUTE:
	container.RegisterAttribute<Project.InjectAttribute>();
	
        // SETTINGS INSTANCES:
        container.Register<ISettingsRepository>(Scope.Singleton, settingsRepository);

        // UI LAYOUT REFERENCES:
        container.Register(Scope.Singleton, mainMenuLayout);
        container.Register(Scope.Singleton, dailyGoalLayout);

        // SINGLETONES:
        container.Register<ILocalizationRepository, LocalizationRepositoryFromFile>(Scope.Singleton);
        container.Register<ITimeProvider, TimeProviderSystemJST>(Scope.Singleton);
    }

    public override Task ResolveAsync(Container container)
    {
        // NON-LAZY RESOLVING (order-sensitive):
        container.Resolve<ILocalizationRepository>();
        return Task.CompletedTask;
    }

    public override async Task AfterResolveAsync(Container container)
    {
        // Do initialization stuff or something.
    }
}
```
If you really want to register `MonoBehaviour` from the component of itself, use `MonoRegistrator`.

## How to contribute
* use separate `feature/your-feature` branch for each pull request
* run `./generate-metas.sh` after adding/removing any file at `SimpleContainer.Unity/Assets/SimpleContainer.Core`
* you can work both with Unity and .NET Core solution

## Roadmap
- [x] Clean up repository
- [ ] Optimize resolving
- [ ] Refactor public methods
