﻿#pragma checksum "..\..\..\..\View\Stalls\NearbyStalls.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "6B3747C69FE67112889AD64ED9106CD316C432824127020716210269A4F15368"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SRO_INGAME.View.Stalls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace SRO_INGAME.View.Stalls {
    
    
    /// <summary>
    /// NearbyStalls
    /// </summary>
    public partial class NearbyStalls : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 12 "..\..\..\..\View\Stalls\NearbyStalls.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelStallsCount;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\View\Stalls\NearbyStalls.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button PrevPageButton;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\View\Stalls\NearbyStalls.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.ImageBrush PrevPageButtonBG;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\View\Stalls\NearbyStalls.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox PaginationNumberLabel;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\View\Stalls\NearbyStalls.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button NextPageButton;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\..\View\Stalls\NearbyStalls.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.ImageBrush NextPageButtonBG;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\View\Stalls\NearbyStalls.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid NearbyStallsGrid;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\..\View\Stalls\NearbyStalls.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel Overlay;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\..\View\Stalls\NearbyStalls.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border EmptyStallLabel;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\..\View\Stalls\NearbyStalls.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Frame stallFrame;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SRO_INGAME;component/view/stalls/nearbystalls.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\Stalls\NearbyStalls.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\..\..\View\Stalls\NearbyStalls.xaml"
            ((SRO_INGAME.View.Stalls.NearbyStalls)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.labelStallsCount = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.PrevPageButton = ((System.Windows.Controls.Button)(target));
            return;
            case 4:
            this.PrevPageButtonBG = ((System.Windows.Media.ImageBrush)(target));
            return;
            case 5:
            this.PaginationNumberLabel = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.NextPageButton = ((System.Windows.Controls.Button)(target));
            return;
            case 7:
            this.NextPageButtonBG = ((System.Windows.Media.ImageBrush)(target));
            return;
            case 8:
            this.NearbyStallsGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 10:
            this.Overlay = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 11:
            this.EmptyStallLabel = ((System.Windows.Controls.Border)(target));
            return;
            case 12:
            this.stallFrame = ((System.Windows.Controls.Frame)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 9:
            
            #line 48 "..\..\..\..\View\Stalls\NearbyStalls.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.StallDetails_OnClick);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

