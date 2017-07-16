using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

using EnvDTE;

using EnvDTE80;

using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.Shell;

using PreventCheckin.Services.Interfaces;
using PreventCheckinVsix.Helpers;

namespace PreventCheckinVsix.Commands
{
    internal sealed class AvoidCheckin
    {
        private readonly IPreventCheckinService iPreventCheckinService;

        private readonly Package package;

        private bool excluded;

        private ProjectItem item;

        private AvoidCheckin(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            this.package = package;

            var commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(PackageGuids.guidPreventCheckinCmdSet, PackageIds.AvoidCheckin);
                var menuItem = new OleMenuCommand(this.MenuItemCallback, menuCommandID);
                menuItem.BeforeQueryStatus += this.MenuItemOnBeforeQueryStatus;
                commandService.AddCommand(menuItem);
            }

            this.iPreventCheckinService = ServiceLocator.Current.GetInstance<IPreventCheckinService>();
        }

        public static AvoidCheckin Instance
        {
            get;
            private set;
        }

        private IServiceProvider ServiceProvider => this.package;

        public static void Initialize(Package package)
        {
            Instance = new AvoidCheckin(package);
        }


        private void MenuItemOnBeforeQueryStatus(object sender, EventArgs eventArgs)
        {
            var button = (OleMenuCommand)sender;
            button.Visible = button.Enabled = false;

            this.item = this.GetProjectItem(PreventCheckinPackage.Dte);

            //var files = this.GetSelectedItemsInSourceControl();
            var files = ProjectHelpers.GetSelectedItemPaths();
            if (!files.Any())
            {
                return;
            }

            button.Visible = button.Enabled = true;

            var fileArray = files.ToArray();
            this.excluded = this.iPreventCheckinService.AreExcluded(fileArray);
            button.Checked = this.excluded;
        }

        private IEnumerable<string> GetSelectedItemsInSourceControl()
        {
            var files = ProjectHelpers.GetSelectedItemPaths();
            files = files.Where(ProjectHelpers.IsFileInSourceControl);
            return files;
        }

        private ProjectItem GetProjectItem(DTE2 dte)
        {
            var window = dte.ActiveWindow;

            if (window == null)
            {
                return null;
            }

            if (window.Type == vsWindowType.vsWindowTypeDocument)
            {
                var doc = dte.ActiveDocument;

                if (doc != null && !string.IsNullOrEmpty(doc.FullName))
                {
                    return dte.Solution.FindProjectItem(doc.FullName);
                }
            }

            return ProjectHelpers.GetSelectedItems().FirstOrDefault();
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            if (this.item == null)
            {
                return;
            }

            var files = this.GetSelectedItemsInSourceControl();

            if (this.excluded)
            {
                this.iPreventCheckinService.UnpreventCheckinStatus(files);
            }
            else
            {
                this.iPreventCheckinService.PreventCheckinStatus(files);
            }
        }
    }
}
