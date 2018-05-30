using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;

namespace EFCoreLoadParentWithChildrenExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var db = new Database(Database.defaultConnectionString))
                {
                    db.Database.EnsureCreated();
                    //var nIremove = db.navItems.ToList();
                    //foreach(var item in nIremove)
                    //{
                    //    db.Remove(item);
                    //}
                    //db.SaveChanges();

                    var nI = db.navItems.Where(p => p.parentId == null).ToList();
                    if (nI.Count == 0)
                    {
                        Console.WriteLine("Creating default data...");
                        db.navItems.AddRange(defaultNav());
                        db.SaveChanges();
                        nI = db.navItems.Where(p => p.parentId == null).ToList();   // Try again
                    }
                    Console.WriteLine($"1: Loaded {nI.Count} NavItems without Include [should be 16]");
                    showTree(nI);
                    Console.WriteLine();
                }
                using (var db2 = new Database(Database.defaultConnectionString))
                {

                    var nI = db2.navItems.Where(p => p.parentId == null).Include(p => p.children).ToList();
                    Console.WriteLine($"2: Loaded {nI.Count} NavItems with Include [should be 16]");
                    showTree(nI);
                    Console.WriteLine();

                }
                using (var db3 = new Database(Database.defaultConnectionString))
                {

                    var nI = db3.navItems.ToList();
                    Console.WriteLine($"3.1: Loaded {nI.Count} NavItems... [should be 43]");

                    nI = nI.Where(p => p.parentId == null).ToList();    // Remove child items from the root of the collection
                    Console.WriteLine($"3.2: Filtered to  {nI.Count} NavItems... [should be 16]");
                    showTree(nI);
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached)
                    Debug.WriteLine(ex.ToString());
            }
            Console.WriteLine("Press Enter to quit.");
            Console.ReadLine();
        }

        private static void showTree(List<NavItem> items, int level=0)
        {
            var pad = new String(' ', level * 4);
            foreach (NavItem item in items)
            {
                
                Console.WriteLine($"{pad}Item {item.navItemId} has {item.children?.Count.ToString() ?? "[null]"} children.");
                if (item.children != null)
                    showTree(item.children.ToList(), level + 1);
            }

        }

        private static List<NavItem> defaultNav()
        {
            // Create default navigation
            var nav = new List<NavItem>();
            List<NavItem> subNav;

            nav.Add(new NavItem() { sortOrder = 1, name = "Dashboard", url = "/dashboard", icon = "icon-speedometer", badgeVariant = "info", badgeText = "new" });
            nav.Add(new NavItem() { sortOrder = 2, isTitle = true, name = "Theme" });
            nav.Add(new NavItem() { sortOrder = 3, name = "Colours", url = "/theme/colors", icon = "icon-drop" });
            nav.Add(new NavItem() { sortOrder = 4, name = "Typeography", url = "/theme/typography", icon = "icon-pencil" });
            nav.Add(new NavItem() { sortOrder = 5, isTitle = true, name = "Components" });

            subNav = new List<NavItem>();
            subNav.Add(new NavItem() { sortOrder = 1, name = "Cards", url = "/base/cards", icon = "icon-puzzle" });
            subNav.Add(new NavItem() { sortOrder = 2, name = "Carousels", url = "/base/carousels", icon = "icon-puzzle" });
            subNav.Add(new NavItem() { sortOrder = 3, name = "Collapses", url = "/base/collapses", icon = "icon-puzzle" });
            subNav.Add(new NavItem() { sortOrder = 4, name = "Forms", url = "/base/forms", icon = "icon-puzzle" });
            subNav.Add(new NavItem() { sortOrder = 5, name = "Pagination", url = "/base/pagination", icon = "icon-puzzle" });
            subNav.Add(new NavItem() { sortOrder = 6, name = "Popovers", url = "/base/popovers", icon = "icon-puzzle" });
            subNav.Add(new NavItem() { sortOrder = 7, name = "Progress", url = "/base/progress", icon = "icon-puzzle" });
            subNav.Add(new NavItem() { sortOrder = 8, name = "Switches", url = "/base/switches", icon = "icon-puzzle" });
            subNav.Add(new NavItem() { sortOrder = 9, name = "Tables", url = "/base/tables", icon = "icon-puzzle" });
            subNav.Add(new NavItem() { sortOrder = 10, name = "Tabs", url = "/base/tabs", icon = "icon-puzzle" });
            subNav.Add(new NavItem() { sortOrder = 11, name = "Tooltips", url = "/base/tooltips", icon = "icon-puzzle" });
            nav.Add(new NavItem()
            {
                sortOrder = 6,
                name = "Base",
                url = "/base",
                icon = "icon-puzzle",
                children = subNav
            });

            subNav = new List<NavItem>();
            subNav.Add(new NavItem() { sortOrder = 1, name = "Buttons", url = "/buttons/buttons", icon = "icon-cursor" });
            subNav.Add(new NavItem() { sortOrder = 2, name = "Dropdowns", url = "/buttons/dropdowns", icon = "icon-cursor" });
            subNav.Add(new NavItem() { sortOrder = 3, name = "Brand Buttons", url = "/buttons/brand-buttons", icon = "icon-cursor" });
            nav.Add(new NavItem()
            {
                sortOrder = 7,
                name = "Buttons",
                url = "/buttons",
                icon = "icon-cursor",
                children = subNav
            });

            nav.Add(new NavItem() { sortOrder = 8, name = "Charts", url = "/charts", icon = "icon-pie-chart" });

            subNav = new List<NavItem>();
            subNav.Add(new NavItem() { sortOrder = 1, name = "CoreUI Icons", url = "/icons/coreui-icons", icon = "icon-star", badgeVariant = "success", badgeText = "NEW" });
            subNav.Add(new NavItem() { sortOrder = 2, name = "Flags", url = "/icons/flags", icon = "icon-star" });
            subNav.Add(new NavItem() { sortOrder = 3, name = "Font Awesome", url = "/icons/font-awesome", icon = "icon-star", badgeVariant = "secondary", badgeText = "4.7" });
            subNav.Add(new NavItem() { sortOrder = 4, name = "Simple Line Icons", url = "/icons/simple-line-icons", icon = "icon-star" });
            nav.Add(new NavItem()
            {
                sortOrder = 9,
                name = "Icons",
                url = "/icons",
                icon = "icon-star",
                children = subNav
            });

            subNav = new List<NavItem>();
            subNav.Add(new NavItem() { sortOrder = 1, name = "Alerts", url = "/notifications/alerts", icon = "icon-bell" });
            subNav.Add(new NavItem() { sortOrder = 2, name = "Badges", url = "/notifications/badges", icon = "icon-bell" });
            subNav.Add(new NavItem() { sortOrder = 3, name = "Modals", url = "/notifications/modals", icon = "icon-bell" });
            nav.Add(new NavItem()
            {
                sortOrder = 10,
                name = "Notifications",
                url = "/notifications",
                icon = "icon-bell",
                children = subNav
            });

            nav.Add(new NavItem() { sortOrder = 11, name = "Widgets", url = "/widgets", icon = "icon-calculator", badgeVariant = "info", badgeText = "NEW" });
            nav.Add(new NavItem() { sortOrder = 12, isDivider = true });

            subNav = new List<NavItem>();
            subNav.Add(new NavItem() { sortOrder = 1, name = "Login", url = "/login", icon = "icon-star" });
            subNav.Add(new NavItem() { sortOrder = 2, name = "Register", url = "/register", icon = "icon-star" });
            subNav.Add(new NavItem() { sortOrder = 3, name = "Error 404", url = "/404", icon = "icon-star" });
            subNav.Add(new NavItem() { sortOrder = 4, name = "Error 500", url = "/500", icon = "icon-star" });
            nav.Add(new NavItem()
            {
                sortOrder = 13,
                name = "Pages",
                url = "/pages",
                icon = "icon-star",
                children = subNav
            });

            nav.Add(new NavItem() { sortOrder = 14, name = "Download CoreUI", url = "http://coreui.io/angular", icon = "icon-cloud-download", cssClass = "mt-auto", variant = "success" });
            nav.Add(new NavItem() { sortOrder = 15, name = "Try CoreUI PRO", url = "http://coreui.io/pro/angular", icon = "icon-layers", variant = "danger" });

            var subSubNav = new List<NavItem>();
            subSubNav.Add(new NavItem() { sortOrder = 1, name = "Child-of-Child" });
            subNav = new List<NavItem>();
            subNav.Add(new NavItem() { sortOrder = 1, name = "Child", children = subSubNav });
            nav.Add(new NavItem() { sortOrder = 16, name = "Parent", children = subNav });

            return nav;
        }

    }

    public class NavItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int navItemId { get; set; }
        public int sortOrder { get; set; }
        public bool isTitle { get; set; }
        public bool isDivider { get; set; }
        public string cssClass { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string icon { get; set; }
        public string variant { get; set; }
        public string badgeText { get; set; }
        public string badgeVariant { get; set; }
        public int? parentId { get; set; }
        [ForeignKey("parentId")]
        public virtual NavItem parent { get; set; }
        public virtual ICollection<NavItem> children { get; set; }
    }

    public class Database : DbContext
    {
        public DbSet<NavItem> navItems { get; private set; }

        public static string defaultConnectionString = $"FileName={Directory.GetCurrentDirectory()}\\mydb.db";

        private static DbContextOptions GetOptions(string connectionString)
        {
            var modelBuilder = new DbContextOptionsBuilder();
            return modelBuilder.UseSqlite(connectionString).Options;
        }

        #region Constructors
        public Database(string connectionString) : base(GetOptions(connectionString))
        {

        }
        public Database(DbContextOptions options) : base(options)
        {

        }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //configuration goes here.                
            base.OnModelCreating(modelBuilder);
        }

    }

}
