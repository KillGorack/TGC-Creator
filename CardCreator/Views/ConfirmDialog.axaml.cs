using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace CardCreator.Views;

public partial class ConfirmDialog : Window
{
    public ConfirmDialog()
    {
        InitializeComponent();
    }

    public static async Task<bool> ShowAsync(Window owner, string title, string message)
    {
        var dialog = new ConfirmDialog
        {
            Title = title
        };

        var messageText = dialog.FindControl<TextBlock>("MessageText")!;
        messageText.Text = message;

        var confirmButton = dialog.FindControl<Button>("ConfirmButton")!;
        var cancelButton = dialog.FindControl<Button>("CancelButton")!;

        confirmButton.Click += (_, _) => dialog.Close(true);
        cancelButton.Click += (_, _) => dialog.Close(false);

        var result = await dialog.ShowDialog<bool>(owner);
        return result;
    }
}
