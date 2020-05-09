using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAspWeb.Models
{
    public class GameSetting
    {
        public string Lang { get; set; }
        public string Game { get; set; }
        public float Size { get; set; }
        public bool Free { get; set; }
        public string Level { get; set; }
    }
}
