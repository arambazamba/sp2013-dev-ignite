using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Runtime.Serialization;
using Microsoft.SharePoint.Phone.Application;
using Microsoft.SharePoint.Client;
using System.Collections.ObjectModel;
using System.Device.Location;

namespace GeoApp
{
    [DataContract]
    public class DesignDisplayItemViewModel : DisplayItemViewModelBase
    {
        /// <summary>
        /// Provides display values for fields of the List, given its name. Also used for data binding to Display form UI
        /// </summary>
        public DesignDisplayItemViewModel()
        {

            //FullName
            this["FullName"] = "Sample Text";

            //HomePhone
            this["HomePhone"] = "Sample Text";

            //Location
            this["Location"] = new GeoCoordinate { Latitude = 47, Longitude = 122 };

            //Title
            this["Title"] = "Sample Text";

            //LastNamePhonetic
            this["LastNamePhonetic"] = "Sample Text";

            //FirstName
            this["FirstName"] = "Sample Text";

            //FirstNamePhonetic
            this["FirstNamePhonetic"] = "Sample Text";

            //Email
            this["Email"] = "Sample Text";

            //Company
            this["Company"] = "Sample Text";

            //CompanyPhonetic
            this["CompanyPhonetic"] = "Sample Text";

            //JobTitle
            this["JobTitle"] = "Sample Text";

            //WorkPhone
            this["WorkPhone"] = "Sample Text";

            //CellPhone
            this["CellPhone"] = "Sample Text";

            //WorkFax
            this["WorkFax"] = "Sample Text";

            //WorkAddress
            this["WorkAddress"] = "This is a multiline text field";

            //WorkCity
            this["WorkCity"] = "Sample Text";

            //WorkState
            this["WorkState"] = "Sample Text";

            //WorkZip
            this["WorkZip"] = "Sample Text";

            //WorkCountry
            this["WorkCountry"] = "Sample Text";

            //WebPage
            this["WebPage"] = new UrlFieldViewModel { Url = "http://www.sampleurl.com", Description = "Sample Description" };

            //Comments
            this["Comments"] = "This is a multiline text field";

            //Modified
            this["Modified"] = "1/21/2012";

            //Created
            this["Created"] = "1/21/2012";

            //Author
            this["Author"] = "John Doe";

            //Editor
            this["Editor"] = "John Doe";

            //_UIVersionString
            this["_UIVersionString"] = "Sample Text";

            //Attachments
            this["Attachments"] = "FileName1.txt;Picture.txt;";

        }


        /// <summary>
        /// Provides display values for fields of the List, given its name. Also used for data binding to Display form UI
        /// </summary>
        /// <param name="fieldName" />
        /// <returns />
        public override object this[string fieldName]
        {
            get
            {
                return base[fieldName];
            }
            set
            {
                base[fieldName] = value;
            }
        }
    }
}

