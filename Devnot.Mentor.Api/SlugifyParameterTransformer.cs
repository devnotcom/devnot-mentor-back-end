using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        //Controller isimlerini, Route attribute icindeki(camel-case) isimleri "-" ile ayırır, örnek [ regularExpression => regular-expression ]
        public string TransformOutbound(object value)
        {
            if (value == null) { return null; }

            return Regex.Replace(value.ToString()!,
                "([a-z])([A-Z])",
                "$1-$2",
                RegexOptions.CultureInvariant,
                TimeSpan.FromMilliseconds(100)).ToLowerInvariant();
        }
    }
