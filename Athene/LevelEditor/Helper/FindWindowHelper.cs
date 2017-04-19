using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LevelEditor.Helper
{
    /// <summary>
    /// Helpter to find a window
    /// </summary>
    class FindWindowHelper
    {
        /// <summary>
        /// Finds a window from all windows
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">window name</param>
        /// <returns></returns>
        public static T IsWindowOpen<T>(string name = null) where T : Window
        {
            var windows = Application.Current.Windows.OfType<T>();
            return string.IsNullOrEmpty(name) ? windows.FirstOrDefault() : windows.FirstOrDefault(w => w.Name.Equals(name));
        }
    }


}
