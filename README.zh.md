# Avalonia.Redux

Avalonia Redux ��һ������ MVU ģʽ�� Redux ״̬�������װʽ��ƽ̨Ӧ�ó����ܣ�ʹ�� C# �� AvaloniaUI ʵ�֡� ��������ʹ�û��� Redux ��״̬������������Ӧ�ó���״̬���߼���MVU�ǵ���������������ͼ��ʾ��

<p><img src="./assets/mvu.png" alt="redux-data-flow"></p>

- **1.��Ӧ������ʱ��״̬����ʼ��Ϊ��ǰ״̬��**
- **2.��״̬����ʱ��������Ҫ���ֵ� UI ��Ⱦ��**
- **3.����������ʱ�����û�����¼�������Reducer���Ͷ�����**
- **4.ִ��Update����������������״̬��ʵ����**
- **5.��״̬ȡ����ǰ״̬�����ص�ǰ�б�Ĳ���[2]��**


## ��װ

�����ʹ�� NuGet ������������װ Avalonia.Redux��ֻ���������Ŀ�������������

```bash
dotnet add package Avalonia.Redux
```

���ߣ���Ҳ�����������Ŀ�ļ����������������

```xml
<ItemGroup>
  <PackageReference Include="Avalonia.Redux" Version="0.1.0" />
</ItemGroup>
```

## ������


<p><img src="./assets/avalonia_redux.png" alt="avalonia-redux-framework"></p>


Avalonia Redux�����Ҫ�������¼������֣�

- **Action Creators**��������ʾҳ�������Ŀ�ִ�ж�����ÿ����������һ�����ͣ��������ֲ�ͬ�Ķ������Լ�һЩ��ѡ�Ĳ�������������һЩ�������Ϣ��
- **Store**�������͹���Ӧ�ó���״̬���ַ��ʹ�������ÿ��Page��ʼ��ʱ���ᴴ��һ�� Store ��ʵ���������ʼ״̬�� Reducer �������Լ�Ҳ������ Store ע��һЩ��������������״̬�����仯ʱ���� UI ����ִ��һЩ�����Ĳ�����
- **Reducers**������һ�������������յ�ǰ��״̬��һ������������һ���µ�״̬��
- **Effect**������Ӧ�ó������õ���չ��������ת·�ɡ��������ݵȡ�
- **Component**����ʾ��״̬�����ͨ��Adapter Connector ���� Slot Connector��ϳ�Page��һ���֡�
- **Page**����ʾ��״̬��ҳ��������̳���Component�������������������֡�
  - InitState: ��ʼ��״̬������Page�������
  - Middleware:  �м����������������д�ӡ��־��
  - CombineReducers:  ��ҳ������������������Reducers���ɵ�һ��


## ʹ�÷���

Ҫʹ�� Avalonia.Redux������Ҫ�������¼������֣�

- **State**������һ���࣬��ʾ���ݣ�����һЩ���Եȡ�
- **Action**������һ�����ö�٣���ʾӦ�ó������ִ�еĶ������������ӡ�ɾ�������µȡ�ÿ����������һ�����ͣ��������ֲ�ͬ�Ķ������Լ�һЩ��ѡ�Ĳ�������������һЩ�������Ϣ��
- **Component**������ѡ������һ���࣬��ʾ��״̬����ͼ�����ʹ�� Store �еķ�����Dispatch�����ַ�����������ʹ�� Store ����������ȡ��ǰ��״̬��
- **Page**������һ���࣬��ʾ��״̬��ҳ���������������6�����֣�����initState��Reducer��View�������á�
  - **InitState**: ��ʼ��״̬����
  - **Reducer**������һ�������������ݶ������ͣ�ִ�ж�Ӧ����ʵ�ֶ�״̬���޸ġ�
  - **View**: �����UI���֣���ʹ��XAML��C#������
  - **Effect**������ѡ����Reducer����չ����Ҫ�������ã����ݶ�������ִ����Ӧ�ĺ�����
  - **Middleware**:����ѡ���м���������ӡ��־�ļ���������
  - **Dependencies**:����ѡ�����ɶ���������ͬ��ɸ��ӵ�ҳ��Page, ������Adapter Connector��Slot Connector��ǰ�������ڶ�̬������������������ڵ������

�Լ�����Ϊ��������Ҫ5������ʹ��avalonia redux����Ӧ�ã�

> 1. ���� avalonia redux 
> 2. ����״̬��
> 3. ���� Action �� ActionCreator
> 4. �����޸�״̬�� Reducer
> 5. ���������ҳ����ͼ����ʾ

