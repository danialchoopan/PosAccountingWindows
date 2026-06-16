using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using PosAccountingApp.Services;

namespace PosAccountingApp.Views;

public partial class AddPopupWindow : Window
{
    private byte[]? _fileData;
    private string _fileName = string.Empty;
    private string _contentType = string.Empty;
    private readonly Action<string, string?, byte[]?>? _onSave;
    private readonly string _entityType;

    public AddPopupWindow(string title, string entityType, Action<string, string?, byte[]?> onSave)
    {
        InitializeComponent();
        TitleText.Text = title;
        _entityType = entityType;
        _onSave = onSave;
    }

    private void BrowseFile_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp;*.gif|PDF|*.pdf|All|*.*",
            Title = "انتخاب فایل پیوست"
        };

        if (dialog.ShowDialog() == true)
        {
            _fileData = File.ReadAllBytes(dialog.FileName);
            _fileName = Path.GetFileName(dialog.FileName);
            _contentType = GetContentType(dialog.FileName);

            FileNameText.Text = _fileName;

            // Show image preview for image files
            if (_contentType.StartsWith("image/"))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(dialog.FileName);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                PreviewImage.Source = bitmap;
                ImagePreview.Visibility = Visibility.Visible;
            }
            else
            {
                ImagePreview.Visibility = Visibility.Collapsed;
            }
        }
    }

    private void RemoveFile_Click(object sender, RoutedEventArgs e)
    {
        _fileData = null;
        _fileName = string.Empty;
        FileNameText.Text = "فایلی انتخاب نشده";
        PreviewImage.Source = null;
        ImagePreview.Visibility = Visibility.Collapsed;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            _onSave?.Invoke(_fileName, _contentType, _fileData);
            this.DialogResult = true;
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("خطا: " + ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        this.Close();
    }

    private static string GetContentType(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLower();
        return ext switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".bmp" => "image/bmp",
            ".gif" => "image/gif",
            ".pdf" => "application/pdf",
            _ => "application/octet-stream"
        };
    }
}
