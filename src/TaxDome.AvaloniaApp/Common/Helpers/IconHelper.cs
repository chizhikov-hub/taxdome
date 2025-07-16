using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace TaxDome.AvaloniaApp.Common.Helpers;

public static class IconHelper
{
    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct SHFILEINFO
    {
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }

    private const uint SHGFI_ICON = 0x100;
    private const uint SHGFI_USEFILEATTRIBUTES = 0x10;
    private const uint SHGFI_LARGEICON = 0x0;
    private const uint SHGFI_SMALLICON = 0x1;
    private const uint FILE_ATTRIBUTE_NORMAL = 0x80;

    [DllImport("user32.dll")]
    private static extern bool DestroyIcon(IntPtr hIcon);

    private static readonly Dictionary<string, Bitmap> IconCache = new(StringComparer.OrdinalIgnoreCase);

    public static Bitmap GetFileIcon(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        if (IconCache.TryGetValue(extension, out var cachedIcon))
            return cachedIcon;

        var icon = GetIconFromSystem(extension); 
        if (icon != null)
            IconCache[extension] = icon;
            
        return icon;
    }
    
    private static Bitmap GetIconFromSystem(string extension)
    {
        if (OperatingSystem.IsWindows())
        {
            try
            {
                var shinfo = new SHFILEINFO();
            
                var flags = SHGFI_ICON | SHGFI_USEFILEATTRIBUTES | SHGFI_LARGEICON;
                SHGetFileInfo(extension, FILE_ATTRIBUTE_NORMAL, ref shinfo, (uint)Marshal.SizeOf(shinfo), flags);

                if (shinfo.hIcon == IntPtr.Zero)
                    return null;

                using var icon = Icon.FromHandle(shinfo.hIcon);
                using var bitmap = icon.ToBitmap();
                using var memoryStream = new MemoryStream();
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Position = 0;

                DestroyIcon(shinfo.hIcon);

                return new Bitmap(memoryStream);
            }
            catch
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
}