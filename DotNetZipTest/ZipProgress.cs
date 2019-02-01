using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;

namespace DotNetZipTest
{
    public class ZipProgress
    {
        private bool pbSet;
        private int phaseStep = 10;
        private int numEntriesSaved = 0;
        private int numEntriesToAdd = 0;
        private int numEntriesAdded = 0;
        private int numFilesToExtract = 0;
        private int epCycles;
        long maxBytesXferred = 0;

        public ZipProgress()
        {
            return;
        }

        public ZipProgress(int phaseStep)
        {
            this.phaseStep = phaseStep;
        }

        public void AddProgress(object sender, AddProgressEventArgs e)
        {
            switch (e.EventType)
            {
                case ZipProgressEventType.Adding_Started:
                    Console.WriteLine("Adding files to the zip...");
                    break;
                case ZipProgressEventType.Adding_AfterAddEntry:
                    if (!e.CurrentEntry.FileName.EndsWith("/"))
                    {
                        numEntriesAdded++;
                        if (numEntriesAdded % phaseStep == 0)
                            Console.WriteLine($"Adding file {numEntriesAdded} | {e.CurrentEntry.FileName}");
                    }
                    break;
                case ZipProgressEventType.Adding_Completed:
                    Console.WriteLine("Added all files" );
                    break;
            }
        }

        public void SaveProgress(object sender, SaveProgressEventArgs e)
        {
            switch (e.EventType)
            {
                case ZipProgressEventType.Saving_Started:
                    numEntriesSaved = 0;
                    Console.WriteLine("Compressing started...");
                    pbSet = false;
                    break;

                case ZipProgressEventType.Saving_BeforeWriteEntry:
                    numEntriesSaved++;
                    if (numEntriesSaved % phaseStep == 0)
                        Console.WriteLine($"Compressing {e.CurrentEntry.FileName}");
                    if (!pbSet)
                    {
                        Console.WriteLine($"Total files to compress {e.EntriesTotal}");
                        pbSet = true;
                    }
                    //throw new Exception("Cancel or exit right here???"); //DEBUG
                    break;

                case ZipProgressEventType.Saving_Completed:
                    Console.WriteLine("Compressing all files completed.");
                    pbSet = false;
                    break;

                //case ZipProgressEventType.Saving_AfterWriteEntry:
                //    Console.WriteLine("Compressed one file per step");
                //    break;

                //case ZipProgressEventType.Saving_EntryBytesRead:
                //    //must be true == (e.BytesTransferred <= e.TotalBytesToTransfer);
                //    break;
            }
        }

        public void ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            switch (e.EventType)
            {
                case ZipProgressEventType.Extracting_BeforeExtractEntry:
                    if (!pbSet)
                    {
                        Console.WriteLine($"pb 1 max {numFilesToExtract}");
                        pbSet = true;
                    }
                    epCycles = 0;
                    //_pb2Set = false;
                    break;

                case ZipProgressEventType.Extracting_EntryBytesWritten:
                    epCycles++;
                    //Progress for each File extraction, not needed?
                    //if ((_epCycles % phaseStep) == 0)
                    //{
                    //    if (!_pb2Set)
                    //    {
                    //        _txrx.Send(String.Format("pb 2 max {0}", e.TotalBytesToTransfer));
                    //        _pb2Set = true;
                    //    }
                    //    _txrx.Send(String.Format("status Extracting {0} :: [{1}/{2}mb] ({3:N0}%)",
                    //                             e.CurrentEntry.FileName,
                    //                             e.BytesTransferred / (1024 * 1024),
                    //                             e.TotalBytesToTransfer / (1024 * 1024),
                    //                             ((double)e.BytesTransferred / (0.01 * e.TotalBytesToTransfer))
                    //                             ));
                    //    string msg = String.Format("pb 2 value {0}", e.BytesTransferred);
                    //    _txrx.Send(msg);
                    //}
                    //if (maxBytesXferred < e.BytesTransferred)
                    //    maxBytesXferred = e.BytesTransferred;
                    break;

                case ZipProgressEventType.Extracting_AfterExtractEntry:
                    Console.WriteLine("pb 1 step");
                    break;
            }
        }
    }
}
