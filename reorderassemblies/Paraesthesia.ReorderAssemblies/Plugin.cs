using System;
using System.Linq;
using Reflector;
using Reflector.CodeModel;

namespace Paraesthesia.ReorderAssemblies
{
	public class Plugin : IPackage
	{
		private IAssemblyManager _assemblyManager = null;
		private ICommandBarManager _commandBarManager = null;
		private IAssemblyBrowser _assemblyBrowser = null;
		private ICommandBarButton _reorderButton = null;
		private IWindowManager _windowManager = null;

		public void Load(IServiceProvider serviceProvider)
		{
			this._assemblyManager = serviceProvider.GetService<IAssemblyManager>();
			this._assemblyBrowser = serviceProvider.GetService<IAssemblyBrowser>();
			this._commandBarManager = serviceProvider.GetService<ICommandBarManager>();
			this._windowManager = serviceProvider.GetService<IWindowManager>();
			var bar = this._commandBarManager.CommandBars[StandardMenus.File];

			// Add this button above "Exit" and the separator.
			var index = bar.Items.Count - 2;
			this._reorderButton = bar.Items.InsertButton(index, "Reorder Asse&mblies", Properties.Resources.Icon, this.ReorderAssemblies);
		}

		private void ReorderAssemblies(object sender, EventArgs e)
		{
			// Deselect the currently selected item and hide the disassembler
			// because it really messes up the sorting otherwise - you get double
			// entries, etc.
			var activeItem = this._assemblyBrowser.ActiveItem;
			this._assemblyBrowser.ActiveItem = null;
			var window = this._windowManager.Windows["DisassemblerWindow"];
			var disassemblerVisible = window.Visible;
			window.Visible = false;

			// Sort the assemblies by name.
			var assemblies = this._assemblyManager.Assemblies;
			var nameComparer = new NameComparer();
			var sortedAssemblies = assemblies
				.Cast<IAssembly>()
				.OrderBy(ia => ia.Name, nameComparer)
				.Distinct()
				.ToArray();

			// Remove all before adding all because it gets
			// really messed up if you try to do it all at once.
			foreach (var asm in sortedAssemblies)
			{
				assemblies.Remove(asm);
			}
			foreach (var asm in sortedAssemblies)
			{
				assemblies.Add(asm);
			}

			// Put stuff back the way it was. For some reason setting the
			// active item isn't working right now, but... oh well.
			this._assemblyBrowser.ActiveItem = activeItem;
			window.Visible = disassemblerVisible;
		}

		public void Unload()
		{
			this._commandBarManager.CommandBars[StandardMenus.File].Items.Remove(this._reorderButton);
		}
	}
}
