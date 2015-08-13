using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoveFilesToAmazonS3.Entities
{
    public class FileSources : List<FileSource>
    {
      
    }

    public class FileSource
    {
        public string S3FolderName { get; set; }
        public string BucketName { get; set; }
        public string LocalDirectoryName { get; set; }
        public string LocalDirectoryPath { get; set; }
    }
}
