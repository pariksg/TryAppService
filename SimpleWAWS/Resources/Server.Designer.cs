﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SimpleWAWS.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Server {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Server() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SimpleWAWS.Resources.Server", typeof(Server).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Deployment in progress..
        /// </summary>
        internal static string Deployment_DeploymentInProgress {
            get {
                return ResourceManager.GetString("Deployment_DeploymentInProgress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Git deployment in progress..
        /// </summary>
        internal static string Deployment_GitDeploymentInProgress {
            get {
                return ResourceManager.GetString("Deployment_GitDeploymentInProgress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An Error occured. Please try again later..
        /// </summary>
        internal static string Error_GeneralErrorMessage {
            get {
                return ResourceManager.GetString("Error_GeneralErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to retrieve the publishing profile. Please try again later..
        /// </summary>
        internal static string Error_GettingPublishingProfileStream {
            get {
                return ResourceManager.GetString("Error_GettingPublishingProfileStream", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to App Service has to be of type &apos;Mobile&apos; to download mobile clients..
        /// </summary>
        internal static string Error_InvalidAppServiceType {
            get {
                return ResourceManager.GetString("Error_InvalidAppServiceType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Github repo URI is either invalid of from an trusted organization..
        /// </summary>
        internal static string Error_InvalidGithubRepo {
            get {
                return ResourceManager.GetString("Error_InvalidGithubRepo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid user..
        /// </summary>
        internal static string Error_InvalidUserIdentity {
            get {
                return ResourceManager.GetString("Error_InvalidUserIdentity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You can&apos;t have more than 1 free resource at a time..
        /// </summary>
        internal static string Error_MoreThanOneFreeResource {
            get {
                return ResourceManager.GetString("Error_MoreThanOneFreeResource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No free resources are available currently. Please try again later..
        /// </summary>
        internal static string Error_NoFreeResourcesAvailable {
            get {
                return ResourceManager.GetString("Error_NoFreeResourcesAvailable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sorry Organizational accounts are not supported. Please use a Microsoft Account..
        /// </summary>
        internal static string Error_OrgIdNotSupported {
            get {
                return ResourceManager.GetString("Error_OrgIdNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unsupported platform..
        /// </summary>
        internal static string Error_UnsupportedPlatform {
            get {
                return ResourceManager.GetString("Error_UnsupportedPlatform", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This Logic App template will ping a website on a recurring schedule.  You can extend it to take an action depending on the result of the ping..
        /// </summary>
        internal static string Templates_PingSiteDescription {
            get {
                return ResourceManager.GetString("Templates_PingSiteDescription", resourceCulture);
            }
        }
    }
}
