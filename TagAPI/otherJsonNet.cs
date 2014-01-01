using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace TagAPIx
{
    class otherJsonNet
    {
        /// <summary>
        /// Parses the minecraft json related to a specific version.
        /// </summary>
        /// <param name="jsonFile">The json file for a sepcific version. Example: "C:\LOCATION\1.6.2.json"</param>
        /// <returns>Returns a dictonary with all of the elements.</returns>
        internal static Dictionary<string, string[]> getVersionData(string jsonFile)
        {
            string archType = "32";
            var json = System.IO.File.ReadAllText(jsonFile);
            dynamic version = JsonConvert.DeserializeObject(json);
            string[] libraries = { "" };
            string[] urlLibraries = { "" };
            int l = 0;
            string[] natives = { "" };
            int n = 0;
            foreach (var param in version.libraries)
            {
                bool isNative = false;
                bool addFile = true;
                bool modFile = false;
                if (param.rules != null)
                {
                    foreach (var entry in param.rules)
                    {
                        if (entry.action == "allow")
                        {
                            if (entry.os != null)
                            {
                                if (entry.os.name != "windows")
                                {
                                    addFile = false;
                                }
                            }
                        }
                        else
                        {
                            if (entry.os != null)
                            {
                                if (entry.os.name == "windows")
                                {
                                    addFile = false;
                                }
                            }
                            else
                            {
                                addFile = false;
                            }
                        }
                    }
                }
                if (param.url != null)
                {
                    modFile = true;
                }

                if (addFile)
                {
                    string fileName = param.name;
                    string[] colonSplit = fileName.Split(new char[] { ':' }, 3);
                    string[] folderSplit = colonSplit[0].Split(new char[] { '.' });
                    string compileFolder = "";
                    foreach (string entry in folderSplit)
                    {
                        compileFolder += entry + @"\";
                    }
                    string compileSplit = "";
                    if (param.natives != null)
                    {
                        isNative = true;
                        compileSplit = compileFolder + @"\" + colonSplit[1] + @"\" + colonSplit[2] + @"\" + colonSplit[1] + "-" + colonSplit[2] + "-" + param.natives.windows + ".jar";
                    }
                    else
                    {
                        compileSplit = compileFolder + @"\" + colonSplit[1] + @"\" + colonSplit[2] + @"\" + colonSplit[1] + "-" + colonSplit[2] + ".jar";
                    }
                    string compileComplete = compileSplit.Replace("${arch}", archType);
                    if (l > libraries.Length - 1)
                    {
                        Array.Resize(ref libraries, libraries.Length + 1);
                        Array.Resize(ref urlLibraries, libraries.Length + 1);
                    }
                    libraries[l] = compileComplete;
                    if (modFile)
                    {
                        urlLibraries[l] = param.url;
                    }
                    else
                    {
                        urlLibraries[l] = "";
                    }

                    l++;
                    if (isNative)
                    {
                        if (n > natives.Length - 1)
                        {
                            Array.Resize(ref natives, natives.Length + 1);
                        }
                        natives[n] = compileComplete;
                        n++;
                    }
                }
            }
            string tmpAssets = "legacy";
            if (version.assets != null)
            {
                tmpAssets = version.assets;
            }
            Dictionary<string, string[]> dict = new Dictionary<string, string[]>{
                {"id"                  , new string[] { version.id }},
                {"time"                , new string[] { version.time }},
                {"releaseTime"         , new string[] { version.releaseTime }},
                {"type"                , new string[] { version.type }},
                {"minecraftArguments"  , new string[] { version.minecraftArguments }},
                {"mainClass"           , new string[] { version.mainClass }},
                {"assets"              , new string[] { tmpAssets }},
              //{"libraries"           , new string[] { "net/sf/jopt-simple/jopt-simple/4.5/jopt-simple-4.5.jar" "etc" "etc" }},
                {"libraries"           , libraries },
                {"urlLibraries"        , urlLibraries },
              //{"natives"             , new string[] { "net/sf/jopt-simple/jopt-simple/4.5/jopt-simple-4.5.jar" "etc" "etc" }},
                {"natives"             , natives   }
            };
            return dict;
        }

        /// <summary>
        /// Parses the minecraft verion list.
        /// </summary>
        /// <param name="jsonFile">The json file for the list of versions. Example: "C:\LOCATION\verions.json"</param>
        /// <returns>Returns a dictonary with all of the elements.</returns>
        internal static Dictionary<string, string[]> getVersionList(string jsonFile)
        {
            var json = System.IO.File.ReadAllText(jsonFile);
            dynamic version = JsonConvert.DeserializeObject(json);
            Dictionary<string, string[]> dict = new Dictionary<string, string[]>{
                {"TagCraftMC", new string[] { version.latest.release, version.latest.snapshot }},
              //{"1.6.4",       new string[] { "time", "releaseTime", "type" }}
            };
            foreach (var entry in version.versions)
            {
                string keyString = entry.id;
                string[] arrString = { entry.time, entry.releaseTime, entry.type };
                dict.Add(keyString, arrString); //Odd errors if put in directly.
            }
            return dict;
        }

        /// <summary>
        /// Parses the minecraft index file for minecraft assets.
        /// </summary>
        /// <param name="jsonFile">The json file for the list of assets. Example: "C:\LOCATION\verions.json"</param>
        /// <returns>Returns a dictonary with all of the elements.</returns>
        internal static Dictionary<string, string[]> getAssetsList(string jsonFile)
        {
            var json = System.IO.File.ReadAllText(jsonFile);
            dynamic assets = JsonConvert.DeserializeObject(json);
            Dictionary<string, string[]> dict = new Dictionary<string, string[]>
            {
                //{"objectFileLocation", new string[] { "hash", "size" }},
            };
            var values = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(assets.objects));
            foreach (KeyValuePair<string, dynamic> entry in values)
            {
                dict.Add(entry.Key, new string[] { entry.Value.hash, entry.Value.size });
            }
            return dict;
        }
    }
}

