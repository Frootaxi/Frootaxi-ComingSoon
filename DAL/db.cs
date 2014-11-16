using System.Globalization;

namespace DAL
{
    partial class package
    {
        public string name_capitalized
        {
            get
            {
                string text = this.name;

                if (text != null && text != "")
                {
                    text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
                }
                return text;
            }
        }
    }

    partial class payment_account_type
    {
        public string type_capitalized
        {
            get
            {
                string text = this.type;

                if (text != null && text != "")
                {
                    text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
                }
                return text;
            }
        }
    }

    partial class region
    {
        public string name_capitalized
        {
            get
            {
                string text = this.name;

                if (text != null && text != "")
                {
                    text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
                }
                return text;
            }
        }
    }
}
