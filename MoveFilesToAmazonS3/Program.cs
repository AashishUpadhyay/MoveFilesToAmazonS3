using MoveFilesToAmazonS3.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoveFilesToAmazonS3
{
    class Program
    {
        static void Main(string[] args)
        {
            var moveFilesToAmazonS3Processor = new MoveFilesToAmazonS3Processor();
            var fileSources = Helpers.Helpers.Load<FileSources>("FileSources.xaml");

            try
            {
                moveFilesToAmazonS3Processor.Upload(fileSources);
            }
            catch (Exception ex)
            {
                Helpers.Helpers.LogExceptions(ex.Message);
            }
        }
    }
}
