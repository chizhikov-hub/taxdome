using System;
using System.Collections.Generic;
using System.IO;

namespace TaxDome.AvaloniaApp.Common;

public static class MimeTypeHelper
{
    private static readonly Dictionary<string, string> MimeTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        // Images
        {".jpg", "image/jpeg"},
        {".jpeg", "image/jpeg"},
        {".png", "image/png"},
        {".gif", "image/gif"},
        {".bmp", "image/bmp"},
        
        // Documents
        {".pdf", "application/pdf"},
        {".doc", "application/msword"},
        {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
        {".xls", "application/vnd.ms-excel"},
        {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
        {".ppt", "application/vnd.ms-powerpoint"},
        {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
        
        // Text Files
        {".txt", "text/plain"},
        {".csv", "text/csv"},
        {".xml", "application/xml"},
        {".json", "application/json"},
        
        // Archives
        {".zip", "application/zip"},
        {".rar", "application/x-rar-compressed"},
        {".7z", "application/x-7z-compressed"}
    };

    public static string GetMimeType(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        return MimeTypes.TryGetValue(extension, out var mime) ? mime : "application/octet-stream";
    }
}
