using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectBase.Data
{
    public static class DataAnnotationsValidationRunner
    {
        public static IEnumerable<ErrorInfo> GetErrors(object instance)
        {
            return from prop in TypeDescriptor.GetProperties(instance).Cast<PropertyDescriptor>()
                   from attribute in prop.Attributes.OfType<ValidationAttribute>()
                   where !attribute.IsValid(prop.GetValue(instance))
                   select new ErrorInfo(prop.Name, attribute.FormatErrorMessage(string.Empty), instance);
        }
    }

    public class ErrorInfo
    {
        public ErrorInfo(string name, string message, object instance)
        {
            Name = name;
            Message = message;
            Instance = instance;
        }

        public string Name { get; set; }

        public string Message { get; set; }

        public object Instance { get; set; }
    }
}
