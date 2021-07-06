using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        //transforming controller name into a separated form in router, Route("customerRelations")=>customer-relations
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