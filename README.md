# Avalonia.Redux

Avalonia Redux is an assembled cross-platform application framework based on Redux state management for MVU pattern,  using C# and AvaloniaUI. It allows you to manage your application state and logic based on Redux implemention of MVU. MVU is an implementation idea based on one-way data flow, as shown in the figure below:

<p><img src="./assets/mvu.png" alt="redux-data-flow"></p>

- **1. When the application start, and state is initialized to the current state.**
- **2. When the state changes, and UI rendering to be rendered will be triggered.**
- **3. When interaction occurs, such as the user clicks on the event, an action is sent to the Reducer.**
- **4. Executing the Update action will create an instance of the updated state.**
- **5. The new state replaces the current state, and returns to step [2].**


## Installation

You can use the NuGet package manager to install Avalonia.Redux, just run the following command in your project:

```bash
dotnet add package Avalonia.Redux
```

Or, you can also add the following dependency in your project file:

```xml
<ItemGroup>
  <PackageReference Include="Avalonia.Redux" Version="0.1.0" />
</ItemGroup>
```

## Design Principles

<p><img src="./assets/avalonia_redux.png" alt="avalonia-redux-framework"></p>

The Avalonia Redux framework mainly contains the following parts:

- **Action Creators**: Create executable actions that represent pages or components. Each action has a type to distinguish different actions, and some optional parameters to pass some additional information.
- **Store**: Create and manage application state, dispatch and process actions. When each Page is initialized, a Store instance is created, the initial state and Reducer function are passed in, and some listeners can also be registered with the Store to update the UI or perform some other operations when the state changes.
- **Reducers**: This is a pure function that receives the current state and an action and returns a new state.
- **Effect**: Extensions that handle application side effects, such as jump routing, request data, etc.
- **Component**: Represents a stateless component, which is combined into a part of the Page through Adapter Connector or Slot Connector.
- **Page**: Represents a stateful page component, inherited from Component, which contains the following three parts.
   - InitState: initialization state function, unique to Page component
   - Middleware: middleware, such as printing logs in listening functions, etc.
   - CombineReducers: Integrate all Reducers of page components and sub-components together


## Usage

To use Avalonia.Redux, you need to define the following parts:

- **State**: This is a class that represents data and contains some properties, etc.
- **Action**: This is a class or enumeration that represents the actions that the application can perform, such as add, delete, update, etc. Each action has a type to distinguish different actions, and some optional parameters to pass some additional information.
- **Component**: (Optional) This is a class that represents a stateless view component. Use the method (Dispatch) in Store to dispatch actions, or use the properties of Store to obtain the current state.
- **Page**: This is a class that represents a stateful page component, including the following 6 parts, of which initState, Reducer, and View must be set.
   - **InitState**: initialization state function
   - **Reducer**: This is a pure function. According to the action type, the corresponding function is executed to modify the state.
   - **View**: The UI part of the component, which can be built using XAML or C#
   - **Effect**: (optional) An extension to Reducer, which mainly handles side effects and executes corresponding functions according to the action type.
   - **Middleware**: (optional) middleware, such as listening functions for printing logs, etc.
   - **Dependencies**: (Optional) If a complex page is composed of multiple sub-components, you can set Adapter Connector and Slot Connector. The former is suitable for dynamic collection components, and the latter is suitable for single components.


## Example

You can find a simple example in this repository, showing how to use Avalonia.Redux to implement a Todo List application. You can run the following commands to clone this repository and run the example:

```bash
git clone https://github.com/GavinHome/Avalonia.Redux.git
cd Avalonia.Redux
dotnet run --project samples\samples.Desktop
```

You can also check the source code in the [Example] folder to understand the implementation details of the example.
