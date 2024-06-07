using System;
using System.Collections.Generic;
using System.IO;

namespace jerry_manager.Core;

public class SearchParameters
{
    public string SearchName { get; set; }
    public string SearchPath { get; set; }

    public bool IsSearchInArchives { get; set; }

    public DateTime? LeftDate { get; set; } = null;
    public DateTime? RightDate { get; set; } = null;

    public long? FileSize { get; set; } = null;

    public string? NotOlderType { get; set; } = null;
    public int? NotOlderThan { get; set; } = null;

    public string? OlderType { get; set; } = null;
    public int? OlderThan { get; set; } = null;

    public List<FileAttributes>? Attributes { get; set; } = null;
}