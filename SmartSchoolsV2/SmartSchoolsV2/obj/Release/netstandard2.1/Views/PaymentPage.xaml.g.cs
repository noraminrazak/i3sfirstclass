//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::Xamarin.Forms.Xaml.XamlResourceIdAttribute("SmartSchoolsV2.Views.PaymentPage.xaml", "Views/PaymentPage.xaml", typeof(global::SmartSchoolsV2.Views.PaymentPage))]

namespace SmartSchoolsV2.Views {
    
    
    [global::Xamarin.Forms.Xaml.XamlFilePathAttribute("Views\\PaymentPage.xaml")]
    public partial class PaymentPage : global::Xamarin.Forms.ContentPage {
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.ProgressBar activity_indicator;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Plugin.HybridWebView.Shared.HybridWebViewControl WebContent;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::SmartSchoolsV2.SnackBar SnackB;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private void InitializeComponent() {
            global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(PaymentPage));
            activity_indicator = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.ProgressBar>(this, "activity_indicator");
            WebContent = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Plugin.HybridWebView.Shared.HybridWebViewControl>(this, "WebContent");
            SnackB = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::SmartSchoolsV2.SnackBar>(this, "SnackB");
        }
    }
}
