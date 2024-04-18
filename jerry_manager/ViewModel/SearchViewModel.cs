using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using jerry_manager.Core;
using jerry_manager.Core.FileSystem;
using jerry_manager.Model;
using System.IO;

namespace jerry_manager.ViewModel;

public class SearchViewModel
{
    #region Variables

    private SearchModel m_Model;
    public ObservableCollection<FileSystemObject> Items => m_Model.FileObjects;
    
    private FileSystemObject m_SelectedFileObject;

    public FileSystemObject SelectedFileObject
    {
        get => m_SelectedFileObject;
        set
        {
            m_SelectedFileObject = value;
            OnPropertyChanged("SelectedFileObject");
        }
    }
    
    public bool Disable => !m_SearchInArchives;


    private string m_SearchName;

    public string SearchName
    {
        get => m_SearchName;
        set
        {
            m_SearchName = value;
            OnPropertyChanged("SearchName");
        }
    }

    private string m_SearchPath;

    public string SearchPath
    {
        get => m_SearchPath;
        set
        {
            m_SearchPath = value;
            OnPropertyChanged("SearchPath");
        }
    }

    private bool m_SearchInArchives;

    public bool SearchInArchives
    {
        get => m_SearchInArchives;
        set
        {
            m_SearchInArchives = value;
            EnabledInput = !m_SearchInArchives;
            OnPropertyChanged("SearchInArchives");
        }
    }

    private bool m_EnabledInput;

    public bool EnabledInput
    {
        get => !m_SearchInArchives;
        set
        {
            m_EnabledInput = value; 
        }
    }

    private bool m_NotOlderEnabled;

    public bool NotOlderEnabled
    {
        get => m_NotOlderEnabled;
        set
        {
            m_NotOlderEnabled = value;
            OnPropertyChanged("NotOlderEnabled");
        }
    }

    private bool m_OlderEnabled;

    public bool OlderEnabled
    {
        get => m_OlderEnabled;
        set
        {
            m_OlderEnabled = value;
            OnPropertyChanged("OlderEnabled");
        }
    }

    private bool m_FileSizeEnabled;

    public bool FileSizeEnabled
    {
        get => m_FileSizeEnabled;
        set
        {
            m_FileSizeEnabled = value;
            OnPropertyChanged("FileSizeEnabled");
        }
    }

    private bool m_AttributesEnabled;

    public bool AttributesEnabled
    {
        get => m_AttributesEnabled;
        set
        {
            m_AttributesEnabled = value;
            OnPropertyChanged("AttributesEnabled");
        }
    }

    private bool m_DateBetweenEnabled;

    public bool DateBetweenEnabled
    {
        get => m_DateBetweenEnabled;
        set
        {
            m_DateBetweenEnabled = value;
            OnPropertyChanged("DateBetweenEnabled");
        }
    }

    private DateTime m_LeftDate;

    public DateTime LeftDate
    {
        get => m_LeftDate;
        set
        {
            m_LeftDate = value;
            OnPropertyChanged("LeftDate");
        }
    }

    private DateTime m_RightDate;

    public DateTime RightDate
    {
        get => m_RightDate;
        set
        {
            m_RightDate = value;
            OnPropertyChanged("RightDate");
        }
    }

    public List<string> DateValTypes { get; set; }

    private string m_NotOlderSelected;

    public string NotOlderSelected
    {
        get => m_NotOlderSelected;
        set
        {
            m_NotOlderSelected = value;
            OnPropertyChanged("NotOlderSelected");
        }
    }

    private string m_OlderSelected;

    public string OlderSelected
    {
        get => m_OlderSelected;
        set
        {
            m_OlderSelected = value;
            OnPropertyChanged("OlderSelected");
        }
    }

    private int m_NotOlderInteger;

    public int NotOlderInteger
    {
        get => m_NotOlderInteger;
        set
        {
            m_NotOlderInteger = value;
            OnPropertyChanged("NotOlderInteger");
        }
    }

    private int m_OlderInteger;

    public int OlderInteger
    {
        get => m_OlderInteger;
        set
        {
            m_OlderInteger = value;
            OnPropertyChanged("OlderInteger");
        }
    }

    private int m_FileSizeInteger;

    public int FileSizeInteger
    {
        get => m_FileSizeInteger;
        set
        {
            m_FileSizeInteger = value;
            OnPropertyChanged("FileSizeInteger");
        }
    }

    public List<string> CompareTypes { get; set; }

    private string m_CompareTypeSelected;

    public string CompareTypeSelected
    {
        get => m_CompareTypeSelected;
        set
        {
            m_CompareTypeSelected = value;
            OnPropertyChanged("CompareTypeSelected");
        }
    }

    public List<string> MemoryTypes { get; set; }

    private string m_MemoryTypeSelected;

    public string MemoryTypeSelected
    {
        get => m_MemoryTypeSelected;
        set
        {
            m_MemoryTypeSelected = value;
            OnPropertyChanged("MemoryTypeSelected");
        }
    }

    private bool m_IsArchive;

    public bool IsArchive
    {
        get => m_IsArchive;
        set
        {
            m_IsArchive = value;
            OnPropertyChanged("IsArchive");
        }
    }

    private bool m_IsReadOnly;

    public bool IsReadOnly
    {
        get => m_IsReadOnly;
        set
        {
            m_IsReadOnly = value;
            OnPropertyChanged("IsReadOnly");
        }
    }

