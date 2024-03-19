using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using jerry_manager.Core;
using jerry_manager.Core.FileSystem;
using File = jerry_manager.Core.FileSystem.File;

namespace jerry_manager.Model;

public class SearchModel
{
    #region Variables

    private List<string> m_ArchiveFormats = new() { ".7z", ".rar", ".zip" };
    public ObservableCollection<FileSystemObject> FileObjects { get; set; }

    #endregion

    #region Constructors

    public SearchModel()
    {
        FileObjects = new();
    }

    #endregion

    #region Methods

    public void SearchInFileSystem(SearchParameters parameters)
    {
        if (FileObjects.Count() != 0)
            Application.Current.Dispatcher.Invoke(() => FileObjects.Clear());

        DirectoryInfo directoryInfo = new DirectoryInfo(parameters.SearchPath);
        List<FileSystemObject> archiveFiles = new();
        List<FileInfo> files = directoryInfo.GetFiles("*" + parameters.SearchName + "*.*", SearchOption.AllDirectories)
            .Where(i => !m_ArchiveFormats.Any(j => i.Name.EndsWith(j)))
            .ToList();
        List<FileInfo> archives = directoryInfo.GetFiles().Where(i => m_ArchiveFormats.Any(j => i.Name.EndsWith(j)))
            .ToList();
        List<DirectoryInfo>directories = directoryInfo
            .GetDirectories("*" + parameters.SearchName + "*.*", SearchOption.AllDirectories).ToList();

        if (parameters.IsSearchInArchives)
        {
            foreach (var archive in archives)
            {
                try
                {
                    archiveFiles.AddRange(LoadArchive(new Archive(archive)));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        if (parameters.LeftDate is not null && parameters.RightDate is not null)
        {
            files = files.Where(i =>
                i.LastWriteTime >= parameters.LeftDate && i.LastWriteTime <= parameters.RightDate).ToList();
            directories = directories.Where(i =>
                i.LastWriteTime >= parameters.LeftDate && i.LastWriteTime <= parameters.RightDate).ToList();
            archives = archives.Where(i =>
                i.LastWriteTime >= parameters.LeftDate && i.LastWriteTime <= parameters.RightDate).ToList();
            archiveFiles = archiveFiles.Where(i =>
                i.DateModified >= parameters.LeftDate && i.DateModified <= parameters.RightDate).ToList();
        }

        if (parameters.Attributes is not null && parameters.Attributes.Count != 0)
        {
            foreach (var attribute in parameters.Attributes)
            {
                files = files.Where(i => i.Attributes.HasFlag(attribute)).ToList();
                directories = directories.Where(i => i.Attributes.HasFlag(attribute)).ToList();
                archives = archives.Where(i => i.Attributes.HasFlag(attribute)).ToList();
            }
        }

        foreach (var directory in directories)
        {
            Application.Current.Dispatcher.Invoke(() => FileObjects.Add(new Folder(directory)));
        }

        foreach (var file in files)
        {
            Application.Current.Dispatcher.Invoke(() => FileObjects.Add(new File(file)));
        }

        foreach (var archive in archives)
        {
            Application.Current.Dispatcher.Invoke(() => FileObjects.Add(new Archive(archive)));
        }

        foreach (var archiveFile in archiveFiles)
        {
            Application.Current.Dispatcher.Invoke(() => FileObjects.Add(archiveFile));
        }
    }

    public List<FileSystemObject> LoadArchive(FileSystemObject fileObject)
    {
        var selectedFileObject = fileObject as Archive;
        switch (selectedFileObject!.ArchiveType)
        {
            case ArchiveType.ZIP:
                return Load_ZIP(selectedFileObject);
            case ArchiveType.RAR:
                return Load_RAR(selectedFileObject);
            case ArchiveType.SevenZip:
                return Load_7Zip(selectedFileObject);
            default:
                throw new FileLoadException("Illegal format type.");
        }
    }

    private List<FileSystemObject> Load_ZIP(FileSystemObject file)
    {
        List<FileSystemObject> result = new List<FileSystemObject>();
        String curPath = file.Path + "\\";
        var path = file.Path.Substring(0, file.Path.LastIndexOf(".zip", StringComparison.Ordinal) + 4);
        using (var archive = new Aspose.Zip.Archive(path))
        {
            foreach (var entry in archive.Entries)
            {
                if (entry.IsDirectory)
                {
                    result.Add(new Folder(entry, path));
                }
                else
                {
                    if (m_ArchiveFormats.Any(j => entry.Name.EndsWith(j)))
                    {
                        result.Add(new Archive(entry, path));
                    }
                    else
                    {
                        result.Add(new File(entry, path));
                    }
                }
            }
        }

        return result;
    }

    private List<FileSystemObject> Load_RAR(FileSystemObject file)
    {
        List<FileSystemObject> result = new List<FileSystemObject>();
        String curPath = file.Path + "\\";
        var path = file.Path.Substring(0, file.Path.LastIndexOf(".rar", StringComparison.Ordinal) + 4);
        using (var archive = new Aspose.Zip.Rar.RarArchive(path))
        {
            foreach (var entry in archive.Entries)
            {
                if (entry.IsDirectory)
                {
                    result.Add(new Folder(entry, path));
                }
                else
                {
                    if (m_ArchiveFormats.Any(j => entry.Name.EndsWith(j)))
                    {
                        result.Add(new Archive(entry, path));
                    }
                    else
                    {
                        result.Add(new File(entry, path));
                    }
                }
            }
        }

        return result;
    }

    private List<FileSystemObject> Load_7Zip(FileSystemObject file)
    {
        List<FileSystemObject> result = new List<FileSystemObject>();
        string curPath = file.Path + "\\";
        var path = file.Path.Substring(0, file.Path.LastIndexOf(".7z", StringComparison.Ordinal) + 3);
        using (var archive = new Aspose.Zip.SevenZip.SevenZipArchive(path))
        {
            foreach (var entry in archive.Entries)
            {
                if (entry.IsDirectory)
                {
                    result.Add(new Folder(entry, path));
                }
                else
                {
                    if (m_ArchiveFormats.Any(j => entry.Name.EndsWith(j)))
                    {
                        result.Add(new Archive(entry, path));
                    }
                    else
                    {
                        result.Add(new File(entry, path));
                    }
                }
            }
        }

        return result;
    }

    #endregion
}