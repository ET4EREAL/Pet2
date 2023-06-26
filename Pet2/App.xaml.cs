using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Pet2
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public  static PETHHEntities db = new PETHHEntities();


public static bool DateRangeIntersects(DateTime targetStart, DateTime targetEnd, DateTime start, DateTime end)
        {
            return (((start <= targetStart) && (end >= targetStart)) ||
                ((start <= targetEnd) && (end >= targetEnd)) ||
                ((start >= targetStart) && (end <= targetEnd)));
        }
    }
}
