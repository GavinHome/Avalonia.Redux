using Avalonia.Controls;
using Avalonia.Controls.Templates;
using samples.ViewModels;
using System;

namespace samples
{
    public class ViewLocator : IDataTemplate
    {
#pragma warning disable CS8767 // 参数类型中引用类型的为 Null 性与隐式实现的成员不匹配(可能是由于为 Null 性特性)。
        public Control Build(object data)
#pragma warning restore CS8767 // 参数类型中引用类型的为 Null 性与隐式实现的成员不匹配(可能是由于为 Null 性特性)。
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