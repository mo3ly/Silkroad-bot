﻿#pragma checksum "..\..\..\..\..\View\GameBot\Potion\PetPotion.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "EE3B269B5821045C1A7DB12B8C02BF379A83E819BC3818C36ADCEDBF812D977A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SRO_INGAME.View.GameBot.Potion;
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


namespace SRO_INGAME.View.GameBot.Potion {
    
    
    /// <summary>
    /// PetPotion
    /// </summary>
    public partial class PetPotion : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 26 "..\..\..\..\..\View\GameBot\Potion\PetPotion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image SlotHgpPet;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\..\View\GameBot\Potion\PetPotion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox petHGPValueBox;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\..\View\GameBot\Potion\PetPotion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox petHGPCheckBox;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\..\View\GameBot\Potion\PetPotion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider petHGPSlider;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\..\View\GameBot\Potion\PetPotion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox sHGPCheckBox;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\..\..\View\GameBot\Potion\PetPotion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid PetRootFrame;
        
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
            System.Uri resourceLocater = new System.Uri("/SRO_INGAME;component/view/gamebot/potion/petpotion.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\View\GameBot\Potion\PetPotion.xaml"
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
            this.SlotHgpPet = ((System.Windows.Controls.Image)(target));
            return;
            case 2:
            this.petHGPValueBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.petHGPCheckBox = ((System.Windows.Controls.CheckBox)(target));
            
            #line 32 "..\..\..\..\..\View\GameBot\Potion\PetPotion.xaml"
            this.petHGPCheckBox.Checked += new System.Windows.RoutedEventHandler(this.SliderChecked);
            
            #line default
            #line hidden
            
            #line 32 "..\..\..\..\..\View\GameBot\Potion\PetPotion.xaml"
            this.petHGPCheckBox.Unchecked += new System.Windows.RoutedEventHandler(this.SliderChecked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.petHGPSlider = ((System.Windows.Controls.Slider)(target));
            
            #line 33 "..\..\..\..\..\View\GameBot\Potion\PetPotion.xaml"
            this.petHGPSlider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.SliderValueChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.sHGPCheckBox = ((System.Windows.Controls.CheckBox)(target));
            
            #line 37 "..\..\..\..\..\View\GameBot\Potion\PetPotion.xaml"
            this.sHGPCheckBox.Checked += new System.Windows.RoutedEventHandler(this.AutoSwitchChecked);
            
            #line default
            #line hidden
            
            #line 37 "..\..\..\..\..\View\GameBot\Potion\PetPotion.xaml"
            this.sHGPCheckBox.Unchecked += new System.Windows.RoutedEventHandler(this.AutoSwitchChecked);
            
            #line default
            #line hidden
            return;
            case 6:
            this.PetRootFrame = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

