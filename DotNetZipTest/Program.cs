using Ionic.Zip;
using System;

namespace DotNetZipTest
{
    class Program
    {

        private static void Usage()
        {
            Console.WriteLine("usage:\n  ZipDir <ZipFileToCreate> <directory>");
            Environment.Exit(1);
        }

        static void Main(string[] args)
        {
            if (args.Length != 2) Usage();
            if (!System.IO.Directory.Exists(args[1]))
            {
                Console.WriteLine("The directory does not exist!\n");
                Usage();
            }
            if (System.IO.File.Exists(args[0]))
            {
                Console.WriteLine("That zipfile already exists!\n");
                Usage();
            }
            if (!args[0].EndsWith(".zip"))
            {
                Console.WriteLine("The filename must end with .zip!\n");
                Usage();
            }

            string TargetZipFile = args[0];
            string SourceDirectory = args[1];
            try
            {
                var progress = new ZipProgress(5);
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddProgress += progress.AddProgress;
                    zip.StatusMessageTextWriter = Console.Out;
                    zip.AddDirectory(SourceDirectory); // recurses subdirectories

                    var count = zip.Count; //number of files and sub folders added

                    //zip.BufferSize = 4096;
                    //zip.SortEntriesBeforeSaving = true;
                    zip.SaveProgress += progress.SaveProgress;
                    zip.Save(TargetZipFile);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("exception: " + ex);
            }
        }



    }
}
