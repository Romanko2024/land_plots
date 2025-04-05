using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandManagementApp.Models
{
    public enum PurposeType
    {
        [Description("Під забудову")]
        Construction,
        [Description("Сільськогосподарського призначення")]
        Agricultural,
        [Description("Зарезервована")]
        Reserved
    }
}
