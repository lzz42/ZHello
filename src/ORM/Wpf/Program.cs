using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application app = new Application();
            app.ShutdownMode = ShutdownMode.OnLastWindowClose;
            //Func1(app);
            //Func2(app);
            //Func3(app);
            ShowModelWindow(app);
        }

        static void ShowModelWindow(Application app)
        {
            MainWindow main = new MainWindow();
            main.Content = new Model.StudentInformationControl();
            app.Run(main);
        }

        static void Func1(Application app)
        {
            MainWindow main = new MainWindow();
            app.Run(main);
        }

        static void Func2(Application app)
        {
            MainWindow main = new MainWindow();
            app.MainWindow = main;
            main.Show();
            app.Run();

        }
        static void Func3(Application app)
        {
            app.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            app.Run();
        }
    }
}

/*
 * WPF 基础
 * 应用程序管理
 * 1.创建WPF应用程序
 * 两种方式：
 * a.App.xaml中定义
 *  StartupUri="MainWindow.xaml"
 * b.自定义类自定义Main方法
 * 
 * 2.应用程序退出
 *  ShutdownMode:OnLastWindowClose(默认)，OnExplicitShutdown,OnMainWindowClose
 * a. App.xaml 中定义
 *  ShutdownMode="OnExplicitShutdown"
 * b. 自定义类中 必须在Run方法前调用
 * 
 * 3.定义事件
 * xaml 后台代码
 * 
 * 4.wpf应用程序的生命周期
 * 
 * 5.窗体
 * 由xaml和后台代码组成，或者将后台代码放到xaml中
 * <x:code>
 * <![CDATA[
 * //TODO:your C# code
 * ]]>
 * </x:code>
 * 
 * 6.窗体的生命周期
 *  a.SourceInitiated=>b.Actived=>c.Loaded=>d.ContentRender=>e.Deactived=>f.Closed
 *  
 * 7.其他窗体属性-常用
 *  a.WindowStyle - 窗体边框模式,ResizeMode,WindowStartupLocation,WindowState
 *  b.Title,TopMost,ShowInTaskbar
 *  
 * 8.容器控件
 * StackPanel：堆叠方式显式控件 
 * WrapPanel:以流的方式显式控件，从左到右从上到下
 * DockPanel:基本结构布局方式设置布局
 * 
 
     */
