using Ninject;
using SubmissionProvider.Exceptions;
using SubmissionProvider.Interfaces;
using System;
using System.Reflection;

namespace SubmissionProvider
{
    public class Program
    {
        static void Main(string[] args)
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            ILogger logger = kernel.Get<ILogger>();

            FeedSubmissionProvider FeedSubmissionProvider = kernel.Get<FeedSubmissionProvider>();

            if (args.Length == 0)
            {
                logger.Log("\nPlease provide file path ...");
                return;
            }

            string inputPath = args[0];

            try
            {
                FeedSubmissionProvider.ProcessFeeds(inputPath);
            }
            catch(InvalidFilePathException)
            {
                logger.Log("\nFile not exists, Please provide valid Input Path ...");
            }
            catch (InvalidSourceException ex)
            {
                logger.Log($"\nBad File format {ex} ...");
            }
            catch (Exception ex)
            {
                logger.Log($"\nCritical Exception occure at {ex} ");
            }
            Console.ReadLine();
        }
    }

}
