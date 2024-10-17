using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RmlEditor.Models
{
    public class Constants
    {
        public static readonly string Template = 
@"<Receipt>
    <DataContext>
        <Dictionary>

            <Pair Key=""TemplateValue"" Value=""123456"" />

        </Dictionary>
    </DataContext>
    <Resources>



    </Resources>
    <Body Width=""500px"" Background=""FFFFFF"" Margin=""5,5,5,5"">



    </Body>
</Receipt>";

        public static readonly string PNG = "image/png";

        public static readonly string JPEG = "image/jpeg";
    }

}
