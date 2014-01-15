﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Xml.Linq;
using System.IO;
using Ionic.Zip;

namespace TagAPIx
{

    public class Class1
    {

       static string username = null;
       static string versionnumber = null;
       static string rememberaccount = null;
       static string debugmode = null;
       static string memory = null;
       static string memorypass = null;
       static string tagoptions = null;
       static string runtimecatch = null;
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
        //static string mcMaxRam = System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.minecraft/TagCraftMC Files/Settings/memory.txt");
        public static void extractfile()
        {
            string mcLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.minecraft/";
         //   string text = System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.minecraft/TagCraftMC Files/Settings/versionselect.txt");
                
            try
            {
                foreach (string entry in versionData["natives"])
                {
                //    otherDotNetZip.Extract(mcLocation + @"\libraries\" + entry, mcLocation + @"\versions\" + versionData["id"][0] + @"\" + versionData["id"][0] + "_TagCraftMC", "META-INF");
                    otherDotNetZip.Extract(mcLocation + @"\libraries\" + entry, mcLocation + @"\versions\" + versionnumber + @"\" + versionnumber + "_TagCraftMC", "META-INF");
                
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
            }
            //return status;
        }
        //-----------------------------------------

      

        public static void optionreader()
        {


            try
            {
                StreamReader reader = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.minecraft/TagCraftMC Files/Settings/options.txt");

                string a = null;

                do
                {
                    a = reader.ReadLine();

                    try
                    {
                        if (a.Contains("username:"))
                        {
                           username  = a.Replace("username:", "");

                      
                        }
                        else
                        {
                            // do nothing!
                        }

                        if (a.Contains("versionnumber:"))
                        {
                            versionnumber = a.Replace("versionnumber:", "");

                        }
                        else
                        {
                            // do nothing!
                        }

                        if (a.Contains("memory:"))
                        {
                            memory = a.Replace("memory:", "");

                        }
                        else
                        {
                            // do nothing!
                        }


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred: '{0}'", ex);

                    }

                } while (!(a == null));

                reader.Close();



            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: '{0}'", ex);

            }
        }

        //-------------------------------------
         public static void main()
        {
            try
            {
                //Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData
               // string text = System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.minecraft/TagCraftMC Files/Settings/versionselect.txt");
               // string username = System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.minecraft/TagCraftMC Files/Settings/taguser.txt");

                // Display the file contents to the console. Variable text is a string.
                //System.Console.WriteLine("Contents of WriteText.txt = {0}", text);


                //screw this 1.7.4 make it to text ah ah ah. 
                // string mcLocation = "C:" + @"\" + "Users" + @"\" + "User" + @"\" + "AppData" + @"\" + "Roaming" + @"\" + ".minecraft" + @"\";
                string mcLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.minecraft/";

                string fileName = mcLocation + @"\versions\" + versionnumber + @"\" + versionnumber + ".json";


                if (File.Exists(fileName))
                {
                    versionData = otherJsonNet.getVersionData(fileName);
                }
                else
                {
                    //status = subString + " / Version data file missing.";
                }
                string mojangIntelTrick = "-XX:HeapDumpPath=MojangTricksIntelDriversForPerformance_javaw.exe_minecraft.exe.heapdump";
                string mcNatives = "-Djava.library.path=\"" + mcLocation + @"\versions\" + versionnumber + @"\" + versionnumber + "_TagCraftMC\"";
                string mcLibraries = "-cp ";
                foreach (string entry in versionData["libraries"])
                {
                    mcLibraries = mcLibraries + "\"" + mcLocation + @"\libraries\" + entry + "\";";
                }
                string mcJar = "\"" + mcLocation + @"\versions\" + versionnumber + @"\" + versionnumber + ".jar\"";
                string mcClass = versionData["mainClass"][0];
                string mcArgs = versionData["minecraftArguments"][0];
                mcArgs = mcArgs.Replace("${auth_player_name}", username);

                mcArgs = mcArgs.Replace("${auth_session}", "OFFLINE_MODE");
                mcArgs = mcArgs.Replace("${auth_uuid}", "OFFLINE_MODE");
                mcArgs = mcArgs.Replace("${auth_access_token}", "OFFLINE_MODE");

                mcArgs = mcArgs.Replace("${assets_index_name}", versionData["assets"][0]);
                mcArgs = mcArgs.Replace("${version_name}", versionnumber);
                mcArgs = mcArgs.Replace("${user_properties}", "{}");
                mcArgs = mcArgs.Replace("${game_directory}", "\"" + mcSave + "\"");
                mcArgs = mcArgs.Replace("${game_assets}", "\"" + mcLocation + "\\assets\\virtual\\legacy\"");
                mcArgs = mcArgs.Replace("${assets_root}", "\"" + mcLocation + "\\assets\"");
                //buildArguments = mojangIntelTrick + " " + mcStartRam + " " + mcMaxRam + " " + mcNatives + " " + mcLibraries + mcJar + " " + mcClass + " " + mcArgs;
                //Removed the min memory option... dont like it. 
                buildArguments = mojangIntelTrick + " -Xmx" + memory + " " + mcNatives + " " + mcLibraries + mcJar + " " + mcClass + " " + mcArgs;

                string[] lines = { buildArguments };
                // WriteAllLines creates a file, writes a collection of strings to the file, 
                // and then closes the file.
                //  oReadp = IO.File.OpenText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.minecraft/TagCraftMC Files/Settings/versions/" + ver + ".txt")

                System.IO.File.WriteAllLines((Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.minecraft/TagCraftMC Files/Settings/versions/" + versionnumber + ".txt"), lines);

                // textBox1.Text = buildArguments;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
            }
           }
        }
    }
 
