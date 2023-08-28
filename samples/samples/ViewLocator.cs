using Avalonia.Controls;
using Avalonia.Controls.Templates;
using samples.ViewModels;
using System;

namespace samples
{
    public class ViewLocator : IDataTemplate
    {
#pragma warning disable CS8767 // �����������������͵�Ϊ Null ������ʽʵ�ֵĳ�Ա��ƥ��(����������Ϊ Null ������)��
        public Control Build(object data)
#pragma warning restore CS8767 // �����������������͵�Ϊ Null ������ʽʵ�ֵĳ�Ա��ƥ��(����������Ϊ Null ������)��
        {
            if (data is null)
#pragma warning disable CS8603 // ���ܷ��� null ���á�
                return null;
#pragma warning restore CS8603 // ���ܷ��� null ���á�

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