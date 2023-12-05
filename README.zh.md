# Avalonia.Redux

Avalonia.Redux 是一个为 [Avalonia] 框架编写的 [Redux] 的实现，可以让你使用 Redux 的模式来管理你的应用程序的状态和逻辑。

## 安装

你可以使用 NuGet 包管理器来安装 Avalonia.Redux，只需在你的项目中运行以下命令：

```bash
dotnet add package Avalonia.Redux
```

或者，你也可以在你的项目文件中添加以下依赖：

```xml
<ItemGroup>
  <PackageReference Include="Avalonia.Redux" Version="0.1.0" />
</ItemGroup>
```

## 使用方法

要使用 Avalonia.Redux，你需要定义以下几个部分：

- **State**：这是一个类，表示你的应用程序的状态，包含一些属性，比如数据、用户输入、UI 状态等。
- **Action**：这是一个类，表示你的应用程序可以执行的动作，比如增加、删除、更新等。每个动作都有一个类型，用来区分不同的动作，以及一些可选的参数，用来传递一些额外的信息。
- **Reducer**：这是一个函数，接收当前的状态和一个动作，返回一个新的状态。根据动作的类型，你可以对状态进行一些修改，或者直接返回原来的状态，如果不需要修改的话。
- **Store**：这是一个类，用来创建和管理你的应用程序的状态，以及分发和处理动作。你可以在你的应用程序的入口处创建一个 Store 的实例，传入你定义的初始状态和 Reducer 函数。你也可以向 Store 注册一些监听器，用来在状态发生变化时更新你的 UI 或者执行一些其他的操作。
- **Component**：这是一个类，继承自 Avalonia 的控件类，比如 Window、UserControl 等。你可以在你的组件中使用 Store 的方法来分发动作，或者使用 Store 的属性来获取当前的状态。你也可以使用一些扩展方法，比如 Connect、Dispatch 等，来简化你的代码。

## 示例

你可以在这个仓库中找到一个简单的示例，展示了如何使用 Avalonia.Redux 来实现一个 Todo List 的应用程序。你可以运行以下命令来克隆这个仓库，并运行示例：

```bash
git clone https://github.com/GavinHome/Avalonia.Redux.git
cd Avalonia.Redux
dotnet run --project samples\samples.Desktop
```

你也可以查看 [Example] 文件夹中的源代码，了解示例的具体实现细节。

