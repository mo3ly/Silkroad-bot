﻿#pragma checksum "..\..\..\..\..\View\GameBot\Potion\Potion.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "764342AB92654BF0B289F90A30FD228E3A0B31AE0FA13FD899B01D40B909992A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace SRO_INGAME.View {
    
    
    /// <summary>
    /// Potion
    /// </summary>
    public partial class Potion : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 13 "..\..\..\..\..\View\GameBot\Potion\Potion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border pCharacter;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\..\View\GameBot\Potion\Potion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label pCharacterLabel;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\..\View\GameBot\Potion\Potion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border pPet;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\..\View\GameBot\Potion\Potion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label pPetLabel;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\..\..\View\GameBot\Potion\Potion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Frame potionFrame;
        
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
            System.Uri resourceLocater = new System.Uri("/SRO_INGAME;component/view/gamebot/potion/potion.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\View\GameBot\Potion\Potion.xaml"
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
            this.pCharacter = ((System.Windows.Controls.Border)(target));
            
            #line 13 "..\..\..\..\..\View\GameBot\Potion\Potion.xaml"
            this.pCharacter.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Navigation);
            
            #line default
            #line hidden
            return;
            case 2:
            this.pCharacterLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.pPet = ((System.Windows.Controls.Border)(target));
            
            #line 18 "..\..\..\..\..\View\GameBot\Potion\Potion.xaml"
            this.pPet.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Navigation);
            
            #line default
            #line hidden
            return;
            case 4:
            this.pPetLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.potionFrame = ((System.Windows.Controls.Frame)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

