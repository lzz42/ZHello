using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Model
{
    public class StudentModel : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }
        public uint age;
        public uint Age
        {
            get { return age; }
            set
            {
                age = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Age"));
            }
        }

        private float height;

        public float Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Height"));
            }
        }

        private bool hasCar;
        public bool HasCar
        {
            get { return hasCar; }
            set
            {
                hasCar = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HasCar"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
