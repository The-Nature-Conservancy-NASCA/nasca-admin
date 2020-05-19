using MessageBox = ArcGIS.Desktop.Framework.Dialogs.MessageBox;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProAppModule1
{

    public class FieldValidator
    {

        public async Task<bool> ValidateFields(string class_name, ArcGIS.Core.Data.FeatureClassDefinition fc_def)
        {
            Type type = Type.GetType(class_name);
            var item = Activator.CreateInstance(type);

            PropertyInfo[] properties = item.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var name = property.Name;
                int index = await QueuedTask.Run(()=> {return fc_def.FindField(name); }); 
                if (index < 0)
                {
                    //MessageBox.Show($"El campo {name} no fue encontrado en el elemento seleccionado.", "Validación de campos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            return true;
        }
    }

    public class FeatureClassFieldValidator: FieldValidator
    {


    }

}
