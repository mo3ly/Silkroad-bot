﻿#pragma checksum "..\..\..\..\View\Main\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "8268884045EC06F59713833D06F360F79645CA4457F3A7F2D154BE9799EF294C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SRO_INGAME;
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


namespace SRO_INGAME {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\..\View\Main\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label windowTitle;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\View\Main\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Frame mContent;
        
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
            System.Uri resourceLocater = new System.Uri("/SRO_INGAME;component/view/main/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\Main\MainWindow.xaml"
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
            
            #line 8 "..\..\..\..\View\Main\MainWindow.xaml"
            ((SRO_INGAME.MainWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.windowTitle = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            
            #line 20 "..\..\..\..\View\Main\MainWindow.xaml"
            ((System.Windows.Controls.DockPanel)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Dashboard_MouseDown);
            
            #line default
            #line hidden
            
            #line 20 "..\..\..\..\View\Main\MainWindow.xaml"
            ((System.Windows.Controls.DockPanel)(target)).MouseMove += new System.Windows.Input.MouseEventHandler(this.Dashboard_MouseMove);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 25 "..\..\..\..\View\Main\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.exit_Button);
            
            #line default
            #line hidden
            return;
            case 5:
            this.mContent = ((System.Windows.Controls.Frame)(target));
            
            #line 26 "..\..\..\..\View\Main\MainWindow.xaml"
            this.mContent.Navigating += new System.Windows.Navigation.NavigatingCancelEventHandler(this.MainFrame_OnNavigating);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
