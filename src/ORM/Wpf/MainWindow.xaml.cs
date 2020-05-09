using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Info.RecordMethodInfo();
        }

        private void Canvas_Click2(object sender, RoutedEventArgs e)
        {
            if(e.Source is Button)
            {
                var btn = e.Source as Button;
                MessageBox.Show(btn.Content.ToString(),"info");
            }
        } 
        /*
#region wpf

protected override void OnInitialized(EventArgs e)
{
   Info.RecordMethodInfo();
}
protected override void OnActivated(EventArgs e)
{
   Info.RecordMethodInfo();
}
protected override void OnDeactivated(EventArgs e)
{
   Info.RecordMethodInfo();
}
protected override void OnContentRendered(EventArgs e)
{
   Info.RecordMethodInfo();
}
protected override void OnClosing(CancelEventArgs e)
{
   Info.RecordMethodInfo();
   if (MessageBox.Show("Are you sure exit ?", "exit", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel) == MessageBoxResult.Cancel)
   {
       e.Cancel = true;
   }
}
protected override void OnClosed(EventArgs e)
{
   Info.RecordMethodInfo();
}
#endregion
*/

    }
}
