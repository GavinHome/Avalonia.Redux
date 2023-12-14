namespace samples.Pages.Todos.Todo;

public partial class View : UserControl
{
    public static readonly StyledProperty<ICommand> OnEditCommandProperty =
        AvaloniaProperty.Register<Button, ICommand>(
            nameof(OnEditCommand));
    
    public static readonly StyledProperty<ICommand> OnRemoveCommandProperty =
        AvaloniaProperty.Register<Button, ICommand>(
            nameof(OnRemoveCommand));
    
    public static readonly StyledProperty<ICommand> OnDoneCommandProperty =
        AvaloniaProperty.Register<Button, ICommand>(
            nameof(OnDoneCommand));
    
    public View()
    {
        InitializeComponent();
    }

    public ICommand? OnRemoveCommand 
    {
        get => GetValue(OnRemoveCommandProperty);
        set => SetValue(OnRemoveCommandProperty!, value);
    }
    public ICommand? OnDoneCommand 
    {
        get => GetValue(OnDoneCommandProperty);
        set => SetValue(OnDoneCommandProperty!, value);
    }
    public ICommand? OnEditCommand 
    {
        get => GetValue(OnEditCommandProperty);
        set => SetValue(OnEditCommandProperty!, value);
    }
}