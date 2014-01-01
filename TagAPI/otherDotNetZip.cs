using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zip;

namespace TagAPIx
{
    class otherDotNetZip
    {
        /// <summary>
        /// Decompress a zip file to a specified directory. Exclude by matching string.
        /// </summary>
        /// <param name="zipFileName">Location and file name of the zip file. Example: "C:\LOCATION\file.zip"</param>
        /// <param name="outputDirectory">Output of the files extracted. Example: "C:\LOCATION\"</param>
        /// <param name="excludeCard">Anything that matches any part of this string will not get extracted. Example: "blah"</param>
        public static void Extract(string zipFileName, string outputDirectory, string excludeCard = "")
        {
            bool exclude = true;
            if (excludeCard == "")
            {
                exclude = false;
            }
            ZipFile zip = ZipFile.Read(zipFileName);
            Directory.CreateDirectory(outputDirectory);
            foreach (ZipEntry e in zip)
            {
                if (exclude)
                {
                    if (!e.FileName.Contains(excludeCard))
                    {
                        e.Extract(outputDirectory, ExtractExistingFileAction.OverwriteSilently);
                    }
                }
                else
                {
                    e.Extract(outputDirectory, ExtractExistingFileAction.OverwriteSilently);
                }
            }
        }
    }
}