```cs
using Redux;
using Redux.Component;
using Action = Redux.Action;
namespace samples.Pages.Counter;

/// [State]
public class CounterState : ReactiveObject
{
    [Reactive]
    public int Count { get; set; }

    public override string ToString()
    {
        return $"Count: {Count}";
    }
}

/// [Action]
enum CounterAction { add, onAdd }

/// [ActionCreator]
internal static class CounterActionCreator
{
    internal static Action addAction(int payload)
    {
        return new Action(CounterAction.add, payload);
    }

    internal static Action onAddAction()
    {
        return new Action(CounterAction.onAdd);
    }
}

/// [Reducer]
public partial class CounterPage
{
    private static Reducer<CounterState> buildReducer() => ReducerConverter.AsReducers(new Dictionary<object, Reducer<CounterState>>
    {
        {
            CounterAction.add, _add
        }
    });

    private static CounterState _add(CounterState state, Action action)
    {
        state.Count += action.Payload;
        return state;
    }
}

/// [Effect]
public partial class CounterPage
{
    private static Effect<CounterState>? buildEffect() => Redux.Component.EffectConverter.CombineEffects(new Dictionary<object, SubEffect<CounterState>>
    {
        {
            CounterAction.onAdd, _onAdd
        }
    });

    private static async Task _onAdd(Action action, ComponentContext<CounterState> ctx)
    {
        ctx.Dispatch(CounterActionCreator.addAction(1));
        await Task.CompletedTask;
    }
}

/// [Page]
public partial class CounterPage() : Page<CounterState, Dictionary<string, dynamic>>(initState: initState,
    effect: buildEffect(),
    reducer: buildReducer(),
    middlewares:
    [
        Redux.Middlewares.logMiddleware<CounterState>(monitor: (state) => state.ToString(), tag: "CounterPage")
    ],
    view: (state, dispatch, _) =>
    {
        return new ContentControl
        {
            Content = new Grid
            {
                Margin = Thickness.Parse("10"),
                RowDefinitions =
                [
                    new() { Height = GridLength.Star },
                    new() { Height = GridLength.Auto }
                ],
                Children =
                {
                    new StackPanel
                    {
                        [Grid.RowProperty] = 0,
                        Orientation = Orientation.Vertical,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Children =
                        {
                            new TextBlock
                            {
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                Text = "You have pushed the button this many times:"
                            },
                            new TextBlock
                            {
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                [!TextBlock.TextProperty] = new Binding
                                    { Source = state, Path = nameof(state.Count) }
                            }
                        }
                    },
                    new Border
                    {
                        [Grid.RowProperty] = 1,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Center,
                        Background = SolidColorBrush.Parse("#bbe9d3ff"),
                        Padding = new Thickness(0),
                        CornerRadius = new CornerRadius(15),
                        BoxShadow = new BoxShadows(new BoxShadow()
                            { OffsetX = 1, OffsetY = 5, Spread = 1, Blur = 8, Color = Colors.LightGray }),
                        Child = new Button()
                        {
                            Background = new SolidColorBrush(Colors.Transparent),
                            CornerRadius = new CornerRadius(15),
                            Padding = new Thickness(0),
                            Height = 50, Width = 50,
                            BorderThickness = new Thickness(0),
                            Content = new Border
                            {
                                Background = new SolidColorBrush(Colors.Transparent),
                                Padding = new Thickness(8, 5, 12, 8),
                                Child = new Path
                                {
                                    Data = Geometry.Parse("M19,13H13V19H11V13H5V11H11V5H13V11H19V13Z"),
                                    Fill = new SolidColorBrush(Colors.Black),
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Center,
                                },
                            },
                            Command = ReactiveCommand.Create(() => dispatch(CounterActionCreator.onAddAction()))
                        }
                    }
                }
            }
        };
    })
{
    /// [InitState]
    private static CounterState initState(Dictionary<string, dynamic>? param) => new() { Count = 99 };
}
```

## ʾ��

�����������ֿ����ҵ�һ���򵥵�ʾ����չʾ�����ʹ�� Avalonia.Redux ��ʵ��һ�� Todo List ��Ӧ�ó������������������������¡����ֿ⣬������ʾ����

```bash
git clone https://github.com/GavinHome/Avalonia.Redux.git
cd Avalonia.Redux
dotnet run --project samples\samples.Desktop
```

��Ҳ���Բ鿴 [Example] �ļ����е�Դ���룬�˽�ʾ���ľ���ʵ��ϸ�ڡ�

