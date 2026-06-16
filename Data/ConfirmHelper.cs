using System.Windows;

namespace PosAccountingApp.Data;

public static class ConfirmHelper
{
    public static bool Confirm(string message, string title = "تایید")
    {
        var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        return result == MessageBoxResult.Yes;
    }

    public static bool ConfirmDelete(string itemName)
    {
        return Confirm($"آیا از حذف '{itemName}' اطمینان دارید؟\nاین عمل قابل بازگشت نیست.", "تایید حذف");
    }

    public static bool ConfirmReturn(string invoiceNumber, decimal amount)
    {
        return Confirm(
            $"آیا از مرجوعی فاکتور '{invoiceNumber}' به مبلغ {amount:N0} ریال اطمینان دارید؟\nموجودی کالاها بازگردانده می‌شود.",
            "تایید مرجوعی");
    }

    public static void ShowSuccess(string message)
    {
        MessageBox.Show(message, "موفق", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public static void ShowError(string message)
    {
        MessageBox.Show(message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public static void ShowWarning(string message)
    {
        MessageBox.Show(message, "توجه", MessageBoxButton.OK, MessageBoxImage.Warning);
    }
}
