using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using Lux.IO;
using Remotus.Base;
using Remotus.Base.Scripting;

namespace Remotus.Plugins.Scripting
{
    public class FileSystemScriptDataStore : IScriptDataStore
    {
        private IFileSystem _fileSystem;

        public FileSystemScriptDataStore()
        {
            _fileSystem = new FileSystem();
            Directory = PathHelper.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Scripts");
            SearchOption = SearchOption.TopDirectoryOnly;
        }

        public IEnumerable<Script> GetScripts()
        {
            var serializer = new XmlSerializer(typeof(Script));

            var filePaths = _fileSystem.EnumerateFiles(Directory, "*.xml", SearchOption);
            foreach (var filePath in filePaths)
            {
                using (var stream = _fileSystem.OpenFile(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var obj = serializer.Deserialize(stream);
                    var script = (Script) obj;
                    yield return script;
                }
            }
        }

        public string Directory { get; set; }
        public SearchOption SearchOption { get; set; }
    }
}