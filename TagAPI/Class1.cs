using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Xml.Linq;
using System.IO;

namespace TagAPIx
{
    public class Class1
    {
        //how this works... 
        //folder names read by main application and shown in listbox
        //user selects and version gets saved in versionselect.txt
        //(this application)application reads versionselect.txt (-) and reads json and makes arguments and then create a new txt file and run the argument.
        //go back to main app.

        internal static Dictionary<string, string[]> versionData = new Dictionary<string, string[]>
        {
            //{"id"                  , new string[] { "1.6.4" }},
            //{"time"                , new string[] { "2013-09-19T10:52:37-05:00" }},
            //{"releaseTime"         , new string[] { "2013-09-19T10:52:37-05:00" }},
            //{"type"                , new string[] { "release" }},
            //{"minecraftArguments"  , new string[] { "--username ${auth_player_name} --session ${auth_session} --version ${version_name} --appDir ${app_directory} --assetsDir ${app_assets} --uuid ${auth_uuid} --accessToken ${auth_access_token}" }},
            //{"mainClass"           , new string[] { "net.minecraft.client.main.Main" }},
            //{"assets"              , new string[] { "legacy" or "1.6.4" }},
            //{"libraries"           , new string[] { "net\sf\jopt-simple\jopt-simple\4.5\jopt-simple-4.5.jar" "etc" "etc" }},
            //{"urlLibraries"        , new string[] { "MODWEBSITE" "" "etc" }}, //for Mods the declare there own downloads.
            //{"natives"             , new string[] { "net\sf\jopt-simple\jopt-simple\4.5\jopt-simple-4.5.jar" "etc" "etc" }},
        };

        internal static Dictionary<string, string[]> assetsList = new Dictionary<string, string[]>
        {
            //{"objectFileLocation", new string[] { "hash", "size" }},
            //{"objectFileLocation", new string[] { "hash", "size" }},
            //{"objectFileLocation", new string[] { "hash", "size" }},
            //{"etc", new string[] { "etc", "etc" }},
            //{"etc", new string[] { "etc", "etc" }},
            //{"etc", new string[] { "etc", "etc" }},
        };

        

    
        static string buildArguments = "";
        static string mcSave =  Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.minecraft/";
        //static string mcStartRam = "";
        static string mcMaxRam = System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.minecraft/TagCraftMC Files/Settings/memory.txt");


         public static void main()
        {
            //Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData
            string text = System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.minecraft/TagCraftMC Files/Settings/versionselect.txt");
            string username = System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.minecraft/TagCraftMC Files/Settings/taguser.txt");

            // Display the file contents to the console. Variable text is a string.
            //System.Console.WriteLine("Contents of WriteText.txt = {0}", text);
            

            //screw this 1.7.4 make it to text ah ah ah. 
               // string mcLocation = "C:" + @"\" + "Users" + @"\" + "User" + @"\" + "AppData" + @"\" + "Roaming" + @"\" + ".minecraft" + @"\";
               string mcLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.minecraft/";

                string fileName = mcLocation + @"\versions\" + text + @"\" + text + ".json";


                if (File.Exists(fileName))
                {
                   versionData = otherJsonNet.getVersionData(fileName);
                }
                else
                {
                    //status = subString + " / Version data file missing.";
                }
                string mojangIntelTrick = "-XX:HeapDumpPath=MojangTricksIntelDriversForPerformance_javaw.exe_minecraft.exe.heapdump";
                string mcNatives = "-Djava.library.path=\"" + mcLocation + @"\versions\" + text + @"\" + text + "_TagCraftMC\"";
                string mcLibraries = "-cp ";
                foreach (string entry in versionData["libraries"])
                {
                    mcLibraries = mcLibraries + "\"" + mcLocation + @"\libraries\" + entry + "\";";
                }
                string mcJar = "\"" + mcLocation + @"\versions\" + text + @"\" + text + ".jar\"";
                string mcClass = versionData["mainClass"][0];
                string mcArgs = versionData["minecraftArguments"][0];
                mcArgs = mcArgs.Replace("${auth_player_name}", username);
                
                mcArgs = mcArgs.Replace("${auth_session}", "OFFLINE_MODE");
                mcArgs = mcArgs.Replace("${auth_uuid}", "OFFLINE_MODE");
                mcArgs = mcArgs.Replace("${auth_access_token}", "OFFLINE_MODE");

                mcArgs = mcArgs.Replace("${assets_index_name}", versionData["assets"][0]);
                mcArgs = mcArgs.Replace("${version_name}", text);
                mcArgs = mcArgs.Replace("${user_properties}", "{}");
                mcArgs = mcArgs.Replace("${game_directory}", "\"" + mcSave + "\"");
                mcArgs = mcArgs.Replace("${game_assets}", "\"" + mcLocation + "\\assets\\virtual\\legacy\"");
                mcArgs = mcArgs.Replace("${assets_root}", "\"" + mcLocation + "\\assets\"");
                //buildArguments = mojangIntelTrick + " " + mcStartRam + " " + mcMaxRam + " " + mcNatives + " " + mcLibraries + mcJar + " " + mcClass + " " + mcArgs;
                //Removed the min memory option... dont like it. 
                buildArguments = mojangIntelTrick + " -Xmx" + mcMaxRam + " " + mcNatives + " " + mcLibraries + mcJar + " " + mcClass + " " + mcArgs;

                string[] lines = {buildArguments};
                // WriteAllLines creates a file, writes a collection of strings to the file, 
                // and then closes the file.
                //  oReadp = IO.File.OpenText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.minecraft/TagCraftMC Files/Settings/versions/" + ver + ".txt")

                System.IO.File.WriteAllLines((Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.minecraft/TagCraftMC Files/Settings/versions/" + text + ".txt"), lines);
              
           // textBox1.Text = buildArguments;

           }
        }
    }
 
