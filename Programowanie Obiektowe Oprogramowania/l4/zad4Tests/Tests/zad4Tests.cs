using System;
using System.IO;
using zad4.TagBuilder;
using Xunit;

namespace zad4Tests.Tests
{
    public class zad4Tests
    {
        [Fact]
        public void ShouldGenerateBasicTagWithoutIndentation()
        {
            var tag = new TagBuilder()
                .StartTag("div")
                .AddContent("Hello")
                .EndTag();

            var expected = "<div>Hello</div>";
            Assert.Equal(expected, tag.ToString().Trim());
        }

        [Fact]
        public void ShouldGenerateIndentedTags()
        {
            var tag = new TagBuilder()
            {
                IsIndented = true,
                Indentation = 2
            }
            .StartTag("div")
                .StartTag("span")
                    .AddContent("Text")
                .EndTag()
            .EndTag();

            var expected =
@"<div>
  <span>
    Text
  </span>
</div>";

            Assert.Equal(expected, tag.ToString().Trim());
        }

        [Fact]
        public void ShouldHandleAttributesCorrectly()
        {
            var tag = new TagBuilder()
                .StartTag("input")
                .AddAttribute("type", "text")
                .AddAttribute("value", "Hello")
                .EndTag();

            var expected = "<input type='text' value='Hello'/>";
            Assert.Equal(expected, tag.ToString().Trim());
        }

        [Fact]
        public void ShouldGenerateComplexIndentedStructure()
        {
            var tag = new TagBuilder()
            {
                IsIndented = true,
                Indentation = 4
            }
            .StartTag("parent")
                .AddAttribute("parentproperty1", "true")
                .AddAttribute("parentproperty2", "5")
                .StartTag("child1")
                    .AddAttribute("childproperty1", "c")
                    .AddContent("childbody")
                .EndTag()
                .StartTag("child2")
                    .AddAttribute("childproperty2", "c")
                    .AddContent("childbody")
                .EndTag()
            .EndTag()
            .StartTag("script")
                .AddContent("$.scriptbody();")
            .EndTag();

            var expected =
@"<parent parentproperty1='true' parentproperty2='5'>
    <child1 childproperty1='c'>
        childbody
    </child1>
    <child2 childproperty2='c'>
        childbody
    </child2>
</parent>
<script>
    $.scriptbody();
</script>";

            Assert.Equal(expected, tag.ToString().Trim());
        }
    }
}
