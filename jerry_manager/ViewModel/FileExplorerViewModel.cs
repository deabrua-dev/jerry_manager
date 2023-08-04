using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using jerry_manager.Core.FileSystem;
using jerry_manager.Model;
using File = jerry_manager.Core.FileSystem.File;

namespace jerry_manager.ViewModel;

public class FileExplorerViewModel : INotifyPropertyChanged
{
    #region Variables
    
    private FileExplorerModel m_Model { get; set; }
    public ObservableCollection<Drive> Drives { get; set; }

    public string DriveFreeSpace
    {
        get => SelectedDrive.Size + " kb of " + SelectedDrive.TotalSize + " kb free";
    }
    public ObservableCollection<FileSystemObject> Items { get; set; }
    private FileSystemWatcher m_FileSystemWatcher = new();
    private FileSystemObject m_SelectedObject;
    public FileSystemObject SelectedFileObject
    {
        get => m_SelectedObject;
        set { m_SelectedObject = value; OnPropertyChanged("SelectedObject"); }
    }
    public List<FileSystemObject> SelectedFileObjects
    {
        get 
        {
            List<FileSystemObject> result = new List<FileSystemObject>();
            foreach (var item in Items) 
            { 
                if (item.IsSelected && item is not ParentFolder)
                {
                    result.Add(item);
                }
            }
            return result;
        }
    }

    private Drive m_SelectedDrive;

    public Drive SelectedDrive
    {
        get => m_SelectedDrive;
        set
        {
            m_SelectedDrive = value;
            OnPropertyChanged("SelectedDrive");

            CurrentPath = m_SelectedDrive.Path;
        }
    }
    
    public string CurrentPath
    {
        get => m_Model.CurrentPath;
        set 
        { 
            m_Model.CurrentPath = value;
            OnPropertyChanged("CurrentPath");
            if (SelectedFileObject is Archive)
            {
                m_Model.LoadArchive(SelectedFileObject, Items);
                m_FileSystemWatcher.EnableRaisingEvents = false;
            }
            else if (SelectedFileObject != null && SelectedFileObject.IsArchived)
            {
                m_Model.LoadInArchive(SelectedFileObject, Items);
            }
            else if (Directory.Exists(CurrentPath))
            {
                m_Model.LoadPath(Items);
                m_FileSystemWatcher.Path = m_Model.CurrentPath;
                m_FileSystemWatcher.EnableRaisingEvents = true;
            }
            else
            {
                m_FileSystemWatcher.EnableRaisingEvents = true;
            }
        }
    }

    #endregion

    #region Constructors

    public FileExplorerViewModel()
    {
        m_Model = new();
        Drives = new();
        Items = new();
        
        m_Model.LoadDrives(Drives);
        SelectedDrive = Drives[0];

        m_FileSystemWatcher.Changed += FileWatcher_Interacted;
        m_FileSystemWatcher.Created += FileWatcher_Interacted;
        m_FileSystemWatcher.Deleted += FileWatcher_Interacted;
        m_FileSystemWatcher.Renamed += FileWatcher_Interacted;
    }

    #endregion

    #region Methods

    private void FileWatcher_Interacted(object sender, FileSystemEventArgs e)
    {
        m_Model.LoadPath(Items);
    }
    
    public void DoubleClick()
    {
        try
        {
            if (SelectedFileObject == null)
            {
                return;
            }
            if (SelectedFileObject.IsArchived)
            {
                m_Model.LoadInArchive(SelectedFileObject, Items);
            }
            else
            {
                if (SelectedFileObject is Folder)
                {
                    CurrentPath = SelectedFileObject.Path;
                }
                else if (SelectedFileObject is File)
                {
                    m_Model.Execute(SelectedFileObject);
                }
                else if (SelectedFileObject is Archive)
                {
                    CurrentPath = SelectedFileObject.Path;
                }
                else if (SelectedFileObject is ParentFolder)
                {
                    CurrentPath = SelectedFileObject.Path;
                }
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public void Search(string searchText)
    {
        try
        {
            m_Model.Search(searchText);
        }
        catch(Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public void Update()
    {
        m_Model.LoadPath(Items);
    }

    #endregion

    #region PropertyChangedInterface
    
    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string prop = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    
    #endregion
}