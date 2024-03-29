using System;
using System.IO;
using System.Windows;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using jerry_manager.Model;
using jerry_manager.Core.FileSystem;
using File = jerry_manager.Core.FileSystem.File;

namespace jerry_manager.ViewModel;

public class FileExplorerViewModel : INotifyPropertyChanged
{
    #region Variables

    private FileExplorerModel m_Model { get; set; }

    public string DriveFreeSpace => SelectedDrive.Size + " kb of " + SelectedDrive.TotalSize + " kb free";

    public ObservableCollection<Drive> Drives => m_Model.Drives;

    public ObservableCollection<FileSystemObject> Items => m_Model.FileObjects;

    private FileSystemWatcher m_FileSystemWatcher;

    private FileSystemObject m_SelectedObject;

    public FileSystemObject SelectedFileObject
    {
        get => m_SelectedObject;
        set
        {
            m_SelectedObject = value;
            OnPropertyChanged("SelectedObject");
        }
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
            try
            {
                m_Model.CurrentPath = value;
                OnPropertyChanged("CurrentPath");
                if (SelectedFileObject is Archive)
                {
                    m_Model.LoadArchive(SelectedFileObject);
                    m_FileSystemWatcher.EnableRaisingEvents = false;
                }
                else if (SelectedFileObject is not null && SelectedFileObject.IsArchived)
                {
                    m_Model.LoadInArchive(SelectedFileObject);
                }
                else if (Directory.Exists(CurrentPath))
                {
                    m_Model.LoadPath();
                    m_FileSystemWatcher.Path = m_Model.CurrentPath;
                    m_FileSystemWatcher.EnableRaisingEvents = true;
                }
                else
                {
                    m_FileSystemWatcher.EnableRaisingEvents = true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }

    #endregion

    #region Constructors

    public FileExplorerViewModel()
    {
        m_Model = new();
        m_FileSystemWatcher = new();

        m_Model.LoadDrives();
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
        try
        {
            m_Model.LoadPath();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    public void DoubleClick()
    {
        try
        {
            if (SelectedFileObject.IsArchived)
            {
                m_Model.LoadInArchive(SelectedFileObject);
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

    public void Update()
    {
        try
        {
            m_Model.LoadPath();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public void OrderListBy(string sortBy, bool sortDirection)
    {
        try
        {
            var firstOne = Items.First();
            Items.RemoveAt(0);
            List<FileSystemObject> orderList;
            switch (sortBy)
            {
                case "Name":
                    orderList = sortDirection
                        ? Items.OrderBy(x => x.Name).ToList()
                        : Items.OrderByDescending(x => x.Name).ToList();
                    break;
                case "Type":
                    orderList = sortDirection
                        ? Items.OrderBy(x => x.Extension).ToList()
                        : Items.OrderByDescending(x => x.Extension).ToList();
                    break;
                case "Size":
                    orderList = sortDirection
                        ? Items.OrderBy(x => x.SizeInBytes).ToList()
                        : Items.OrderByDescending(x => x.SizeInBytes).ToList();
                    break;
                case "Date":
                    orderList = sortDirection
                        ? Items.OrderBy(x => x.DateModified).ToList()
                        : Items.OrderByDescending(x => x.DateModified).ToList();
                    break;
                default:
                    throw new Exception("Sort name error.");
            }

            Items.Clear();
            Items.Add(firstOne);
            foreach (var item in orderList)
            {
                Application.Current.Dispatcher.Invoke(() => Items.Add(item));
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public void CopyDropped(List<FileSystemObject> droppedItems)
    {
        try
        {
            m_Model.CopyObjects(droppedItems);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    #endregion

    #region PropertyChangedInterface

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string prop = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    #endregion
}