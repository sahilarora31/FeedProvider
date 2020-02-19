using Ninject.Modules;
using SubmissionProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionProvider
{
    public class NinjectConfig : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<Logger>().InSingletonScope();
            Bind<FileParser>().To<FileParser>().InSingletonScope();
            Bind<FeedSubmissionProvider>().To<FeedSubmissionProvider>().InSingletonScope();

        }
    }
}
