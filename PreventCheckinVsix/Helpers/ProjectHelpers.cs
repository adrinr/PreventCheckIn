using System;
using System.Collections.Generic;
using System.IO;

using EnvDTE;

using EnvDTE80;

namespace PreventCheckinVsix.Helpers
{
    public static class ProjectHelpers
    {
        private static readonly DTE2 Dte = PreventCheckinPackage.Dte;

        public static bool IsFileInSourceControl(string file)
        {
            var itemUnderSCC = false;
            if (File.Exists(file) && Dte.Solution.FindProjectItem(file) != null)
            {
                itemUnderSCC = Dte.SourceControl.IsItemUnderSCC(file);
            }

            return itemUnderSCC;
        }

        public static IEnumerable<ProjectItem> GetSelectedItems()
        {
            var items = (Array)Dte.ToolWindows.SolutionExplorer.SelectedItems;

            foreach (UIHierarchyItem selItem in items)
            {
                var item = selItem.Object as ProjectItem;

                if (item != null)
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<string> GetSelectedItemPaths()
        {
            foreach (var item in GetSelectedItems())
            {
                if (item != null && item.Properties != null)
                {
                    yield return item.Properties.Item("FullPath").Value.ToString();
                }
            }
        }
    }
}