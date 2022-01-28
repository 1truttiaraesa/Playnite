﻿using Playnite.Common;
using Playnite.DesktopApp.ViewModels;
using Playnite.SDK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Playnite.DesktopApp.Controls.Views
{
    [TemplatePart(Name = "PART_ListGames", Type = typeof(GamesGridView))]
    public class LibraryListView : Control
    {
        internal readonly DesktopAppViewModel mainModel;
        internal GamesGridView ListGames;

        static LibraryListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LibraryListView), new FrameworkPropertyMetadata(typeof(LibraryListView)));
        }

        public LibraryListView() : this(DesktopApplication.Current?.MainModel)
        {
        }

        public LibraryListView(DesktopAppViewModel mainModel)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.mainModel = DesignMainViewModel.DesignIntance;
            }
            else if (mainModel != null)
            {
                this.mainModel = mainModel;
            }

            this.mainModel.AppSettings.ViewSettings.PropertyChanged += ViewSettings_PropertyChanged;
        }

        private void ViewSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewSettings.GamesViewType))
            {
                if (ListGames != null)
                {
                    SetListGamesBinding();
                }
            }
        }

        private void SetListGamesBinding()
        {
            if (mainModel.AppSettings.ViewSettings.GamesViewType == DesktopView.List)
            {
                BindingTools.SetBinding(ListGames,
                    GamesGridView.ItemsSourceProperty,
                    mainModel,
                    $"{nameof(mainModel.GamesView)}.{nameof(DesktopCollectionView.CollectionView)}");
            }
            else
            {
                ListGames.ItemsSource = null;
            }
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ListGames = Template.FindName("PART_ListGames", this) as GamesGridView;
            if (ListGames != null)
            {
                SetListGamesBinding();
                ListGames.AppSettings = mainModel.AppSettings;

                BindingTools.SetBinding(ListGames,
                    GamesGridView.SelectedItemProperty,
                    mainModel,
                    nameof(DesktopAppViewModel.SelectedGame),
                    BindingMode.TwoWay);
                BindingTools.SetBinding(ListGames,
                    GamesGridView.SelectedItemsListProperty,
                    mainModel,
                    nameof(DesktopAppViewModel.SelectedGamesBinder),
                    BindingMode.TwoWay);
            }

            ControlTemplateTools.InitializePluginControls(
                mainModel.Extensions,
                Template,
                this,
                SDK.ApplicationMode.Desktop,
                mainModel,
                $"{nameof(DesktopAppViewModel.SelectedGameDetails)}.{nameof(GameDetailsViewModel.Game)}.{nameof(GameDetailsViewModel.Game.Game)}");
        }
    }
}
