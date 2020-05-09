using System;
using System.Collections.Generic;
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

namespace Wpf.Model
{
    /// <summary>
    /// StudentInformationControl.xaml 的交互逻辑
    /// </summary>
    public partial class StudentInformationControl : UserControl
    {
        private StudentModel mStudent { get; set; }
        public StudentInformationControl()
        {
            InitializeComponent();
            mStudent = new StudentModel()
            {
                Name = "testName",
                Age = 20,
                Height = 65,
                HasCar = true,
            };
            DefineBinding();
        }
        
        private void DefineBinding()
        {
            Binding mBinding = new Binding();
            mBinding.Source = mStudent;
            mBinding.Path = new PropertyPath("Name");
            BindingOperations.SetBinding(this.label_name, Label.ContentProperty, mBinding);
            BindingOperations.SetBinding(this.tb_name, TextBlock.TextProperty, new Binding("Name") { Source = mStudent });
            BindingOperations.SetBinding(this.tb_height, TextBlock.TextProperty, new Binding("Height") { Source = mStudent });
            BindingOperations.SetBinding(this.tb_age, TextBlock.TextProperty, new Binding("Age") { Source = mStudent });
            BindingOperations.SetBinding(this.tb_hasCar, TextBlock.TextProperty, new Binding("HasCar") { Source = mStudent });
        }


        private void btn_write_Click(object sender, RoutedEventArgs e)
        {
            var temp = Environment.TickCount;
            mStudent.Name =temp.ToString();
            var rand = new Random(7);
            mStudent.Age = (uint)(rand.Next(10, 80)+ temp%10);
            mStudent.Height = (uint)(rand.Next(200, 1000)+temp&10);
            mStudent.HasCar = (temp & 1) == 0;
        }
    }
}
