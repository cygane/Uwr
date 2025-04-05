using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace zad4.TagBuilder
{
    public class TagBuilder
    {
        private readonly StringWriter _writer;
        private string tagName;
        private TagBuilder parent;
        private StringBuilder body = new StringBuilder();
        private Dictionary<string, string> _attributes = new Dictionary<string, string>();

        // obsluga wciec
        public bool IsIndented { get; set; } = false;
        public int Indentation { get; set; } = 4;
        private int _indentLevel = 0;

        public TagBuilder() { }

        public TagBuilder(string TagName, TagBuilder Parent)
        {
            this.tagName = TagName;
            this.parent = Parent;
            this.IsIndented = Parent.IsIndented;
            this.Indentation = Parent.Indentation;
            this._indentLevel = Parent._indentLevel + 1;
        }

        public TagBuilder AddContent(string Content)
        {
            // body.Append(Content);
            // return this;
            if (IsIndented)
                body.Append(new string(' ', Indentation));

            body.Append(Content);
            return this;
        }

        public TagBuilder AddContentFormat(string Format, params object[] args)
        {
            body.AppendFormat(Format, args);
            return this;
        }

        public TagBuilder StartTag(string TagName)
        {
            //zle cos
            return new TagBuilder(TagName, this);
        }

        public TagBuilder EndTag()
        {
            parent.AddContent(this.ToString());
            return parent;
        }

        public TagBuilder AddAttribute(string Name, string Value)
        {
            _attributes[Name] = Value;
            return this;
        }

        public override string ToString()
        {
            StringBuilder tag = new StringBuilder();

            if (IsIndented)
                tag.Append(new string(' ', Indentation));

            if (!string.IsNullOrEmpty(tagName))
                tag.Append($"<{tagName}");

            if (_attributes.Count > 0)
            {
                tag.Append(" ");
                tag.Append(string.Join(" ", _attributes.Select(kvp => $"{kvp.Key}='{kvp.Value}'")));
            }

            if (body.Length > 0)
            {
                if ( !string.IsNullOrEmpty( this.tagName) || this._attributes.Count > 0 )
                {
                    tag.Append( ">" );
                }
                if (IsIndented) tag.AppendLine();
                tag.Append(body.ToString());
                if (IsIndented) tag.AppendLine();
                if (!string.IsNullOrEmpty(tagName))
                {
                    if (IsIndented)
                        tag.Append(new string(' ', Indentation));
                    tag.Append($"</{tagName}>");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(tagName))
                    tag.Append("/>");
            }

            if (IsIndented) tag.AppendLine();
            return tag.ToString();
        }
    }
}
