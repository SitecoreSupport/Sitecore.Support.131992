using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Support.ContentSearch.SolrProvider.Pipelines.FormatQueryFieldValue
{
  using Sitecore.ContentSearch.Pipelines.FormatQueryFieldValue;
  using Sitecore.ContentSearch.SolrProvider.Pipelines.FormatQueryFieldValue;
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;

  public class ApplyFieldMappingRule : Sitecore.ContentSearch.SolrProvider.Pipelines.FormatQueryFieldValue.ApplyFieldMappingRule
  {
    protected override void FormatArgs(string schemaType, FormatQueryFieldValueArgs args)
    {
      if (!string.IsNullOrEmpty(schemaType))
      {
        bool flag = !string.IsNullOrEmpty(args.EscapeCharacter) && args.IncludeExistingCharacter.HasValue;
        switch (schemaType.ToLower())
        {
          case "text":
          case "text_general":
            {
              string str2 = " ";
              Collection<string> additionalEscapeCharacters = new Collection<string> { "-" };
              args.FieldValue = this.Escape(args.FieldValue, !string.IsNullOrEmpty(args.EscapeCharacter) ? args.EscapeCharacter : str2, flag ? args.IncludeExistingCharacter.Value : false, args.ExcludeEscapeCharacters, additionalEscapeCharacters);
              break;
            }
          case "string":
          case "lowercase":
            args.FieldValue = this.Escape(args.FieldValue, !string.IsNullOrEmpty(args.EscapeCharacter) ? args.EscapeCharacter : @"\", flag ? args.IncludeExistingCharacter.Value : true, args.ExcludeEscapeCharacters, null);
            args.IsQuoted = false;
            break;
        }
        args.FieldValue = args.FieldValue.Trim();
      }
    }

    protected override HashSet<string> GetEscapeCharacterSet() =>
        new HashSet<string> {
              "+",
              "&",
              "|",
              "!",
              "{",
              "}",
              "[",
              "]",
              "^",
              "(",
              ")",
              "~",
              ":",
              ";",
              "/",
              @"\",
              "?",
              "\"",
              " "
        };
  }
}