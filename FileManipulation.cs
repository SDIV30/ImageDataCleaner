using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Drawing;
using Image =System.Drawing.Image;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Runtime.Serialization;
using Path = System.IO.Path;

namespace ImageDataCleaner
{
    internal class FileManipulation
    {
        internal static List<string> imagePaths= new List<string>();
        internal static string pickedFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        internal List<string> getImages() {
            using (CommonOpenFileDialog fileDialog = new CommonOpenFileDialog()) {           
                fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                fileDialog.Multiselect = true;
                if(fileDialog.ShowDialog()== CommonFileDialogResult.Ok)
                {
                    imagePaths.Clear();
                    imagePaths= fileDialog.FileNames.ToList();
                }

                List<string> fileNames = new List<string>();
                for (int i = 0; i < imagePaths.Count(); i++)
                {
                    fileNames.Add(Path.GetFileName(imagePaths[i]));
                }
                return fileNames;
            }
        }
        internal string getFolder()
        {
            using (CommonOpenFileDialog fileDialog = new CommonOpenFileDialog())
            {
                fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                fileDialog.IsFolderPicker = true;
                if (fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    DirectoryInfo dirInf = new DirectoryInfo(fileDialog.FileName);
                    if(dirInf.Attributes!=FileAttributes.ReadOnly)
                    {
                        pickedFolder = fileDialog.FileName;
                    }
                }
                    return pickedFolder;
            }
        }
        
        internal void cleanMeta(string fileName, string path, string prefix) 
        {
            using (Image img = Image.FromFile(fileName))
            {
                var allPoprItem = img.PropertyItems;
                const int description = 0x10e;
                const int comment = 0x9286;
                const int software = 0x0131;
                
                using (Bitmap imgCopy = new Bitmap(img)) 
                {
                    if (imgCopy.PropertyItems.Any(x => x.Id == description))
                    {
                        imgCopy.RemovePropertyItem(description);
                    }

                    if (imgCopy.PropertyItems.Any(x => x.Id == comment))
                    {
                        imgCopy.RemovePropertyItem(comment);
                    }

                    if (imgCopy.PropertyItems.Any(x => x.Id == software))
                    {
                        imgCopy.RemovePropertyItem(software);
                    }
                    //string directory = Path.GetDirectoryName(path);
                    //string directory = path;
                    string prefixedFileName = $"{prefix}{Path.GetFileName(fileName)}";
                    imgCopy.Save(Path.Combine(path, prefixedFileName), ImageFormat.Jpeg);
                }
            }
        }

        internal void runFiles (string newPath, string prefix)
        {
            if (imagePaths.Count != 0) { 
                List<string> imgs = imagePaths;
                if (newPath == "")
                {
                    newPath = Path.GetDirectoryName(imgs[0]);
                }

                if (prefix == "") 
                { 
                    prefix = "Clr_";
                }

                for (int i = 0; i < imgs.Count(); i++)
                {
                    cleanMeta(imgs[i], newPath, prefix);
                }
                imagePaths.Clear();
                pickedFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            }
        }
    }
}
