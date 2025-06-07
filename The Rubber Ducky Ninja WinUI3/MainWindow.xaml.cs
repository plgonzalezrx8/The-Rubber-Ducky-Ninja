using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using WinRT.Interop;
using Windows.Storage;
using System.Diagnostics;

namespace The_Rubber_Ducky_Ninja_WinUI3
{
    public sealed partial class MainWindow : Window
    {
        private string currentFilePath = "";
        private DuckyScriptProcessing duckyProcessor = new DuckyScriptProcessing();

        public MainWindow()
        {
            this.InitializeComponent();
            this.Title = "The Rubber Ducky Ninja - WinUI 3";
            
            // Set window size
            this.AppWindow.Resize(new Windows.Graphics.SizeInt32(700, 900));
            
            CheckForJavaAndDuckEncode();
        }

        private void CheckForJavaAndDuckEncode()
        {
            if (!File.Exists("duckencode.jar"))
            {
                ShowInfoBar("⚠️ Encoder Not Found", 
                    "duckencode.jar not found. Binary encoding feature will be disabled.", 
                    InfoBarSeverity.Warning);
            }
        }

        private async void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            
            // Make it work in WinUI 3
            var hwnd = WindowNative.GetWindowHandle(this);
            InitializeWithWindow.Initialize(picker, hwnd);
            
            picker.ViewMode = PickerViewMode.List;
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".txt");
            picker.CommitButtonText = "Load DuckyScript";

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                currentFilePath = file.Path;
                FilePathDisplay.Text = $"📄 {file.Name}";
                
                // Enable validation and configuration
                ValidateButton.IsEnabled = true;
                SetDelayButton.IsEnabled = true;
                EditButton.IsEnabled = true;
                
                ShowInfoBar("✅ File Loaded", 
                    $"Successfully loaded: {file.Name}", 
                    InfoBarSeverity.Success);
            }
        }

        private void SetDelayButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int delay = (int)DelayNumberBox.Value;
                duckyProcessor.SetDelay(delay);
                
                ShowInfoBar("⏱️ Delay Set", 
                    $"Delay between commands set to {delay}ms", 
                    InfoBarSeverity.Informational);
            }
            catch (Exception ex)
            {
                ShowInfoBar("❌ Invalid Delay", 
                    "Please enter a valid number between 0-10000", 
                    InfoBarSeverity.Error);
            }
        }

        private async void ValidateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath)) return;

            // Visual feedback
            ValidateButton.Content = "🔄 Validating...";
            ValidateButton.IsEnabled = false;

            try
            {
                await Task.Run(() =>
                {
                    var isValid = duckyProcessor.validateCode(currentFilePath);
                    
                    // Update UI on main thread
                    DispatcherQueue.TryEnqueue(() =>
                    {
                        if (isValid)
                        {
                            ExecuteButton.IsEnabled = true;
                            EncodeButton.IsEnabled = File.Exists("duckencode.jar");
                            
                            ShowInfoBar("✅ Validation Success", 
                                "No problems found in the script!", 
                                InfoBarSeverity.Success);
                        }
                        else
                        {
                            ShowInfoBar("❌ Validation Failed", 
                                "Please check your script for errors.", 
                                InfoBarSeverity.Error);
                        }
                        
                        ValidateButton.Content = "✅ Validate Script";
                        ValidateButton.IsEnabled = true;
                    });
                });
            }
            catch (Exception ex)
            {
                ShowInfoBar("❌ Validation Error", 
                    $"Error during validation: {ex.Message}", 
                    InfoBarSeverity.Error);
                
                ValidateButton.Content = "✅ Validate Script";
                ValidateButton.IsEnabled = true;
            }
        }

        private async void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath)) return;

            // Warning dialog
            var dialog = new ContentDialog()
            {
                Title = "⚠️ Execute Script",
                Content = "This will simulate keyboard input on your system. Make sure you're ready!\n\n" +
                         "The script will start in 3 seconds after clicking OK.",
                PrimaryButtonText = "Execute",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.Content.XamlRoot
            };

            var result = await dialog.ShowAsync();
            if (result != ContentDialogResult.Primary) return;

            // Visual feedback
            ExecuteButton.Content = "⏳ Executing...";
            ExecuteButton.IsEnabled = false;

            try
            {
                // Give user time to prepare
                for (int i = 3; i > 0; i--)
                {
                    ExecuteButton.Content = $"⏳ Starting in {i}...";
                    await Task.Delay(1000);
                }

                ExecuteButton.Content = "⏳ Running Script...";
                
                // Execute the script
                await duckyProcessor.ReadFileAsync(currentFilePath);

                ShowInfoBar("🎯 Execution Complete", 
                    "Script execution finished successfully!", 
                    InfoBarSeverity.Success);
            }
            catch (Exception ex)
            {
                ShowInfoBar("❌ Execution Error", 
                    $"Error during execution: {ex.Message}", 
                    InfoBarSeverity.Error);
            }
            finally
            {
                ExecuteButton.Content = "▶️ Execute Script";
                ExecuteButton.IsEnabled = true;
            }
        }

        private async void EncodeButton_Click(object sender, RoutedEventArgs e)
        {
            // This would open the encoding dialog
            // For now, show a placeholder
            ShowInfoBar("🔧 Encoding Feature", 
                "Binary encoding dialog coming soon!", 
                InfoBarSeverity.Informational);
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath)) return;

            try
            {
                // Try Notepad++ first, then fallback to Notepad
                try
                {
                    Process.Start("notepad++.exe", currentFilePath);
                }
                catch
                {
                    Process.Start("notepad.exe", currentFilePath);
                }
            }
            catch (Exception ex)
            {
                ShowInfoBar("❌ Edit Error", 
                    $"Could not open editor: {ex.Message}", 
                    InfoBarSeverity.Error);
            }
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("restore.vbs");
            }
            catch (Exception ex)
            {
                ShowInfoBar("❌ Restore Error", 
                    $"Could not create restore point: {ex.Message}", 
                    InfoBarSeverity.Error);
            }
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            // Show about dialog
            ShowInfoBar("🥷 The Rubber Ducky Ninja", 
                "Modern WinUI 3 version - A toolkit for USB Rubber Ducky testing", 
                InfoBarSeverity.Informational);
        }

        private void UACSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(@"C:\Windows\System32\UserAccountControlSettings.exe");
            }
            catch (Exception ex)
            {
                ShowInfoBar("❌ UAC Settings Error", 
                    $"Could not open UAC settings: {ex.Message}", 
                    InfoBarSeverity.Error);
            }
        }

        private void ShowInfoBar(string title, string message, InfoBarSeverity severity)
        {
            // Create and show a temporary InfoBar
            var infoBar = new InfoBar()
            {
                Title = title,
                Message = message,
                Severity = severity,
                IsOpen = true
            };

            // Auto-close after 4 seconds
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(4);
            timer.Tick += (s, e) =>
            {
                infoBar.IsOpen = false;
                timer.Stop();
            };
            timer.Start();

            // You would add this to a notification area in your UI
            // For now, this is a placeholder implementation
        }
    }
} 