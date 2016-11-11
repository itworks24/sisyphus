using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Sisyphus.Common;
using Sisyphus.Logging;
using Settings;

namespace Sisyphus.Settings
{
    public class SettingsIni
    {
        private const string SettingsFileName = "SisyphusSettings.ini";

        public static readonly string FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                                        "Sisyphus");

        protected static readonly string SettingsFilePath = Path.Combine(FolderPath,
                                                                 SettingsFileName);

        protected bool NotAllSettingsExist;
        protected const string NotSetValue = "ValueNotSet";

        public static void SaveDefaultSettingsForAllClasses()
        {
            var logger = new TaskLogging();
            try
            {
                CommonFunctions.LoadAllSolutionAssemblies();
            }
            catch (Exception e)
            {
                logger.CreateLogRecord(e);
            }

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().OrderBy(t => t.FullName))
            {
                var q = from t in assembly.GetTypes()
                        where t.IsClass && t.IsSubclassOf(typeof(SettingsRepresent))
                        select t;
                try
                {
                    q.ToList().ForEach(t => Activator.CreateInstance(t, ""));
                }
                catch (Exception e)
                {
                    logger.CreateLogRecord(e);
                }
            }
        }

        protected static Ini GetSettingsIni()
        {
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);
            var settingsIni = new Ini(SettingsFilePath);

            return settingsIni;
        }

        protected static void SaveSettingsIni(Ini settingsIni)
        {
            settingsIni.Save();
            try
            {
                File.Encrypt(SettingsFilePath);
            }
            catch (Exception e)
            {
                return;
            }    
        }

        protected bool SectionExist(string sectionName)
        {
            var settingsIni = GetSettingsIni();
            return (settingsIni.GetSections().Count(t => t == sectionName) != 0);
        }

        public static void EditIniFileManual()
        {
            SaveDefaultSettingsForAllClasses();
            GetSettingsIni();
            var startInfo = new ProcessStartInfo("notepad.exe", SettingsFilePath);
            Process.Start(startInfo);
        }
        public static List<string> GetSections(string typeSectionName)
        {
            return GetSettingsIni().GetSections().Where(t => t.StartsWith($"{typeSectionName}.")).ToList();
        }
        public static List<string> GetSectionsByGroupName(string groupName)
        {
            return GetSettingsIni().GetSections().Where(t => t.EndsWith($".{groupName}")).ToList();
        }

        public static string[] GetSeparatedSection(string section)
        {
            if (!section.Contains('.')) return new string[] { section, "default"};
            var splited = section.Split('.');
            return splited;
        }
        public static string GetClassName(string section)
        {
            return GetSeparatedSection(section)[0];
        }
        public static string GetGroupName(string section)
        {
            return GetSeparatedSection(section)[1];
        }
    }
}
