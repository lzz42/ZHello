using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAspWeb.Services
{
    public interface ISystemTime
    {
        DateTime Now { get; }
    }
}
