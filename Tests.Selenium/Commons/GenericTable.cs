using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Selenium.ToscaObstacleTests.Commons
{
    public class GenericTable
    {
        //By rowLocator;

        /// <summary>
        ///     The table data.
        /// </summary>
        private readonly List<Row> tableData = new List<Row>();

  //      public PaginationElement PaginationElement { get; set; }
  //      public PaginationELementForElasticSearch PaginationElementel { get; set; }
        Action<GenericTable> buildTableAct;
        /// <summary>
        /// Build table object
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="rowLocator">
        /// </param>
        /// <param name="colLocators">
        /// </param>
        /// <returns>
        /// The <see cref="Table"/>.
        /// </returns>
        public static GenericTable BuildTable(IWebDriver driver, By rowLocator, params KeyValuePair<string, By>[] colLocators)
        {
            //this.rowLocator = rowLocator;

            return new GenericTable(t =>
            {
                var rows = driver.FindElements(rowLocator);

                foreach (var r in rows)
                {
                    var rowData = t.NewRow();
                    rowData.rowElement = r;

                    foreach (var loc in colLocators)
                    {
                        try
                        {
                            rowData.AddColumn(loc.Key, r.FindElement(loc.Value));
                        }
                        catch (NoSuchElementException)
                        {
                            // ignore 
                        }
                    }
                }
            });

        }

        /// <summary>
        /// The build table.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="rows">
        /// The rows.
        /// </param>
        /// <param name="colLocators">
        /// The col locators.
        /// </param>
        /// <returns>
        /// The <see cref="Table"/>.
        /// </returns>
        public static GenericTable BuildTable(ICollection<IWebElement> rows,
            params KeyValuePair<string, By>[] colLocators)
        {
            return new GenericTable(t =>
            {

                foreach (var r in rows)
                {
                    var rowData = t.NewRow();
                    rowData.rowElement = r;

                    foreach (var loc in colLocators)
                    {
                        try
                        {
                            rowData.AddColumn(loc.Key, r.FindElement(loc.Value));
                        }
                        catch (NoSuchElementException)
                        {
                            System.Console.WriteLine("can't find table elemment:" + loc);
                        }
                    }
                }
            });
        }

        /// <summary>
        /// The build table.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="tableEl">
        /// The table el.
        /// </param>
        /// <param name="colsNames">
        /// The cols names.
        /// </param>
        /// <returns>
        /// The <see cref="Table"/>.
        /// </returns>
        public static GenericTable BuildTable(IWebElement tableEl, string[] colsNames = null)
        {
            return new GenericTable(t =>
            {
                // read headers
                var rowsBy = By.TagName("tr");
                var colsBy = By.TagName("td");

                var isHeader = false;
                try
                {
                    isHeader = tableEl.FindElements(By.TagName("th")).Count > 0; // get header count if it exists
                }
                catch
                {
                    // ignore
                }

                // var rows = tableEl.FindElements(rowsBy).Skip(isHeader ? 1 : 0);
                var rows = tableEl.FindElements(rowsBy).GetEnumerator();
                if (isHeader && rows.MoveNext())
                {
                    if (colsNames == null)
                    {
                        colsNames = rows.Current.FindElements(By.TagName("th")).Select(e => e.Text).ToArray();
                    }
                }


                while (rows.MoveNext())
                {
                    var tds = rows.Current.FindElements(colsBy);

                    if (tds.Count == 0) continue; // skip header columns 

                    var rowData = t.NewRow();
                    rowData.rowElement = rows.Current;

                    for (var i = 0; i < tds.Count; i++)
                    {
                        try
                        {
                            if (colsNames != null && colsNames[i] != null) rowData.AddColumn(colsNames[i], tds[i]);
                            else rowData.AddColumn(i.ToString(), tds[i]);

                        }
                        catch (Exception)
                        {
                            //ignore: could fail due to  duplicate column names
                        }
                    }
                }
            });
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        public GenericTable(List<Row> data)
        {
            tableData = data;
        }




        private GenericTable(Action<GenericTable> act)
        {
            this.buildTableAct = act;
        }


        //public GenericTable WithPagination(PaginationElement el)
        //{
        //    PaginationElement = el;
        //    return this;
        //}



    //    public GenericTable WithPaginationForElasticSearch(PaginationELementForElasticSearch element)
    //    {
    //        PaginationElementel = element;
    //        return this;
    //
    //    }
        /// <summary>
        ///     The new row.
        /// </summary>
        /// <returns>
        ///     The <see cref="Row" />.
        /// </returns>
        protected Row NewRow()
        {
            var r = new Row();
            tableData.Add(r);

            return r;
        }

        /// <summary>
        ///     The get rows.
        /// </summary>
        /// <returns>
        ///     The <see cref="List" />.
        /// </returns>
        public List<Row> GetRows()
        {
            buildTableAct(this);
            return tableData;
        }

        public Row GetRow(int index)
        {
            buildTableAct(this);
            return tableData[index];
        }

        /// <summary>
        /// search a match row in table.
        /// if there is any exception in predict, will be ignored and treated as not match
        /// return null if not found
        /// </summary>
        /// <param name="predict"></param>
        /// <returns></returns>
        //public Row FindRow(Predicate<Row> predict)
        //{
        //    if (PaginationElement != null)
        //    {
        //        try
        //        {
        //            return PaginationElement.FindInPage<Row>(() =>
        //            {
        //                return GetRows().FirstOrDefault(r => { try { return predict(r); } catch { return false; } });
        //            });
        //        }
        //        catch
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        return GetRows().FirstOrDefault(r => { try { return predict(r); } catch { return false; } });
        //    }
        //}

        /// <summary>
        ///     The row.
        /// </summary>
        public class Row
        {
            /// <summary>
            ///     The row data.
            /// </summary>
            private readonly Dictionary<string, IWebElement> rowData;

            /// <summary>
            /// Initializes a new instance of the <see cref="Row"/> class.
            /// </summary>
            /// <param name="data">
            /// The data.
            /// </param>
            public Row(Dictionary<string, IWebElement> data)
            {
                rowData = data;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Row" /> class.
            /// </summary>
            public Row()
            {
                rowData = new Dictionary<string, IWebElement>();
            }

            /// <summary>
            ///     Gets or sets the row element.
            /// </summary>
            public IWebElement rowElement { get; set; }

            /// <summary>
            /// The add column.
            /// </summary>
            /// <param name="name">
            /// The name.
            /// </param>
            /// <param name="value">
            /// The value.
            /// </param>
            /// <returns>
            /// The <see cref="Row"/>.
            /// </returns>
            public Row AddColumn(string name, IWebElement value)
            {
                rowData.Add(name, value);
                return this;
            }

            /// <summary>
            /// Get column data
            /// </summary>
            /// <param name="col">
            /// </param>
            /// <returns>
            /// element, null if not found
            /// </returns>
            public IWebElement GetColumn(string col)
            {
                try
                {
                    return rowData[col];
                }
                catch
                {
                    return null;
                }
            }

            public IWebElement GetColumn(int index)
            {
                try
                {
                    return rowData.ElementAt(index).Value;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}