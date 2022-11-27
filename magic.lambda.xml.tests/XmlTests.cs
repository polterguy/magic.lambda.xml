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
.xml:@""<CATALOG>
  <PLANT>
    <COMMON a=""""qwertyuio"""">Bloodroot</COMMON>
    <BOTANICAL>Sanguinaria canadensis</BOTANICAL>
    <ZONE>4</ZONE>
    <LIGHT>Mostly Shady</LIGHT>
    <PRICE>$2.44</PRICE>
    <AVAILABILITY>031599</AVAILABILITY>
  </PLANT>
</CATALOG>
""
xml2lambda:x:@.xml");
            Assert.Equal("Bloodroot", new Expression("**/xml2lambda/*/*/*/COMMON/*/\\#text").Evaluate(result).First().Value);
            Assert.Equal("qwertyuio", new Expression("**/xml2lambda/*/*/*/COMMON/*/\\@a").Evaluate(result).First().Value);
        }

        [Fact]
        public void Roundtrip()
        {
            Common.Evaluate(@"
.xml:@""<CATALOG>
  <PLANT>
    <COMMON a=""""qwertyuio"""">Bloodroot</COMMON>
    <BOTANICAL>Sanguinaria canadensis</BOTANICAL>
    <ZONE>4</ZONE>
    <LIGHT>Mostly Shady</LIGHT>
    <PRICE>$2.44</PRICE>
    <AVAILABILITY>031599</AVAILABILITY>
  </PLANT>
</CATALOG>""
xml2lambda:x:@.xml
lambda2xml:x:@xml2lambda/*
if
   neq:x:@lambda2xml
      get-value:x:@.xml
   .lambda
      throw:Not matching");
        }
    }
}
