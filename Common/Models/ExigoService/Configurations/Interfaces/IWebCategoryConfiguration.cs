using System.Collections.Generic;

namespace ExigoService
{
    public interface IWebCategoryConfiguration
    {
        List<WebCategory> Categories { get; }

        WebCategory EnrollmentKits { get; }
        WebCategory Accessories { get; }

        WebCategory Health { get; }
        WebCategory Drinks { get; }
        WebCategory Sprays { get; }
    }                                       
}