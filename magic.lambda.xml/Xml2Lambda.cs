/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Linq;
using System.Web;
using HtmlAgilityPack;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.xml
{
    /// <summary>
    /// [xml2lambda] slot for transforming a piece of HTML to a lambda hierarchy.
    /// </summary>
    [Slot(Name = "xml2lambda")]
    public class Xml2Lambda : ISlot
    {
        /// <summary>
        /// Slot implementation.
        /// </summary>
        /// <param name="signaler">Signaler that raised signal.</param>
        /// <param name="input">Arguments to slot.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            var xml = input.GetEx<string>();
            input.Value = null;
            var doc = new HtmlDocument();
            doc.LoadHtml(xml);
            ParseXmlDocument(input, doc.DocumentNode);
        }

        #region [ -- Private helper methods -- ]

        static Xml2Lambda()
        {
            // Making sure "form" element conforms to relational DOM structure
            HtmlNode.ElementsFlags.Remove("form");
        }

        static void ParseXmlDocument(Node resultNode, HtmlNode xmlNode)
        {
            // Skipping document node
            if (xmlNode.Name != "#document")
            {
                // Adding all attributes
                resultNode.AddRange(
                    xmlNode.Attributes
                        .Select(ix => new Node("@" + ix.Name, HttpUtility.HtmlDecode(ix.Value))));

                // Then the name of HTML element
                resultNode.Name = xmlNode.Name;
                if (xmlNode.Name == "#text")
                {
                    // This is a "simple node", with no children, only HTML content
                    resultNode.Value = HttpUtility.HtmlDecode(xmlNode.InnerText);
                }
            }

            // Then looping through each child HTML element
            foreach (var idxChild in xmlNode.ChildNodes)
            {
                // We don't add comments or empty elements
                if (idxChild.Name != "#comment")
                {
                    if (idxChild.Name == "#text" && string.IsNullOrEmpty(idxChild.InnerText.Trim()))
                        continue;
                    resultNode.Add(new Node());
                    ParseXmlDocument(resultNode.Children.Last(), idxChild);
                }
            }
        }

        #endregion
    }
}
