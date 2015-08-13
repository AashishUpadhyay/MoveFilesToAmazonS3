using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using MoveFilesToAmazonS3.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MoveFilesToAmazonS3
{
    public class MoveFilesToAmazonS3Processor
    {
        private List<Task> parallelTasks = new List<Task>();
        IAmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(RegionEndpoint.APSoutheast1);
        TransferUtility utility;

        public void Upload(FileSources fileSources)
        {
            if (fileSources.Count() > 0)
            {
                utility = new TransferUtility(client);
                foreach (var source in fileSources)
                {
                    parallelTasks.Add(Task.Factory.StartNew(() => StartUpload(source)));
                }
            }

            Task.WaitAll(parallelTasks.ToArray());
        }

        private void StartUpload(FileSource fileSource)
        {
            try
            {
                var allDirectories = Helpers.Helpers.GetSubdirectoriesContainingOnlyFiles(fileSource.LocalDirectoryPath);

                foreach (var directory in allDirectories)
                {
                    var allFilesInADirectory = Directory.GetFiles(directory);

                    foreach (var file in allFilesInADirectory)
                    {
                        var testString = directory.Remove(0, fileSource.LocalDirectoryPath.Length);

                        var bucketName = fileSource.BucketName + @"/" + fileSource.S3FolderName + @"/" + fileSource.LocalDirectoryName + directory.Remove(0, fileSource.LocalDirectoryPath.Length).Replace("\\", "/");
                        var localFilePath = file;
                        UploadFileToAmazon(bucketName, localFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.Helpers.LogExceptions(ex.Message);
            }
        }

        private void UploadFileToAmazon(string bucketName, string localFilePath)
        {
            try
            {
                TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();
                request.BucketName = bucketName;
                request.FilePath = localFilePath;
                utility.Upload(request);
                File.Delete(localFilePath);
            }
            catch (Exception ex)
            {
                Helpers.Helpers.LogExceptions(ex.Message);
            }
        }
    }
}
