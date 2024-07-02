using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using YearBook.Factories.Interfaces;

namespace YearBook.Factories
{
    internal class AbstractFactory : IAbstractFactory
    {
        public readonly IServiceProvider serviceProvider;
        public AbstractFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public TService? GetService<TService>()
        {
            return this.serviceProvider.GetService<TService>();
        }
    }
}
