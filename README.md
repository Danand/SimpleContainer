# SimpleContainer
![](https://github.com/danand/SimpleContainer/workflows/Build%20and%20test/badge.svg)
[![MIT license](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/danand/SimpleContainer/blob/master/LICENSE.md)

The point is the usage of DI-container in a proper way.

## How to install

### From remote repository
Add in `Packages/manifest.json` to `dependencies`:
```javascript
"com.danand.simplecontainer": "https://github.com/Danand/SimpleContainer.git#0.7.0-package-unity",
```

### From local path
<details>
	<summary>From local repository</summary>
	
	"com.danand.simplecontainer": "file:///D/repos/SimpleContainer/.git#0.7.0-package-unity",
</details>

<details>
	<summary>From local working copy</summary>
	
	"com.danand.simplecontainer": "file:D:/repos/SimpleContainer/SimpleContainer.Unity/Assets",
</details>

<details>
	<summary>What is the difference?</summary>
	<p>
		Local repository is resolved just like normal Git repository with optionally specified revision.<br />
		Local working copy is being copied "as is" into dependent project, without running any Git process.
	</p>
</details>

## How to use
1. Add `UnityProjectRoot` component to any object in hierarchy.
2. Reference `SimpleContainer.Core` and `SimpleContainer.Unity` assemblies to your installer assembly via [Assembly Definitions](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html)).
3. Implement your own `MonoInstaller` and add it as component too ([example project below](#example)).
4. Reference `MonoInstaller` to `UnityProjectRoot` via inspector.
5. Run.

## Best practices
1. **Do not** use `DontDestroyOnLoad()` to pass container through different scenes. **Use** `LoadSceneMode.Single` for "immortal" objects (e.g. managers, services, etc.), and `LoadSceneMode.Additive` for presentation layer (e.g. gameplay, UI, etc.). Quite good alternative is to use separate `UnityProjectRoot` per each loaded scene.
2. **Prefer** constructor injection over any other. Yeah, that means less usage of `MonoBehaviour` dependants.
3. **Do not** put dependencies on container. It will be perfect, if you did not have any `using SimpleContainer;` in your needy classes.
4. **Use** custom `Inject` attribute (example below).
5. You may move your installers to a separate assembly (via `.asmdef`) for stronger container usage protection.
6. If you really want to inject into `MonoBehaviour` without explicit registration – use `MonoRegistrator`.

## Example
See [example project](SimpleContainer.Unity.Example) included into this repository. Also you may open it with Unity and run.

## How to contribute
See [contribution note](CONTRIBUTING.md).

## Roadmap
- [x] Clean up repository
- [x] Set up CI
- [ ] Optimize resolving
- [ ] Refactor container interface
