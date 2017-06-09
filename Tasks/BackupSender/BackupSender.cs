using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sisyphus.Tasks;
using NCrontab;
using Tamir.SharpSsh;

namespace Sisyphus
{
    public partial class BackupSender : TaskBase
    {
        public override ExecutionDelegate ExecutuionMethod => ExecuteTask;
        public override IEnumerable<Type> TaskSettings => new[] { typeof(BackupSenderSettings) };
    }

    public partial class BackupSender
    {

        private static string[] GetFileArray(BackupSenderSettings settings)
        {
            var cronShedule = CrontabSchedule.Parse(settings.DatePattern);
            var files = Directory.GetFiles(settings.Directory, settings.Mask, settings.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                .Where(t => DateTime.Now >= Directory.GetLastWriteTime(t).AddHours(settings.FileAge))
                .Where(fileName => cronShedule.GetNextOccurrences(File.GetCreationTime(fileName), File.GetCreationTime(fileName).AddMinutes(1)).Any());
            return files.ToArray();
        }

        public bool ExecuteTask()
        {
            var settings = GetSettings(typeof(BackupSenderSettings)) as BackupSenderSettings;
            var files = GetFileArray(settings);

            CreateLogRecord($"Files count {files.Length}");
            var sucessfullyTransferedFiles = new List<string>();
            var errorAccured = false;
            if (settings.Protocol == ProtocolType.Sftp)
            {
                try
                {
                    var sftp = new Sftp(settings.ServerAddress, settings.UserName, settings.Password);
                    sftp.Connect();
                    foreach (var file in files)
                    {
                        try
                        {
                            sftp.Put(file, settings.DestinationPath);
                            sucessfullyTransferedFiles.Add(file);
                            CreateLogRecord($"{file} successfully uploaded");
                        }
                        catch (Exception e)
                        {
                            CreateLogRecord(e);
                            errorAccured = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    CreateLogRecord(e);
                    return false;
                }
            }
            else
            {
                try
                {
                    var ftp = new ftp(settings.ServerAddress, settings.UserName, settings.Password);
                    foreach (var file in files)
                    {
                        try
                        {
                            var destPath = Path.Combine(settings.DestinationPath, Path.GetFileName(file));
                            ftp.upload(destPath.Replace("\\", "/"), file);
                            sucessfullyTransferedFiles.Add(file);
                            CreateLogRecord($"{file} successfully uploaded");
                        }
                        catch (Exception e)
                        {
                            CreateLogRecord(e);
                            errorAccured = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    CreateLogRecord(e);
                    return false;
                }
            }
            if (!settings.DeleteFile || settings.MoveToDirectory == string.Empty || settings.MoveToDirectory == settings.EmptyValue) return !errorAccured;

            foreach (var file in sucessfullyTransferedFiles)
            {
                try
                {
                    if (settings.DeleteFile)
                    {
                        File.Delete(file);
                    }
                    else
                    {
                        File.Move(file, Path.Combine(settings.MoveToDirectory, Path.GetFileName(file)));
                        CreateLogRecord($"{file} successfully moved to folder {settings.MoveToDirectory}");
                    }
                }
                catch (Exception e)
                {
                    CreateLogRecord(e);
                    errorAccured = true;
                }
            }

            return !errorAccured;
        }
    }
}
