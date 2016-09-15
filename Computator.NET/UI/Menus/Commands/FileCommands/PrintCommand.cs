using Accord.Collections;
using Computator.NET.Charting;
using Computator.NET.DataTypes;
using Computator.NET.Properties;
using Computator.NET.UI.Controls.CodeEditors;
using Computator.NET.UI.Models;
using Computator.NET.UI.Views;

namespace Computator.NET.UI.Menus.Commands.FileCommands
{
    public class PrintCommand : CommandBase
    {
        private ICanFileEdit _customFunctionsCodeEditor;
        private readonly ISharedViewState _sharedViewState;
        private ICanFileEdit _scriptingCodeEditor;
        private readonly ApplicationManager _applicationManager;
        private readonly ReadOnlyDictionary<CalculationsMode, IChart> _charts; 
        public PrintCommand(ICanFileEdit scriptingCodeEditor, ICanFileEdit customFunctionsCodeEditor, ISharedViewState sharedViewState, ApplicationManager applicationManager, ReadOnlyDictionary<CalculationsMode, IChart> charts)
        {
            Icon = Resources.printToolStripButtonImage;
            Text = MenuStrings.printToolStripButton_Text;
            ToolTip = MenuStrings.printToolStripButton_Text;
            ShortcutKeyString = "Ctrl+P";
            this._scriptingCodeEditor = scriptingCodeEditor;
            this._customFunctionsCodeEditor = customFunctionsCodeEditor;
            _sharedViewState = sharedViewState;
            _applicationManager = applicationManager;
            _charts = charts;
        }


        public override void Execute()
        {
            switch ((int) _sharedViewState.CurrentView)
            {
                case 0:
                    //if (_calculationsMode == CalculationsMode.Real)
                    _charts[_sharedViewState.CalculationsMode].Print();
                    // else
                    //  SendStringAsKey("^P");
                    break;

                case 4:
                    //scriptingCodeEditor();

                    break;

                case 5:
                    //this.customFunctionsCodeEditor
                    break;

                default:
                    _applicationManager.SendStringAsKey("^P"); //this.chart2d.Printing.PrintPreview();
                    break;
            }
        }
    }
}