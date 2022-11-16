/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Linq;
using Xunit;
using magic.node.extensions;

namespace magic.lambda.xml.tests
{
    public class XmlTests
    {
        [Fact]
        public void FromXml()
        {
            var result = Common.Evaluate(@"
xml2lambda:@""<html>
  <head>
    <title>Foo</title>
  </head>
  <body>
    <p class=""""howdy"""">
  </body>
</html>""");
            Assert.Equal("Foo", new Expression("**/xml2lambda/*/*/*/title/*/\\#text").Evaluate(result).First().Value);
            Assert.Equal("howdy", new Expression("**/xml2lambda/*/*/*/p/*/\\@class").Evaluate(result).First().Value);
        }
    }
}