    private bool m_IsHidden;

    public bool IsHidden
    {
        get => m_IsHidden;
        set
        {
            m_IsHidden = value;
            OnPropertyChanged("IsHidden");
        }
    }

    private bool m_IsSystem;

    public bool IsSystem
    {
        get => m_IsSystem;
        set
        {
            m_IsSystem = value;
            OnPropertyChanged("IsSystem");
        }
    }

    private bool m_IsDirectory;

    public bool IsDirectory
    {
        get => m_IsDirectory;
        set
        {
            m_IsDirectory = value;
            OnPropertyChanged("IsDirectory");
        }
    }

    private bool m_IsCompressed;

    public bool IsCompressed
    {
        get => m_IsCompressed;
        set
        {
            m_IsCompressed = value;
            OnPropertyChanged("IsCompressed");
        }
    }

    private bool m_IsEncrypted;

    public bool IsEncrypted
    {
        get => m_IsEncrypted;
        set
        {
            m_IsEncrypted = value;
            OnPropertyChanged("IsEncrypted");
        }
    }

    #endregion

    #region Constructor

    public SearchViewModel()
    {
        m_Model = new();

        m_SearchName = string.Empty;
        m_EnabledInput = true;
        m_SearchInArchives = false;
        m_SearchPath = DataCache.ActiveView.CurrentPath;

        m_DateBetweenEnabled = false;
        m_NotOlderEnabled = false;
        m_OlderEnabled = false;
        m_FileSizeEnabled = false;
        m_AttributesEnabled = false;

        m_LeftDate = DateTime.Now;
        m_RightDate = DateTime.Now;

        DateValTypes = new()
        {
            "hours",
            "days",
            "years"
        };

        CompareTypes = new()
        {
            ">",
            "=",
            "<"
        };

        MemoryTypes = new()
        {
            "KB",
            "MB",
            "GB"
        };

        m_OlderInteger = 1;
        m_NotOlderInteger = 1;
        m_FileSizeInteger = 1;

        m_NotOlderSelected = DateValTypes[1];
        m_OlderSelected = DateValTypes[1];

        m_CompareTypeSelected = CompareTypes[1];
        m_MemoryTypeSelected = MemoryTypes[1];

        m_IsArchive = false;
        m_IsReadOnly = false;
        m_IsHidden = false;
        m_IsSystem = false;
        m_IsCompressed = false;
        m_IsEncrypted = false;
        m_IsDirectory = false;
    }

    #endregion

    #region Methods

    public void StartSearch()
    {
        try
        {
            Items.Clear();
            List<FileAttributes> attributes = new();
            if (IsArchive)
            {
                attributes.Add(FileAttributes.Archive);
            }

            if (IsReadOnly)
            {
                attributes.Add(FileAttributes.ReadOnly);
            }

            if (IsHidden)
            {
                attributes.Add(FileAttributes.Hidden);
            }

            if (IsSystem)
            {
                attributes.Add(FileAttributes.System);
            }

            if (IsCompressed)
            {
                attributes.Add(FileAttributes.Compressed);
            }

            if (IsDirectory)
            {
                attributes.Add(FileAttributes.Directory);
            }
            
            if (IsEncrypted)
            {
                attributes.Add(FileAttributes.Encrypted);
            }

            var parameters = new SearchParameters()
            {
                SearchName = m_SearchName,
                SearchPath = m_SearchPath,
                IsSearchInArchives = SearchInArchives,
                NotOlderType = NotOlderEnabled ? m_NotOlderSelected : null,
                NotOlderThan = NotOlderEnabled ? m_NotOlderInteger : null,
                OlderType = OlderEnabled ? m_OlderSelected : null,
                OlderThan = OlderEnabled ? m_OlderInteger : null,
                FileSize = FileSizeEnabled ? m_FileSizeInteger : null,
                Attributes = AttributesEnabled ? attributes : null,
                LeftDate = DateBetweenEnabled ? m_LeftDate : null,
                RightDate = DateBetweenEnabled ? m_RightDate : null
            };
            m_Model.SearchInFileSystem(parameters);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public void DoubleClick()
    {
        try
        {
            if (SelectedFileObject is Folder && !SelectedFileObject.IsArchived)
            {
                DataCache.ActiveView.CurrentPath = SelectedFileObject.Path;
            } 
            else if (SelectedFileObject is Folder && SelectedFileObject.IsArchived)
            {
                DataCache.ActiveView.SelectedFileObject = SelectedFileObject;
                DataCache.ActiveView.CurrentPath = SelectedFileObject.Path;
            }
            else
            {
                if (SelectedFileObject is Folder && SelectedFileObject.IsArchived)
                {
                    DataCache.ActiveView.SelectedFileObject = SelectedFileObject;
                }

                string pathTo;
                if (SelectedFileObject.Path.EndsWith(SelectedFileObject.Name) && !SelectedFileObject.IsArchived)
                {
                    pathTo = SelectedFileObject.Path.Substring(0,
                        SelectedFileObject.Path.Length - SelectedFileObject.Name.Length);
                }
                else if (SelectedFileObject.IsArchived)
                {
                    pathTo = SelectedFileObject.Path;
                }
                else
                {
                    throw new Exception("Unexpected error.");
                }

                DataCache.ActiveView.CurrentPath = pathTo;
            }
            
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