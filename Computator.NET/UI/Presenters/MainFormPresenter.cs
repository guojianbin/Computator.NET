﻿#define PREFER_NATIVE_METHODS_OVER_SENDKING_SHORTCUT_KEYS
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Computator.NET.DataTypes;
using Computator.NET.DataTypes.Events;
using Computator.NET.Properties;
using Computator.NET.UI.Interfaces;
using Computator.NET.UI.Models;
using Computator.NET.UI.Views;

namespace Computator.NET.UI.Presenters
{
    public class MainFormPresenter
    {
        private readonly IMainForm _view;
        private readonly ISharedViewState _sharedViewState;

        private CalculationsMode _calculationsMode;
        private bool _applicationNeedRestart;
        private readonly IApplicationManager _applicationManager;
        public MainFormPresenter(IMainForm view, ISharedViewState sharedViewState, IApplicationManager applicationManager)
        {
            _view = view;
            _sharedViewState = sharedViewState;
            _applicationManager = applicationManager;
         

            //  _view.EnterClicked += (o, e) => _sharedViewState.CurrentAction?.Invoke(o, e);


            _view.ModeForcedToReal += (sender, args) =>
            {
                //   SetMode(CalculationsMode.Real);
                EventAggregator.Instance.Publish(new CalculationsModeChangedEvent(CalculationsMode.Real));
            };
            _view.ModeForcedToComplex += (sender, args) =>
            {
                //  SetMode(CalculationsMode.Complex);
                EventAggregator.Instance.Publish(new CalculationsModeChangedEvent(CalculationsMode.Complex));
            };
            _view.ModeForcedToFxy += (sender, args) =>
            {
                //  SetMode(CalculationsMode.Fxy);
                EventAggregator.Instance.Publish(new CalculationsModeChangedEvent(CalculationsMode.Fxy));
            };

            EventAggregator.Instance.Subscribe<CalculationsModeChangedEvent>(mode => SetMode(mode.CalculationsMode));


            Settings.Default.PropertyChanged += (o, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(Settings.Default.Language):
                        _applicationNeedRestart = !Equals(CultureInfo.CurrentCulture, Settings.Default.Language);
                        break;
                }
            };

            Settings.Default.SettingsSaving += (o, e) =>
            {
                if (_applicationNeedRestart)
                {
                    _applicationNeedRestart = false;
                    Task.Factory.StartNew(() => { Thread.Sleep(400); _applicationManager.Restart(); });
                }
            };

            ///////EventAggregator.Instance.Subscribe<ChangeViewEvent>(cv => { _view.SelectedViewIndex = (int) cv.View; });

            _sharedViewState.PropertyChanged += (o, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(_sharedViewState.CurrentView):
                        _view.SelectedViewIndex = (int)_sharedViewState.CurrentView;
                        break;
                }
            };

            _view.SelectedViewChanged += _view_SelectedViewChanged;

            _view.StatusText = GlobalConfig.Version;
        }

        private void _view_SelectedViewChanged(object sender, EventArgs e)
        {
            _sharedViewState.CurrentView = (ViewName)_view.SelectedViewIndex;
        }

        private void SetMode(CalculationsMode mode)
        {
            if (mode == _calculationsMode)
                return;


            switch (mode)
            {
                case CalculationsMode.Complex:
                    _view.ModeText = "Mode[Complex : f(z)]";
                    break;
                case CalculationsMode.Fxy:
                    _view.ModeText = "Mode[3D : f(x,y)]";
                    break;
                case CalculationsMode.Real:
                    _view.ModeText = "Mode[Real : f(x)]";
                    break;
            }

            _calculationsMode = mode;
            //  _view.EditChartMenus.SetMode(_calculationsMode);
        }
    }
}