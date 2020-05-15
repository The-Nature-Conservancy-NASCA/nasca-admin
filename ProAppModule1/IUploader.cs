using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Core;

namespace ProAppModule1
{
    public interface IUploader
    {
        void UploadData(Item item, string class_name, string service);
    }
}
