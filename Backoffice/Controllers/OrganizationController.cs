using Backoffice.Models;
using Backoffice.Services;
using Backoffice.ViewModels;
using Common;
using Common.Api.ExigoWebService;
using Common.Services;
using ExigoService;
using ExigoWeb.Kendo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Dapper;
using System.Dynamic;
using Newtonsoft.Json;

using System.Data.SqlClient;
using Serilog;

namespace Backoffice.Controllers
{
    public class OrganizationController : Controller
    {
        public bool AllowWaitingRoomPlacements = false;
        public string TreeToUse = GlobalSettings.Exigo.UseBinary ? "BinaryDownline" : "UnilevelDownline";



        public ActionResult Index()
        {
            return View();
        }

        #region Trees
        // Binary Tree
        [Route("binary-tree-viewer/{token?}")]
        public ActionResult BinaryTreeViewer(string token = "")
        {
            if (!GlobalSettings.Exigo.UseBinary) return RedirectToAction("UnilevelTreeViewer", new { token });
            var model = new TreeViewerViewModel();
            var ranks = RankService.GetRanks();

            foreach (var rank in ranks)
            {
                var listItem = new Rank
                {
                    RankID = rank.RankID,
                    RankDescription = rank.RankDescription
                };

                model.Ranks.Add(listItem);
            }
            var topCustomerID = Identity.Current.CustomerID;

            if (!token.IsNullOrEmpty())
            {
                var tokenCustomerID = Convert.ToInt32(Security.Decrypt(token, Identity.Current.CustomerID));
                if (tokenCustomerID != 0 && tokenCustomerID != topCustomerID && ExigoDAL.IsCustomerInlDownline(topCustomerID, tokenCustomerID))
                {
                    topCustomerID = tokenCustomerID;
                }
            }
            model.TopCustomerID = topCustomerID;

            return View(model);
        }
        public JsonNetResult FetchBinaryTree(int id)
        {
            var currentPeriod = ExigoDAL.GetCurrentPeriod(PeriodTypes.Default);
            var backofficeOwnerID = Identity.Current.CustomerID;
            var customerID = (id != 0) ? id : backofficeOwnerID;
            var controller = this;
            var tasks = new List<Task>();



            // Assemble Tree
            var treehtml = string.Empty;
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var request = new GetTreeRequest
                {
                    TopCustomerID = customerID,
                    Levels = 3,
                    Legs = 2,
                    IncludeNullPositions = true,
                    IncludeOpenPositions = true
                };

                var nodeDataRecords = new List<dynamic>();
                using (var context = ExigoDAL.Sql())
                {
                    nodeDataRecords = context.Query(@"
                                SELECT 
	                                c.CustomerID
	                                ,c.FirstName
	                                ,c.LastName
	                                ,c.Company
	                                ,c.EnrollerID
	                                ,c.CreatedDate
                                    ,d.Level
	                                ,RankID = COALESCE(c.RankID, 0)		
                                    ,d.ParentID
	                                ,HasAutoOrder = cast(case when COUNT(ao.AutoOrderID) > 0 then 1 else 0 end as bit)
	                                ,IsPersonallyEnrolled = cast(case when c.EnrollerID = @topcustomerid then 1 else 0 end as bit)
                                    ,PlacementID = bt.Placement
                                    ,[Left] = bt.Lft
                                    ,[Right] = bt.Rgt

                                FROM BinaryDownline d
	                                INNER JOIN Customers c
		                                ON c.CustomerID = d.CustomerID
	                                LEFT JOIN AutoOrders ao
		                                ON ao.CustomerID = c.CustomerID
			                            AND ao.AutoOrderStatusID = 0
                                    INNER JOIN BinaryTree bt
	                                    ON bt.CustomerID = c.CustomerID
                                WHERE 
	                                d.DownlineCustomerID = @topcustomerid
		                                AND Level <= @level

                                GROUP BY
	                                c.CustomerID
	                                ,c.FirstName
	                                ,c.LastName
	                                ,c.Company
	                                ,c.EnrollerID
	                                ,c.CreatedDate
                                    ,d.Level
	                                ,c.RankID	  
                                    ,d.ParentID        
                                    ,bt.Placement
                                    ,bt.Lft
                                    ,bt.Rgt
                    ", new
                    {
                        periodtypeid = currentPeriod.PeriodTypeID,
                        periodid = currentPeriod.PeriodID,
                        topcustomerid = customerID,
                        level = request.Levels
                    }).ToList();
                }

                var nodes = new List<BinaryNestedTreeNode>();
                foreach (var nodeDataRecord in nodeDataRecords)
                {
                    var node = new BinaryNestedTreeNode();

                    node.CustomerID           = nodeDataRecord.CustomerID;
                    node.ParentCustomerID     = nodeDataRecord.ParentID;
                    node.Level                = nodeDataRecord.Level;
                    node.HasAutoOrder         = nodeDataRecord.HasAutoOrder;
                    node.IsPersonallyEnrolled = nodeDataRecord.IsPersonallyEnrolled;
                    node.Customer.CustomerID  = nodeDataRecord.CustomerID;
                    node.Customer.FirstName   = nodeDataRecord.FirstName;
                    node.Customer.LastName    = nodeDataRecord.LastName;
                    node.Customer.Company     = nodeDataRecord.Company;
                    node.Customer.EnrollerID  = nodeDataRecord.EnrollerID;
                    node.Customer.CreatedDate = nodeDataRecord.CreatedDate;
                    node.CurrentRank.RankID   = nodeDataRecord.RankID;
                    node.PlacementID          = nodeDataRecord.PlacementID;
                    node.Left                 = nodeDataRecord.Left;
                    node.Right                = nodeDataRecord.Right;

                    nodes.Add(node);
                }


                var nodeTree = ExigoDAL.OrganizeNestedTreeNodes(nodes, request).FirstOrDefault();

                var container = new TagBuilder("div");
                container.AddCssClass("jOrgChart");

                var service = new TreeService();
                treehtml = service.BuildTree(container, controller, nodeTree, TreeTypes.Binary);
            }));


            // Assemble Customer Details
            var detailsModel = new CustomerDetailsViewModel();
            tasks.Add(Task.Factory.StartNew(() =>
            {
                detailsModel.Customer = Customers.GetCustomer(customerID);

                var volumes = ExigoDAL.GetCustomerVolumes(new GetCustomerVolumesRequest()
                {
                    CustomerID = customerID,
                    PeriodID = currentPeriod.PeriodID,
                    PeriodTypeID = currentPeriod.PeriodTypeID
                });

                detailsModel.PaidRank = volumes.PayableAsRank;
                detailsModel.HighestRank = volumes.HighestAchievedRankThisPeriod;
            }));




            // Get the upline
            var uplineModel = new UplineViewModel();
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var uplineNodes = new List<UplineNodeViewModel>();

                using (var context = ExigoDAL.Sql())
                {
                    uplineNodes = context.Query<UplineNodeViewModel>(@"
                     SELECT 
                        b.CustomerID,
                        b.Level,
                        c.FirstName,
                        c.LastName                        
                    FROM BinaryUpline b
                        LEFT JOIN Customers c 
                            ON c.CustomerID = b.CustomerID
                    WHERE 
                        b.UplineCustomerID = @bottomcustomerid
                    ORDER BY Level",
                        new
                        {
                            bottomcustomerid = customerID
                        }).ToList();
                }

                // Filter out the nodes that don't belong
                var isFound = false;
                var filteredNodes = new List<UplineNodeViewModel>();
                foreach (var node in uplineNodes)
                {
                    if (node.CustomerID == backofficeOwnerID)
                    {
                        isFound = true;
                    }

                    if (isFound) filteredNodes.Add(node);
                }

                // Set the levels
                for (int i = 0; i < filteredNodes.Count; i++)
                {
                    filteredNodes[i].Level = i;
                }

                // Assemble Upline HTML
                uplineModel.UplineNodes = filteredNodes;
            }));


            Task.WaitAll(tasks.ToArray());
            tasks.Clear();


            // Render the partials
            var customerdetailshtml = this.RenderPartialViewToString("_CustomerDetails", detailsModel);
            var uplinehtml = this.RenderPartialViewToString("_CustomerUpline", uplineModel);


            // Return our data
            return new JsonNetResult(new
            {
                treehtml = treehtml,
                customerdetailshtml = customerdetailshtml,
                uplinehtml = uplinehtml,
                id = customerID
            });
        }
        public JsonNetResult BinaryUpOneLevel(int id)
        {

            var inOwnersTree = ExigoDAL.IsCustomerInBinaryDownline(Identity.Current.CustomerID, id);

            if (inOwnersTree)
            {
                var parentId = 0;
                using (var context = ExigoDAL.Sql())
                {
                    parentId = context.Query<int>(@"
                    select top 1 ParentID from BinaryTree where CustomerID = @id
                    ", new
                    {
                        id
                    }).FirstOrDefault();
                }


                return new JsonNetResult(new
                {
                    success = true,
                    parentId
                });
            }

            return new JsonNetResult(new
            {
                success = false
            });
        }
        public JsonNetResult BinaryBottomNode(int id, string direction)
        {
            try
            {
                var bottomCustomerID = 0;

                using (var context = ExigoDAL.Sql())
                {
                    bottomCustomerID = context.Query<int>(@"
                         With tree (CustomerID, SponsorID, placement,  nestedlevel)
                         as
                         (
                         Select CustomerID, ParentID, placement,  nestedlevel
                         from binarytree 
 
                         where  customerid = @customerID 
                         union all
                         select u.CustomerID, u.ParentID, u.placement, u.nestedlevel
                         from binarytree u 
 
                         inner join tree t  on t.customerid = u.ParentID
                                         and u.placement = @placementID
                         )
                         select top 1 
                         CustomerID 
                         From tree 
 
                         order by nestedlevel desc
                         option (maxrecursion 0)
                     ", new
                    {
                        customerID = id,
                        placementID = (direction.ToLower() == "left") ? 0 : 1
                    }).FirstOrDefault();
                }

                var test = bottomCustomerID;

                return new JsonNetResult(new
                {
                    success = true,
                    customerID = bottomCustomerID
                });
            }
            catch (Exception ex)
            {
                return new JsonNetResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        public JsonNetResult BinarySearch(string query)
        {
            try
            {
                // assemble a list of customers who match the search criteria
                var results = new List<SearchResult>();


                using (var context = ExigoDAL.Sql())
                {
                    results = context.Query<SearchResult>(@"
                        SELECT 
                            c.CustomerID,
                            c.FirstName,
                            c.LastName
                                   
                        FROM BinaryDownline d                         

                        LEFT JOIN  Customers c
                           ON d.CustomerID = c.CustomerID
                            
                        WHERE d.downlinecustomerid = @topCustomerID
                            AND (c.FirstName + ' ' + c.LastName LIKE '%' + @query + '%'                                 
                                OR c.CustomerID LIKE @query)                                        
                        ", new
                    {
                        query = query,
                        topCustomerID = Identity.Current.CustomerID
                    }).ToList();
                }

                return new JsonNetResult(new
                {
                    success = true,
                    results = results
                });
            }
            catch (Exception)
            {
                return new JsonNetResult(new
                {
                    success = false
                });
            }
        }

        // D3js Tree
        public ActionResult TreeViewer(string token = "")
        {
            var model = new TreeViewerViewModel();

            model.OwnerCustomerID = Identity.Current.CustomerID;
            var _hasToken = !token.IsNullOrEmpty();

            if (_hasToken)
            {
                model.OwnerCustomerID = Convert.ToInt32(Security.Decrypt(token, Identity.Current.CustomerID));

                model.Token = token;

                using (var context = ExigoDAL.Sql())
                {
                    var name = context.Query<string>(@"select FirstName + ' ' + LastName from Customers where CustomerID = @customerID", new { customerID = model.OwnerCustomerID }).FirstOrDefault();
                    model.OwnerName = name;
                }
            }

            // Logic to determine if we need to pull waiting room customers or not
            ViewBag.AllowWaitingRoomPlacements = AllowWaitingRoomPlacements;

            var ranks = RankService.GetRanks();

            foreach (var rank in ranks.OrderBy(r => r.RankID))
            {
                var listItem = new Rank();

                listItem.RankID = rank.RankID;
                listItem.RankDescription = rank.RankDescription;

                model.Ranks.Add(listItem);
            }

            return View(model);
        }
        /// <summary>
        /// Fetches the tree for the d3js tree viewer.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentNodeLevel">This is the level of the passed-in node.</param>
        /// <returns></returns>
        public JsonNetResult FetchTree(int id, int parentNodeLevel = 0)
        {
            try
            {
                int backofficeOwnerID = Identity.Current.CustomerID;
                int customerID = (id != 0) ? id : backofficeOwnerID;
                var controller = this;
                var tasks = new List<Task>();

                // Logic to determine if we need to pull waiting room customers or not
                ViewBag.AllowWaitingRoomPlacements = false;



                // Assemble Tree
                //var treehtml = string.Empty;
                var jsonTree = string.Empty;
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    var request = new GetTreeRequest
                    {
                        TopCustomerID = customerID,
                        Levels = 3,
                        Legs = 0,
                        IncludeNullPositions = false,
                        IncludeOpenPositions = false
                    };


                    var nodeDataRecords = new List<dynamic>();
                    using (var context = ExigoDAL.Sql())
                    {
                        nodeDataRecords = context.Query(@"
                            SELECT
	                            c.CustomerID
	                            --,HasChildren = cast(CASE WHEN ut.Rgt - ut.Lft > 1 THEN 1 ELSE 0 END as bit)
	                            --,Lft = ut.Lft
	                            --,Rgt = ut.Rgt
	                            ,c.FirstName
	                            ,c.LastName
	                            ,c.Company
                                ,c.Email
                                ,c.Phone
	                            ,EnrollerID = isnull(c.EnrollerID, 0)
	                            ,c.CreatedDate
                                ,RankID = COALESCE(c.RankID, 0)		
                                ,RankDescription = COALESCE(r.RankDescription, '')
	                            ,IsPersonallyEnrolled = cast(case when c.EnrollerID = @topcustomerid then 1 else 0 end as bit)
                                ,SponsorID = isnull(d.SponsorID, 0)
                                ,d.Level
                                ,d.Placement
                                ,d.IndentedSort
                                ,cs.CustomerStatusDescription
                                ,cs.CustomerStatusID

                            FROM UnilevelDownline d	                           
	                            INNER JOIN Customers c
		                            ON c.CustomerID = d.CustomerID
                                LEFT JOIN Ranks r  
                                    ON r.RankID = c.RankID
                                INNER JOIN CustomerStatuses cs
                                    ON c.CustomerStatusID = cs.CustomerStatusID

                            WHERE d.DownlineCustomerID = @topcustomerid
	                            AND d.Level <= @level 

                            GROUP BY
	                            c.CustomerID
	                            ,c.FirstName
	                            ,c.LastName
	                            ,c.Company
                                ,c.Email
                                ,c.Phone
	                            ,c.EnrollerID
	                            ,c.CreatedDate
	                            ,c.RankID	
                                ,d.SponsorID
                                ,d.Level
                                ,d.Placement
                                ,d.IndentedSort
                                ,cs.CustomerStatusDescription
                                ,cs.CustomerStatusID
                                ,r.RankDescription
	                            --,ut.Lft
	                            --,ut.Rgt

                            ORDER BY
	                            d.IndentedSort
                    ", new
                        {
                            topcustomerid = customerID,
                            level = request.Levels
                        }).ToList();
                    }


                    var nodes = new List<UnilevelNestedTreeNode>();
                    foreach (var nodeDataRecord in nodeDataRecords)
                    {
                        var nodeCustomer = new Customer()
                        {
                            CustomerID = nodeDataRecord.CustomerID,
                            FirstName = nodeDataRecord.FirstName,
                            LastName = nodeDataRecord.LastName,
                            Company = nodeDataRecord.Company,
                            PrimaryPhone = nodeDataRecord.Phone,
                            Email = nodeDataRecord.Email,
                            EnrollerID = nodeDataRecord.EnrollerID,
                            CreatedDate = nodeDataRecord.CreatedDate,
                            
                        };
                        nodeCustomer.CustomerStatus = new CustomerStatus(nodeDataRecord.CustomerStatusID, nodeDataRecord.CustomerStatusDescription);
                        var node = new UnilevelNestedTreeNode
                        {
                            ProfileID = Security.Encrypt(nodeDataRecord.CustomerID, backofficeOwnerID),
                            CustomerID = nodeDataRecord.CustomerID,
                            ParentCustomerID = nodeDataRecord.SponsorID,
                            Level = nodeDataRecord.Level + parentNodeLevel,
                            PlacementID = nodeDataRecord.Placement,
                            IndentedSort = nodeDataRecord.IndentedSort,
                            IsPersonallyEnrolled = nodeDataRecord.IsPersonallyEnrolled,
                            Customer = nodeCustomer,

                        };
                        node.CurrentRank.RankID = nodeDataRecord.RankID;
                        node.CurrentRank.RankDescription = CommonResources.Ranks(nodeDataRecord.RankID, defaultDescription: nodeDataRecord.RankDescription);

                        
                        node.Loaded = node.Level == 0;
                        //node.HasChildren                 = nodeDataRecord.HasChildren;

                        nodes.Add(node);
                    }

                    var nodeTree = ExigoDAL.OrganizeNestedTreeNodes(nodes, request).FirstOrDefault();

                    jsonTree = JsonConvert.SerializeObject(nodeTree).Replace("Children","children");
                }));


                Task.WaitAll(tasks.ToArray());
                tasks.Clear();

                // Return our data
                return new JsonNetResult(new
                {
                    success = true,
                    jsonTree = jsonTree,
                    id = customerID
                });
            }
            catch (Exception ex)
            {
                return new JsonNetResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // Unilevel Tree
        [Route("~/treeviewer/{token?}")]
        public ActionResult UnilevelTreeViewer(string token = "")
        {
            if (GlobalSettings.Exigo.UseBinary) return RedirectToAction("BinaryTreeViewer", new { token });
            var model = new TreeViewerViewModel();

            // Logic to determine if we need to pull waiting room customers or not
            ViewBag.AllowWaitingRoomPlacements = AllowWaitingRoomPlacements;


            var ranks = RankService.GetRanks();

            foreach (var rank in ranks)
            {
                var listItem = new ExigoService.Rank();

                listItem.RankID = rank.RankID;
                listItem.RankDescription = rank.RankDescription;

                model.Ranks.Add(listItem);
            }

            var topCustomerID = Identity.Current.CustomerID;

            if (!token.IsNullOrEmpty())
            {
                var tokenCustomerID = Convert.ToInt32(Security.Decrypt(token));
                if (tokenCustomerID != 0 && tokenCustomerID != topCustomerID && ExigoDAL.IsCustomerInlDownline(topCustomerID, tokenCustomerID))
                {
                    topCustomerID = tokenCustomerID;
                }
            }
            model.TopCustomerID = topCustomerID;

            return View(model);
        }
        public JsonNetResult FetchUnilevelTree(string token = "")
        {
            try
            {
                var currentPeriod = ExigoDAL.GetCurrentPeriod(PeriodTypes.Default);
                var backofficeOwnerID = Identity.Current.CustomerID;
                var id = 0;
                if (token.IsNotNullOrEmpty())
                {
                    id = Convert.ToInt32(Security.Decrypt(token));
                }
                var customerID = (id != 0) ? id : backofficeOwnerID;
                var controller = this;
                var tasks = new List<Task>();

                // Logic to determine if we need to pull waiting room customers or not
                ViewBag.AllowWaitingRoomPlacements = AllowWaitingRoomPlacements;



                // Assemble Tree
                var treehtml = string.Empty;
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    var request = new GetTreeRequest
                    {
                        TopCustomerID = customerID,
                        Levels = 3,
                        Legs = 0,
                        IncludeNullPositions = true,
                        IncludeOpenPositions = true
                    };


                    var nodeDataRecords = new List<dynamic>();
                    using (var context = ExigoDAL.Sql())
                    {
                        nodeDataRecords = context.Query(@"
                            SELECT
	                                c.CustomerID
	                                ,c.FirstName
	                                ,c.LastName
	                                ,c.Company
	                                ,EnrollerID = isnull(c.EnrollerID, 0)
	                                ,c.CreatedDate
                                    ,RankID = COALESCE(c.RankID, 0)		
	                                ,HasAutoOrder = cast(case when COUNT(ao.AutoOrderID) > 0 then 1 else 0 end as bit)
	                                ,IsPersonallyEnrolled = cast(case when c.EnrollerID = @topcustomerid then 1 else 0 end as bit)
                                    ,SponsorID = isnull(d.SponsorID, 0)
                                    ,d.Level
                                    ,d.Placement
                                    ,d.IndentedSort
                                    ,cs.CustomerStatusDescription
                                    ,cs.CustomerStatusID

                                FROM UnilevelDownline d
	                                INNER JOIN Customers c
		                                ON c.CustomerID = d.CustomerID
                                    INNER JOIN CustomerStatuses cs
                                        ON c.CustomerStatusID = cs.CustomerStatusID
	                                LEFT JOIN AutoOrders ao
		                                ON ao.CustomerID = c.CustomerID
			                                AND ao.AutoOrderStatusID = 0

                                WHERE d.DownlineCustomerID = @topcustomerid
	                                AND Level <= @level 

                                GROUP BY
	                                c.CustomerID
	                                ,c.FirstName
	                                ,c.LastName
	                                ,c.Company
	                                ,c.EnrollerID
	                                ,c.CreatedDate
	                                ,c.RankID	
                                    ,d.SponsorID
                                    ,d.Level
                                    ,d.Placement
                                    ,d.IndentedSort
                                    ,cs.CustomerStatusDescription
                                    ,cs.CustomerStatusID

                                ORDER BY
	                                d.IndentedSort
                    ", new
                        {
                            periodtypeid = currentPeriod.PeriodTypeID,
                            periodid = currentPeriod.PeriodID,
                            topcustomerid = customerID,
                            level = request.Levels
                        }).ToList();
                    }


                    var nodes = new List<UnilevelNestedTreeNode>();
                    foreach (var nodeDataRecord in nodeDataRecords)
                    {
                        var node = new UnilevelNestedTreeNode();

                        node.CustomerID = nodeDataRecord.CustomerID;
                        node.ParentCustomerID = nodeDataRecord.SponsorID;
                        node.Level = nodeDataRecord.Level;
                        node.PlacementID = nodeDataRecord.Placement;
                        node.IndentedSort = nodeDataRecord.IndentedSort;
                        node.HasAutoOrder = nodeDataRecord.HasAutoOrder;
                        node.IsPersonallyEnrolled = nodeDataRecord.IsPersonallyEnrolled;
                        node.Customer.CustomerID = nodeDataRecord.CustomerID;
                        node.Customer.FirstName = nodeDataRecord.FirstName;
                        node.Customer.LastName = nodeDataRecord.LastName;
                        node.Customer.Company = nodeDataRecord.Company;
                        node.Customer.EnrollerID = nodeDataRecord.EnrollerID;
                        node.Customer.CreatedDate = nodeDataRecord.CreatedDate;
                        node.CurrentRank.RankID = nodeDataRecord.RankID;
                        node.CurrentRank.RankDescription = nodeDataRecord.RankDescription;
                        node.Customer.CustomerStatus = new CustomerStatus(nodeDataRecord.CustomerStatusID, nodeDataRecord.CustomerStatusDescription);

                        nodes.Add(node);
                    }

                    var nodeTree = ExigoDAL.OrganizeNestedTreeNodes(nodes, request).FirstOrDefault();

                    var container = new TagBuilder("div");
                    container.AddCssClass("jOrgChart");

                    var service = new TreeService();
                    treehtml = service.BuildTree(container, controller, nodeTree, TreeTypes.Unilevel);
                }));



                // Assemble Customer Details
                var detailsModel = new CustomerDetailsViewModel();
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    detailsModel.Customer = Customers.GetCustomer(customerID);

                    var volumes = ExigoDAL.GetCustomerVolumes(new GetCustomerVolumesRequest()
                    {
                        CustomerID = customerID,
                        PeriodID = currentPeriod.PeriodID,
                        PeriodTypeID = currentPeriod.PeriodTypeID
                    });

                    detailsModel.PaidRank = volumes.PayableAsRank;
                    detailsModel.HighestRank = volumes.HighestAchievedRankThisPeriod;
                }));


                // Don't pull data form waiting room customers if they can't be placed at the moment
                var waitingRoomModel = new WaitingRoomListViewModel();
                if (AllowWaitingRoomPlacements)
                {
                    // Variables for Waiting Room calls
                    var activeCustomerStatus = (int)CustomerStatuses.Active;
                    var waitingRoomCustomerTypes = new List<int> { (int)CustomerTypes.Distributor };
                    var validWaitingRoomMonths = 1; // Months that a Customer has from their Created date to be able to be placed


                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        var customers = new List<WaitingRoomNode>();
                        using (var context = ExigoDAL.Sql())
                        {
                            customers = context.Query<WaitingRoomNode>(@"
                                    select 
	                                    CustomerID = c.CustomerID,
	                                    FirstName = c.FirstName,
	                                    LastName = c.LastName,
	                                    Email = c.Email,
	                                    Phone = c.Phone,
	                                    EnrollerID = isnull(c.EnrollerID, 0),
	                                    EnrollmentDate = CAST(c.CreatedDate as date)
                                    from 
	                                    Customers c
                                    where 
	                                    c.EnrollerID = @topcustomerID
	                                    and c.SponsorID = @topcustomerID
	                                    and c.CustomerStatusID = @activeCustomerStatus
	                                    and c.CustomerTypeID in @customerTypes
	                                    and getDate() < dateadd(m, @validMonths, c.CreatedDate)
                                    order by c.CreatedDate asc",
                                        new
                                        {
                                            topcustomerID = customerID,
                                            activeCustomerStatus = activeCustomerStatus,
                                            customerTypes = waitingRoomCustomerTypes,
                                            validMonths = validWaitingRoomMonths
                                        }).ToList();
                        }

                        waitingRoomModel.WaitingRoomCustomers = customers;
                    }));
                }



                // Get the upline
                var uplineModel = new UplineViewModel();
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    var uplineNodes = new List<UplineNodeViewModel>();

                    using (var context = ExigoDAL.Sql())
                    {
                        uplineNodes = context.Query<UplineNodeViewModel>(@"
                     SELECT 
                        ul.CustomerID,
                        ul.Level,
                        c.FirstName,
                        c.LastName                        
                    FROM UnilevelUpline ul
                        LEFT JOIN Customers c 
                            ON c.CustomerID = ul.CustomerID
                    WHERE 
                        ul.UplineCustomerID = @bottomcustomerid
                    ORDER BY Level",
                            new
                            {
                                bottomcustomerid = customerID
                            }).ToList();
                    }

                    // Filter out the nodes that don't belong
                    var isFound = false;
                    var filteredNodes = new List<UplineNodeViewModel>();
                    foreach (var node in uplineNodes)
                    {
                        if (node.CustomerID == backofficeOwnerID)
                        {
                            isFound = true;
                        }

                        if (isFound) filteredNodes.Add(node);
                    }

                    // Set the levels
                    for (int i = 0; i < filteredNodes.Count; i++)
                    {
                        filteredNodes[i].Level = i;
                    }

                    // Assemble Upline HTML
                    uplineModel.UplineNodes = filteredNodes;
                }));


                Task.WaitAll(tasks.ToArray());
                tasks.Clear();


                // Render the partials
                var customerdetailshtml = this.RenderPartialViewToString("_CustomerDetails", detailsModel);
                var uplinehtml = this.RenderPartialViewToString("_CustomerUpline", uplineModel);
                var waitingroomlisthtml = this.RenderPartialViewToString("_WaitingRoomPlacement", waitingRoomModel);


                // Return our data
                return new JsonNetResult(new
                {
                    treehtml = treehtml,
                    customerdetailshtml = customerdetailshtml,
                    uplinehtml = uplinehtml,
                    waitingroomlisthtml = waitingroomlisthtml,
                    id = customerID
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error Fetching Unilevel Tree: {Message}", ex.Message);
                return new JsonNetResult(new
                {
                    treehtml = Resources.Common.CouldNotFetchData,
                    customerdetailshtml = "",
                    uplinehtml = "",
                    waitingroomlisthtml = ""
                });
            }
        }
        public JsonNetResult UnilevelUpOneLevel(int id)
        {
            var inOwnersTree = ExigoDAL.IsCustomerInUniLevelDownline(Identity.Current.CustomerID, id);

            if (inOwnersTree)
            {
                var parentId = 0;

                using (var context = ExigoDAL.Sql())
                {
                    parentId = context.Query<int>(@"
                    select top 1 SponsorID from UnilevelTree where CustomerID = @id
                    ", new
                    {
                        id
                    }).FirstOrDefault();
                }
                string token = Security.Encrypt(parentId);

                return new JsonNetResult(new
                {
                    success = true,
                    token
                });
            }

            return new JsonNetResult(new
            {
                success = false
            });
        }
        public JsonNetResult UnilevelSearch(string query)
        {
            try
            {
                // assemble a list of customers who match the search criteria
                var results = new List<SearchResult>();


                using (var context = ExigoDAL.Sql())
                {
                    results = context.Query<SearchResult>(@"
                        SELECT 
                            c.CustomerID,
                            c.FirstName,
                            c.LastName
                                   
                        FROM UnilevelDownline d                         

                        LEFT JOIN  Customers c
                           ON d.CustomerID = c.CustomerID
                            
                        WHERE d.downlinecustomerid = @topCustomerID
                            AND (c.FirstName + ' ' + c.LastName LIKE '%' + @query + '%'                                 
                                OR c.CustomerID LIKE @query)                                            
                        ", new
                    {
                        query = query,
                        topCustomerID = Identity.Current.CustomerID
                    }).ToList();
                }

                return new JsonNetResult(new
                {
                    success = true,
                    results = results
                });
            }
            catch (Exception)
            {
                return new JsonNetResult(new
                {
                    success = false
                });
            }
        }
        
        [Route("popoversummary/{id:int=0}")]
        public ActionResult PopoverSummary(int id)
        {
            var model = new TreeNodePopoverViewModel();

            if (id == 0) id = Identity.Current.CustomerID;

            model.Customer = Customers.GetCustomer(id);

            var highestRankAchieved = RankService.GetRank(model.Customer.RankID);
            if (highestRankAchieved != null)
            {
                model.Customer.HighestAchievedRank = highestRankAchieved;
            }


            if (Request.IsAjaxRequest()) return PartialView("_TreeNodePopover", model);
            else return View(model);
        }
        #endregion

        #region Reports

        [Route("~/personally-enrolled-team")] ///
        public ActionResult PersonallyEnrolledList(KendoGridRequest request = null)
        {
            if (Request.HttpMethod.ToUpper() == "GET") return View();
            var customerID = Identity.Current.CustomerID;
            var distributorCustomerTypes = new List<int> { (int)CustomerTypes.Distributor };
            using (var context = new KendoGridDataContext(ExigoDAL.Sql()))
            {
                return context.Query(request, $@"
                    SELECT 
                         c.CustomerID
                        ,c.FirstName
                        ,c.LastName
                        ,CAST(c.CreatedDate as date) AS 'CreatedDate'
                        ,c.Email
                        ,c.Phone
                        ,c.CustomerTypeID

                    FROM {TreeToUse} ed
                        INNER JOIN Customers c 
	                        ON c.CustomerID = ed.CustomerID
                            AND c.CustomerTypeID IN @customertypes

                    WHERE ed.DownlineCustomerID = @CustomerID
                        AND c.EnrollerID = @customerid
            ", new
                {
                    customertypes = distributorCustomerTypes,
                    customerid = customerID
                }).Tokenize("CustomerID");
            }
        }

        [Route("~/my-customers")] ///
        public ActionResult MyCustomersList(KendoGridRequest request = null)
        {
            if (Request.HttpMethod.ToUpper() == "GET") return View();

            var customerID = Identity.Current.CustomerID;
            var preferredCustomerTypes = new List<int> { CustomerTypes.RetailCustomer, CustomerTypes.PreferredCustomer };

            using (var context = new KendoGridDataContext(ExigoDAL.Sql()))
            {
                return context.Query<PreferredCustomerViewModel>(request, @"
                        SELECT 
	                          c.CustomerID
	                        , c.FirstName
	                        , c.LastName
	                        , CAST(CreatedDate as date) AS 'CreatedDate'
	                        , c.Email
	                        , c.Phone
                            , c.CustomerTypeID
                        FROM
                            EnrollerDownline ud
                            INNER JOIN Customers c 
	                            ON c.CustomerID = ud.CustomerID
                                AND c.CustomerTypeID IN @customertypes
                        WHERE 
                            ud.DownlineCustomerID = @customerid
                            AND ud.Level = 1
                        ", new
                {
                    customertypes = preferredCustomerTypes,
                    customerid = customerID
                });
            }
        }

        [Route("~/downline-orders")] ///
        public ActionResult DownlineOrdersList(KendoGridRequest request = null)
        {
            if (Request.HttpMethod.ToUpper() == "GET") return View();

            var maxDayRange = 90;
            var currentcustomerID = Identity.Current.CustomerID;
            // Fetch the data
            using (var context = new KendoGridDataContext(ExigoDAL.Sql()))
            {
                return context.Query<OrdersViewModel>(request, $@"
                    SELECT 
                          ud.CustomerID		                
                        , CurrencyCode = o.CurrencyCode
                        , CountryCode = o.Country
	                    , c.FirstName
	                    , c.LastName
		                , o.OrderID
		                , o.Total
		                , o.BusinessVolumeTotal
		                , OrderDate = CAST(o.OrderDate AS date)
                    FROM
	                    Orders o
	                    INNER JOIN {TreeToUse} ud
		                    ON ud.CustomerID = o.CustomerID
	                    INNER JOIN Customers c
		                    ON c.CustomerID = ud.CustomerID
                    WHERE
	                    ud.DownlineCustomerID = @downlinecustomerid
                        AND o.OrderDate > GETDATE() - @maxdayrange
                        AND o.OrderStatusID >= 7
                        AND ud.CustomerID <> @customerid
                ", new
                {
                    downlinecustomerid = Identity.Current.CustomerID,
                    maxdayrange = maxDayRange,
                    customerid = currentcustomerID
                });
            }
        }

        [Route("~/downline-autoorders")] ///
        public ActionResult DownlineAutoOrdersList(KendoGridRequest request = null)
        {
            if (Request.HttpMethod.ToUpper() == "GET") return View();


            // Fetch the data
            using (var context = new KendoGridDataContext(ExigoDAL.Sql()))
            {
                return context.Query<OrdersViewModel>(request, $@"
                    SELECT ud.CustomerID
	                    , c.FirstName
	                    , c.LastName          
                        , CurrencyCode = ao.CurrencyCode
                        , CountryCode = ao.Country
		                , ao.Total
		                , ao.BusinessVolumeTotal
		                , LastRunDate = CAST(ao.LastRunDate AS date)
		                , NextRunDate = CAST(ao.NextRunDate AS date)
                    FROM
	                    AutoOrders ao
	                    INNER JOIN {TreeToUse} ud
		                    ON ud.CustomerID = ao.CustomerID
	                    INNER JOIN Customers c
		                    ON c.CustomerID = ud.CustomerID    
                    WHERE
	                    ud.DownlineCustomerID = @downlinecustomerid
                        AND ao.CustomerID <> @downlinecustomerid
                        AND ao.AutoOrderStatusID = 0
                ", new
                {
                    downlinecustomerid = Identity.Current.CustomerID
                });
            }
        }

        [Route("~/upcoming-promotions")] ///
        public ActionResult UpcomingPromotionsList(KendoGridRequest request = null)
        {
            if (Request.HttpMethod.ToUpper() == "GET") return View();

            var currentperiodID = ExigoDAL.GetCurrentPeriod(PeriodTypes.Default).PeriodID;
            // Fetch the data
            using (var context = new KendoGridDataContext(ExigoDAL.Sql()))
            {
                return context.Query(request, $@"
                    SELECT
                  c.CustomerID
                  , c.FirstName
                  , c.LastName
                  , c.Company
                  , x.PaidRankID
                  , prsx.Score AS 'RankScore'
                  , r.RankDescription AS 'NextRankDescription'
                    FROM
                    (
                        select
                            ud.CustomerID
                            , min(prs.PaidRankID) as PaidRankID
                        from
                            {TreeToUse} ud
                        inner join PeriodRankScores prs
                            ON prs.PeriodTypeID = @periodtypeid
                            AND prs.PeriodID = @currentperiodid
                            AND prs.CustomerID = ud.CustomerID
                            AND prs.Score < 100
                        where
                            ud.DownlineCustomerID = @downlinecustomerid
                        group by
                            ud.CustomerID
                    ) x 
                    inner join Ranks r
                        on r.RankID = x.PaidRankID
                    inner join Customers c
                        on c.CustomerID = x.CustomerID
                    inner join PeriodRankScores prsx
                        on x.CustomerID = prsx.CustomerID
                        and prsx.PeriodTypeID = @periodtypeid
                        AND prsx.PeriodID = @currentperiodid
                        and prsx.PaidRankID = x.PaidRankID
                    WHERE
                        prsx.Score > 40
                    ",
                    new
                    {
                        downlinecustomerid = Identity.Current.CustomerID,
                        periodtypeid = PeriodTypes.Default,
                        currentperiodid = currentperiodID
                    }).Tokenize("CustomerID");
            }
        }

        [Route("~/recent-activity")]
        public ActionResult RecentActivityList()
        {
            var model = new RecentActivityListViewModel();

            try
            {
                // Get the customer's recent organization activity
                var RecentActivities = ExigoDAL.GetCustomerRecentActivity(new GetCustomerRecentActivityRequest
                {
                    CustomerID = Identity.Current.CustomerID,
                    Page = 1,
                    RowCount = 50
                }).ToList();

                if (RecentActivities == null)
                {
                    ViewBag.ActivityDefault = "No Recent Activity";
                }

                model.RecentActivities.AddRange(RecentActivities);

            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;
            }

            return View(model);
        }

        [Route("~/downline-ranks")] ///
        public ActionResult DownlineRanksList(KendoGridRequest request = null, int rankid = 0)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                var viewModel = new List<DownlineRankCountViewModel>();
                using (var context = ExigoDAL.Sql())
                {
                    context.Open();
                    viewModel = context.Query<DownlineRankCountViewModel, Rank, DownlineRankCountViewModel>($@"
                        SELECT
                            COUNT(c.RankID) AS 'Total'
	                        ,c.RankID
	                        ,r.RankDescription
	                        
                        FROM {TreeToUse} d
	                        INNER JOIN Customers c
		                        ON c.CustomerID = d.CustomerID
	                        INNER JOIN Ranks r
		                        ON r.RankID = c.RankID
                        WHERE 
	                        d.DownlineCustomerID = @topcustomerid
                            AND c.CustomerTypeID IN @customertypes	
                        GROUP BY
	                        c.RankID,
	                        r.RankDescription
                        ORDER BY
                            c.RankID
                        ", (model, rank) =>
                         {
                             model.Rank = rank;
                             return model;
                         }, new
                         {
                             topcustomerid = Identity.Current.CustomerID,
                             customertypes = new List<int> { CustomerTypes.Distributor }
                         }, splitOn: "RankID"
                         ).ToList();
                    context.Close();
                }

                return View(viewModel);
            }

            // Logic that will run if a user has chosen a specific rank to view in the report
            var whereRank = "IS NOT NULL";
            if (rankid > 0)
            {
                whereRank = "= " + rankid;
            }

            // Get the data
            using (var ctx = new KendoGridDataContext(ExigoDAL.Sql()))
            {
                return ctx.Query<DownlineRankViewModel>(request, @"
                    SELECT
	                    c.CustomerID
                        ,c.FirstName
                        ,c.LastName
	                    ,c.RankID
	                    ,COALESCE(r.RankDescription, '---') AS 'RankDescription'
	                    ,CAST(c.CreatedDate AS date) AS 'CreatedDate'
                    FROM UnilevelDownline d
	                    LEFT JOIN Customers c
		                    ON c.CustomerID = d.CustomerID
	                    LEFT JOIN Ranks r
		                    ON r.RankID = c.RankID
                    WHERE 
	                    d.DownlineCustomerID = @downlinecustomerid
                        AND c.RankID " + whereRank +
                        " AND c.CustomerTypeID IN @customertypes", new
                        {
                            downlinecustomerid = Identity.Current.CustomerID,
                            customertypes = new List<int> { CustomerTypes.Distributor }
                        });
            }
        }

        [HttpPost]
        public JsonNetResult PlaceWaitingRoomCustomer(int customerid, int sponsorid)
        {
            try
            {
                ExigoDAL.PlaceUniLevelCustomer(new PlaceUniLevelCustomerRequest
                {
                    CustomerID = customerid,
                    ToSponsorID = sponsorid,
                    Reason = "Waiting room placement"
                });

                return new JsonNetResult(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                return new JsonNetResult(new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }

        [Route("~/new-distributors")] ///
        public ActionResult NewDistributorsList(KendoGridRequest request = null)
        {
            if (Request.HttpMethod.ToUpper() == "GET") return View();

            var topCustomerID = Identity.Current.CustomerID;
            // Fetch the data
            using (var context = new KendoGridDataContext(ExigoDAL.Sql()))
            {

                var days = GlobalSettings.Backoffices.Reports.NewestDistributors.Days;
                var customerTypes = GlobalSettings.Backoffices.Reports.NewestDistributors.CustomerTypes;
                var customerStatuses = GlobalSettings.Backoffices.Reports.NewestDistributors.CustomerStatuses;

                return context.Query(request, $@"
                    SELECT 
                          c.CustomerID
                        , c.FirstName
                        , c.LastName
                        , c.CreatedDate  
                    FROM {TreeToUse} d
                        INNER JOIN Customers c
                            ON c.CustomerID = d.CustomerID
                    WHERE d.DownlineCustomerID = @topcustomerid
                        AND d.Level > 0
                        AND c.CustomerTypeID IN @CustomerTypes
                        AND c.CustomerStatusID IN @CustomerStatuses
                        AND c.CreatedDate >= CASE 
                                        WHEN @days > 0 
                                        THEN getdate()-@Days 
                                        ELSE c.CreatedDate 
                                        END", new
                 {
                    topcustomerid = topCustomerID,
                    CustomerTypes = customerTypes,
                    CustomerStatuses = customerStatuses,
                    Days = days
                }).Tokenize("CustomerID");
            }
        }

        [Route("~/organization/viewer")]
        public ActionResult OrganizationViewer()
        {
            return View();
        }
        #endregion

        #region Models and Enums

        public class SearchResult
        {
            public int CustomerID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Token { get { return Security.Encrypt(CustomerID); } }
        }

        #endregion
    }
}
