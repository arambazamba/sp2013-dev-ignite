﻿#pragma checksum "c:\devprojects\JobsApp\JobsApp\Views\DisplayForm.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "AEF380F590192744D991A1E1A6F7CFC5"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace JobsApp {
    
    
    public partial class DisplayForm : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Controls.PhoneApplicationPage ViewPage;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ProgressBar progressBar;
        
        internal System.Windows.Controls.TextBlock txtTitle;
        
        internal System.Windows.Controls.TextBlock txtDescription;
        
        internal System.Windows.Controls.TextBlock txtAssignedTo;
        
        internal System.Windows.Controls.TextBlock txtModified;
        
        internal System.Windows.Controls.TextBlock txtCreated;
        
        internal System.Windows.Controls.TextBlock txtAuthor;
        
        internal System.Windows.Controls.TextBlock txtEditor;
        
        internal System.Windows.Controls.TextBlock txt_UIVersionString;
        
        internal System.Windows.Controls.TextBlock txtAttachments;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton btnEdit;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton btnDelete;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton btnBack;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/JobsApp;component/Views/DisplayForm.xaml", System.UriKind.Relative));
            this.ViewPage = ((Microsoft.Phone.Controls.PhoneApplicationPage)(this.FindName("ViewPage")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.progressBar = ((System.Windows.Controls.ProgressBar)(this.FindName("progressBar")));
            this.txtTitle = ((System.Windows.Controls.TextBlock)(this.FindName("txtTitle")));
            this.txtDescription = ((System.Windows.Controls.TextBlock)(this.FindName("txtDescription")));
            this.txtAssignedTo = ((System.Windows.Controls.TextBlock)(this.FindName("txtAssignedTo")));
            this.txtModified = ((System.Windows.Controls.TextBlock)(this.FindName("txtModified")));
            this.txtCreated = ((System.Windows.Controls.TextBlock)(this.FindName("txtCreated")));
            this.txtAuthor = ((System.Windows.Controls.TextBlock)(this.FindName("txtAuthor")));
            this.txtEditor = ((System.Windows.Controls.TextBlock)(this.FindName("txtEditor")));
            this.txt_UIVersionString = ((System.Windows.Controls.TextBlock)(this.FindName("txt_UIVersionString")));
            this.txtAttachments = ((System.Windows.Controls.TextBlock)(this.FindName("txtAttachments")));
            this.btnEdit = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("btnEdit")));
            this.btnDelete = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("btnDelete")));
            this.btnBack = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("btnBack")));
        }
    }
}

