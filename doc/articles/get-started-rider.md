---
uid: Uno.GetStarted.Rider
---

# Get Started on JetBrains Rider

> [!IMPORTANT]
> Project templates from Uno Platform 5.3 or later are needed to use Rider. See our [migration guide](xref:Uno.Development.MigratingFromPreviousReleases) to upgrade.
>
> [!IMPORTANT]
> Depending on the version of Rider you will want to use, here are some additional information that you will want to know:
>
> - **Rider**: Current versions of Rider (2024.1 and earlier) do not support creating Uno Platform projects using the "New Solution" dialog, even if the Uno Platform project template appears. In this case, creating an Uno Platform project is done [using dotnet new](xref:Uno.GetStarted.dotnet-new), and the <a target="_blank" href="https://aka.platform.uno/app-wizard">Uno Platform Live Wizard</a>.
>   Make sure to follow the directions for **Rider** provided below.
>
> - **Rider EAP (Early Access Program)**: We have released a new Preview version of the Uno Platform Plugin that supports creating Uno Platform projects using the "New Solution" dialog with Rider EAP.
>   Make sure to install and follow the directions for **Rider EAP** provided below.

## Prerequisites

* [**Rider Version 2024.1+**](https://www.jetbrains.com/rider/download/) or [**Rider EAP Version 2024.2+**](https://www.jetbrains.com/rider/nextversion/)
* [**Rider Xamarin Android Support**](https://plugins.jetbrains.com/plugin/12056-rider-xamarin-android-support/) plugin from Rider in **Settings** / **Plugins**

## Check your environment

[!include[use-uno-check](includes/use-uno-check-inline-noheader.md)]

## Supported Platforms

|                       | **Rider for Windows** | **Rider for Mac**  | **Rider for Linux** |
|-----------------------|-----------------------|--------------------|---------------------|
| Windows (UWP/WinUI)   | ✔️                   | ❌                 | ❌                 |
| Android               | ✔️                   | ✔️                 | ✔️                |
| iOS                   | ❌                   | ✔️                 | ❌                 |
| Wasm                  | ✔️†                  | ✔️†                | ✔️†                |
| Catalyst              | ❌                   | ✔️                 | ❌                 |
| Skia Desktop          | ✔️                   | ✔️                 | ✔️                |

<details>
    <summary>† Notes (Click to expand)</summary>

* **WebAssembly**: debugging from the IDE is not available yet on Rider. You can use the [Chromium in-browser debugger](xref:UnoWasmBootstrap.Features.Debugger#how-to-use-the-browser-debugger) instead.

</details>

## Install the Uno Platform plugin

<!-- TODO: This section needs to be reviewed to determine if we should reference the JetBrains Marketplace instead of directly pointing to the Configure menu in Rider. It appears that the channel options (Stable | Preview) are not available at this level based on my quick first review. -->
In Rider, in the **Configure**, **Plugins** menu, open the **Marketplace** tab, then search for **Uno Platform**:

![Visual Studio Installer - .NET desktop development workload](Assets/ide-rider-plugin-search.png)

Then click the install button.

<!-- END TODO -->

## Platform specific setup

You may need to follow additional directions, depending on your development environment.

### Linux

[!include[linux-setup](includes/additional-linux-setup-inline.md)]

---

## Next Steps

You're all set! You can create your [first Uno Platform app](xref:Uno.GettingStarted.CreateAnApp.Rider).
