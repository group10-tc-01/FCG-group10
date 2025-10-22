using System;

namespace FCG.Messages
{

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ResourceMessages
    {

        private static global::System.Resources.ResourceManager resourceMan;

        private static global::System.Globalization.CultureInfo resourceCulture;

        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResourceMessages()
        {
        }

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FCG.Messages.ResourceMessages", typeof(ResourceMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }
        public static string GameCategoryIsRequired
        {
            get
            {
                return ResourceManager.GetString("GameCategoryIsRequired", resourceCulture);
            }
        }

        public static string GameCategoryMaxLength
        {
            get
            {
                return ResourceManager.GetString("GameCategoryMaxLength", resourceCulture);
            }
        }

        public static string GameDescriptionIsRequired
        {
            get
            {
                return ResourceManager.GetString("GameDescriptionIsRequired", resourceCulture);
            }
        }

        public static string GameDescriptionMaxLength
        {
            get
            {
                return ResourceManager.GetString("GameDescriptionMaxLength", resourceCulture);
            }
        }

        public static string GameNameAlreadyExists
        {
            get
            {
                return ResourceManager.GetString("GameNameAlreadyExists", resourceCulture);
            }
        }

        public static string GameNameIsRequired
        {
            get
            {
                return ResourceManager.GetString("GameNameIsRequired", resourceCulture);
            }
        }

        public static string GameNameMaxLength
        {
            get
            {
                return ResourceManager.GetString("GameNameMaxLength", resourceCulture);
            }
        }

        public static string GamePriceMustBeGreaterThanZero
        {
            get
            {
                return ResourceManager.GetString("GamePriceMustBeGreaterThanZero", resourceCulture);
            }
        }
        public static string InvalidAccessLevel
        {
            get
            {
                return ResourceManager.GetString("InvalidAccessLevel", resourceCulture);
            }
        }
        public static string InvalidEmailOrPassword
        {
            get
            {
                return ResourceManager.GetString("InvalidEmailOrPassword", resourceCulture);
            }
        }

        public static string InvalidRefreshToken
        {
            get
            {
                return ResourceManager.GetString("InvalidRefreshToken", resourceCulture);
            }
        }
        public static string InvalidToken
        {
            get
            {
                return ResourceManager.GetString("InvalidToken", resourceCulture);
            }
        }

        public static string InvalidUserRole
        {
            get
            {
                return ResourceManager.GetString("InvalidUserRole", resourceCulture);
            }
        }

        public static string LoginEmalRequired
        {
            get
            {
                return ResourceManager.GetString("LoginEmalRequired", resourceCulture);
            }
        }

        public static string LoginInvalidEmailFormat
        {
            get
            {
                return ResourceManager.GetString("LoginInvalidEmailFormat", resourceCulture);
            }
        }

        public static string LoginPasswordRequired
        {
            get
            {
                return ResourceManager.GetString("LoginPasswordRequired", resourceCulture);
            }
        }

        public static string LogoutSuccessFull
        {
            get
            {
                return ResourceManager.GetString("LogoutSuccessFull", resourceCulture);
            }
        }

        public static string LongPassword
        {
            get
            {
                return ResourceManager.GetString("LongPassword", resourceCulture);
            }
        }

        public static string NameIsLong
        {
            get
            {
                return ResourceManager.GetString("NameIsLong", resourceCulture);
            }
        }

        public static string NameRequired
        {
            get
            {
                return ResourceManager.GetString("NameRequired", resourceCulture);
            }
        }

        public static string Password
        {
            get
            {
                return ResourceManager.GetString("Password", resourceCulture);
            }
        }

        public static string RefreshTokenDefaultReason
        {
            get
            {
                return ResourceManager.GetString("RefreshTokenDefaultReason", resourceCulture);
            }
        }

        public static string UserIdRequired
        {
            get
            {
                return ResourceManager.GetString("UserIdRequired", resourceCulture);
            }
        }
    }
}
