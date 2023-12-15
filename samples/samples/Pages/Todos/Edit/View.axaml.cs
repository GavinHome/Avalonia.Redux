namespace samples.Pages.Todos.Edit;

public partial class View : UserControl
{
    public static readonly StyledProperty<ICommand> OnDoneCommandProperty =
        AvaloniaProperty.Register<Button, ICommand>(
            nameof(OnDoneCommand));
    
    public View()
    {
        InitializeComponent();
    }
 
    public ICommand? OnDoneCommand 
    {
        get => GetValue(OnDoneCommandProperty);
        set => SetValue(OnDoneCommandProperty!, value);
    }
}