using Common;
using ExigoService;
using System.Web.Mvc;

namespace Backoffice.Services
{
    public class TreeService
    {
        public int NodeCounter = 0;
        public string BuildTree<T>(TagBuilder html, Controller controller, T node, TreeTypes treeType) where T : INestedTreeNode<T>
        {
            var table = new TagBuilder("table");
            table.Attributes.Add("cellpadding", "0");
            table.Attributes.Add("cellspacing", "0");
            table.Attributes.Add("border", "0");
            table.Attributes.Add("width", "100%");

            var tbody = new TagBuilder("tbody");

            var tr = new TagBuilder("tr");
            tr.AddCssClass("node-cells");

            var td = new TagBuilder("td");
            td.AddCssClass("node-cell");
            if (node.Children.Count <= 1)
            {
                td.Attributes.Add("colspan", "2");
            }
            else
            {
                td.Attributes.Add("colspan", (node.Children.Count * 2).ToString());
            }


            // Node HTML
            if (treeType == TreeTypes.Unilevel)
            {
                td.InnerHtml += controller.RenderPartialViewToString("UnilevelTreeNode", node);
            }
            else
            {
                td.InnerHtml += controller.RenderPartialViewToString("BinaryTreeNode", node);
            }            

            NodeCounter++;

            tr.InnerHtml = td.ToString();
            tbody.InnerHtml = tr.ToString();


            if (node.Children.Count > 0)
            {
                var downlineTr = new TagBuilder("tr");
                var downlineTd = new TagBuilder("td");

                downlineTd.Attributes.Add("colspan", (node.Children.Count * 2).ToString());


                var nullNodeClass = (node.IsOpenPosition || node.IsNullPosition) ? "null-node" : "";


                // Draw the connecting line from the parent node to the horizontal line 
                var verticalLine = new TagBuilder("div");
                verticalLine.AddCssClass("line down");
                verticalLine.AddCssClass(nullNodeClass);

                downlineTd.InnerHtml += verticalLine.ToString();
                downlineTr.InnerHtml += downlineTd.ToString();
                tbody.InnerHtml += downlineTr.ToString();


                // Draw the horizontal lines
                var linesTr = new TagBuilder("tr");
                var horizontalLineCounter = 0;
                var maxHorizontalLines = node.Children.Count - 1;

                foreach (var child in node.Children)
                {
                    var leftLine = new TagBuilder("td");
                    leftLine.AddCssClass("line left");
                    leftLine.AddCssClass(nullNodeClass);
                    if (horizontalLineCounter > 0) leftLine.AddCssClass("top");
                    linesTr.InnerHtml += leftLine.ToString();

                    var rightLine = new TagBuilder("td");
                    rightLine.AddCssClass("line right");
                    rightLine.AddCssClass(nullNodeClass);
                    if (horizontalLineCounter < maxHorizontalLines) rightLine.AddCssClass("top");
                    linesTr.InnerHtml += rightLine.ToString();

                    horizontalLineCounter++;
                };

                // Horizontal line shouldn't extend beyond the first and last child branches
                tbody.InnerHtml += linesTr.ToString();


                var childNodesTr = new TagBuilder("tr");
                foreach (var childNode in node.Children)
                {
                    var childNodeTd = new TagBuilder("td");
                    childNodeTd.AddCssClass("node-container");
                    childNodeTd.AddCssClass(nullNodeClass);
                    childNodeTd.Attributes.Add("colspan", "2");

                    // Build recursively
                    BuildTree(childNodeTd, controller, childNode, treeType);

                    childNodesTr.InnerHtml += childNodeTd.ToString();
                }

                tbody.InnerHtml += childNodesTr.ToString();
            }

            table.InnerHtml += tbody.ToString();


            html.InnerHtml += table.ToString();

            return html.ToString();
        }
    }
}