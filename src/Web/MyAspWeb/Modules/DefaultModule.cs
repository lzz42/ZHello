using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;

namespace MyAspWeb.Modules
{
    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Controllers.CharacterRepository>().As<Controllers.ICharacterRepository>(); 
        }
    }
}
