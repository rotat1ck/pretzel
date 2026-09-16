using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Collections;
using SngLib;
using SongLib;

namespace SngCli
{
    public static class SngDecode
    {
        private static void SerializeMetadata(SngFile sngFile, string savePath)
        {
            KnownKeys.ValidateKeys(sngFile.Metadata);

            IniFile iniFile = new IniFile();
            foreach (var (key, value) in sngFile.Metadata)
            {
                iniFile.SetString("song", key, value);
            }

            iniFile.Save(savePath);
        }

        public static async Task DecodeSong(string sngPath)
        {
            var folderName = Path.GetFileNameWithoutExtension(sngPath);
            var parentFolder = Path.GetDirectoryName(sngPath);

            var outputFolder = Path.Combine(parentFolder!, folderName);
            
            SngFile sngFile = SngSerializer.LoadSngFile(sngPath);

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // create ini file from metadata
            SerializeMetadata(sngFile, Path.Combine(outputFolder, "song.ini"));

            // iterate through files and save them to disk
            foreach ((var name, var data) in sngFile.Files)
            {
                var filePath = Path.Combine(outputFolder, Path.Combine(name.Split("/")));
                var folder = Path.GetDirectoryName(filePath)!;
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                await data!.WriteToFileAsync(filePath);
            }
           
        }
    }
}