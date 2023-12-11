using Avalonia.Controls;
using Avalonia.Controls.Templates;
using samples.ViewModels;

namespace samples
{
    public class ViewLocator : IDataTemplate
    {
        public Control Build(object? data)
        {
            if (data is null)
#pragma warning disable CS8603 // 可能返回 null 引用。
                return null;
#pragma warning restore CS8603 // 可能返回 null 引用。

            var name = data.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }

            return new TextBlock { Text = name };
        }

        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
    }
}