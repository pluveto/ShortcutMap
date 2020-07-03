using Pluvet.ShortcutMap.Entities;
using Pluvet.ShortcutMap.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Converters;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pluvet.ShortcutMap
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            foreach (var item in App.Maps)
            {
                var menuItem = new System.Windows.Controls.MenuItem { Header = item.App, Tag = item };
                menuItem.Click += MenuItem_Click;
                this.LoadedMenuItem.Items.Add(menuItem);
            }

            var bgBrush = new ImageBrush(new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "/bg.png")));
            this.Background = bgBrush;

            new GlobalHotKey(Key.N, KeyModifier.Win, OnHotKeyHandler);
            this.Hide();

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as System.Windows.Controls.MenuItem;
            var item = menuItem.Tag as ShortcutMapEntity;
            Process.Start("explorer.exe", item.FileLocation);
        }

        private void OnHotKeyHandler(GlobalHotKey hotKey)
        {
            if (this.IsFullscreen())
            {
                this.Hide();
                this.ExitFullscreen();
            }
            else
            {

                var activeModule = WindowHelper.GetActiveModuleName();
                if (activeModule == null) return;
                var matchedMap = App.MatchMap(App.Maps, activeModule);
                if (matchedMap == null) return;

                RenderMap(matchedMap);

                this.Show();
                this.GoFullscreen();

            }
        }
        public static ShortcutMapEntity Previous = null;
        private void RenderMap(ShortcutMapEntity map)
        {
            if (Previous == map) { return; }
            Previous = map;
            MapPanel.Children.RemoveRange(0, MapPanel.Children.Count);

            var logo = new BitmapImage(map.IconUri);
            this.Logo.Source = logo;
            this.AppName.Text = map.App;

            Style styleItemDockPanel = this.FindResource("ItemDockPanel") as Style;
            Style styleShortcutButton = this.FindResource("ShortcutButton") as Style;
            Style styleGroupLabel = this.FindResource("GroupLabel") as Style;
            Style styleShortcutDescLabel = this.FindResource("ShortcutDescLabel") as Style;

            foreach (var group in map.Groups)
            {
                if (group.Name == "::linebreak")
                {
                    //MapPanel.Children.Add(new NewLine());
                    continue;
                }

                var wrapper = new StackPanel() { Margin = new Thickness(0, 0, 60, 20) };

                var groupLabel = new TextBlock();
                groupLabel.Text = group.Name;
                groupLabel.Style = styleGroupLabel;
                wrapper.Children.Add(groupLabel);


                foreach (var shortcut in group.Shortcuts)
                {
                    var itemDockPanel = new DockPanel();
                    itemDockPanel.Style = styleItemDockPanel;

                    foreach (var key in shortcut.Keys)
                    {
                        var button = new System.Windows.Controls.Button();
                        button.Style = styleShortcutButton;
                        button.Content = key;
                        itemDockPanel.Children.Add(button);
                    }
                    var descLabel = new TextBlock();
                    descLabel.Text = shortcut.Action;
                    descLabel.Style = styleShortcutDescLabel;
                    itemDockPanel.Children.Add(descLabel);
                    wrapper.Children.Add(itemDockPanel);
                }
                MapPanel.Children.Add(wrapper);

            }
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Hide();
                this.ExitFullscreen();
            }
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void ShareMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/pluveto/ShortcutMap/labels/Share");
        }
    }
    public class NewLine : FrameworkElement
    {
        public NewLine()
        {
            Height = 0;
            var binding = new System.Windows.Data.Binding
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(WrapPanel), 1),
                Path = new PropertyPath("ActualHeight")
            };
            BindingOperations.SetBinding(this, WidthProperty, binding);
        }
    }

}
