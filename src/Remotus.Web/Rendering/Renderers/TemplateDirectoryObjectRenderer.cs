using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Remotus.Web.Rendering
{
    public class TemplateDirectoryObjectRenderer : ObjectRendererCollection
    {
        private FileSystemWatcher _watcher;

        public TemplateDirectoryObjectRenderer(string directory)
        {
            Directory = directory;

            Load();
            
            _watcher = new FileSystemWatcher(Directory);
            _watcher.Changed += WatcherOnEvent;
            _watcher.Deleted += WatcherOnEvent;
            _watcher.Created += WatcherOnEvent;
            _watcher.Renamed += WatcherOnEvent;
            _watcher.EnableRaisingEvents = true;
        }

        public string Directory { get; private set; }
        


        protected void Load()
        {
            var filePaths = System.IO.Directory.EnumerateFiles(Directory, "*.render", SearchOption.AllDirectories);
            foreach (var filePath in filePaths)
            {
                // todo: load via web.config?
                LoadFile(filePath);
            }
        }

        protected virtual void LoadFile(string filePath)
        {
            var template = new FileTemplateObjectRenderer.FileTemplate();
            template.Load(filePath, watch: false);
            var renderer = new FileTemplateObjectRenderer(template);
            Children.Add(renderer);
        }


        private void WatcherOnEvent(object sender, FileSystemEventArgs e)
        {
            var renameArgs = e as RenamedEventArgs;

            var renderer =
                Children.OfType<FileTemplateObjectRenderer>()
                    .FirstOrDefault(x => Lux.IO.PathHelper.AreEquivalent(x.Template.FilePath, renameArgs?.OldFullPath ?? e.FullPath));

            if (e.ChangeType == WatcherChangeTypes.Created ||
                e.ChangeType == WatcherChangeTypes.Renamed)
            {
                if (renderer != null)
                {
                    Children.Remove(renderer);
                    var template = renderer?.Template as FileTemplateObjectRenderer.FileTemplate;
                    template?.Dispose();
                }
                LoadFile(e.FullPath);
            }
            else if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                if (renderer != null)
                {
                    Children.Remove(renderer);
                    var template = renderer?.Template as FileTemplateObjectRenderer.FileTemplate;
                    template?.Dispose();
                }
            }
            else if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                var template = renderer?.Template as FileTemplateObjectRenderer.FileTemplate;
                if (template != null)
                {
                    template.Load(e.FullPath, watch: false);
                }
            }
        }

    }
}