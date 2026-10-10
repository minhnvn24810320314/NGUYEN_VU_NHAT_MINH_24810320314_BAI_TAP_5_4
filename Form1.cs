using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NGUYEN_VU_NHAT_MINH_24810320314_BAI_TAP_5_4
{
    public partial class Form1 : Form
    {
        // UI controls created in code
        private SplitContainer splitContainer;
        private TreeView tvDepartments;
        private ListView lsvEmployees;
        private ComboBox cboViewMode;
        private ImageList imgTreeIcons;
        private ImageList imgLargeIcons;
        private ImageList imgSmallIcons;

        // In-memory mock data
        private List<Employee> employees = new List<Employee>();

        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            InitUI();
            LoadEmployeeData();
            LoadTreeViewData();

            // Select root node and trigger display
            if (tvDepartments.Nodes.Count > 0)
            {
                tvDepartments.SelectedNode = tvDepartments.Nodes[0];
            }
        }

        private void InitUI()
        {
            // SplitContainer
            splitContainer = new SplitContainer();
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Orientation = Orientation.Vertical;
            splitContainer.SplitterDistance = 250;
            Controls.Add(splitContainer);

            // ImageLists (generate simple colored icons programmatically)
            imgTreeIcons = new ImageList();
            imgTreeIcons.ImageSize = new Size(16, 16);
            imgTreeIcons.ColorDepth = ColorDepth.Depth32Bit;
            imgTreeIcons.Images.Add(CreateIcon(Color.DarkBlue)); // company
            imgTreeIcons.Images.Add(CreateIcon(Color.Green)); // department
            imgTreeIcons.Images.Add(CreateIcon(Color.Orange)); // group

            imgLargeIcons = new ImageList();
            imgLargeIcons.ImageSize = new Size(64, 64);
            imgLargeIcons.Images.Add(CreateIcon(Color.Purple, 64));
            imgLargeIcons.Images.Add(CreateIcon(Color.Teal, 64));
            imgLargeIcons.Images.Add(CreateIcon(Color.Brown, 64));

            imgSmallIcons = new ImageList();
            imgSmallIcons.ImageSize = new Size(16, 16);
            imgSmallIcons.Images.Add(CreateIcon(Color.Purple));
            imgSmallIcons.Images.Add(CreateIcon(Color.Teal));
            imgSmallIcons.Images.Add(CreateIcon(Color.Brown));

            // TreeView
            tvDepartments = new TreeView();
            tvDepartments.Dock = DockStyle.Fill;
            tvDepartments.ImageList = imgTreeIcons;
            tvDepartments.AfterSelect += TvDepartments_AfterSelect;
            splitContainer.Panel1.Controls.Add(tvDepartments);

            // Right panel: top ComboBox, rest ListView
            Panel rightTop = new Panel();
            rightTop.Dock = DockStyle.Top;
            rightTop.Height = 30;
            splitContainer.Panel2.Controls.Add(rightTop);

            cboViewMode = new ComboBox();
            cboViewMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cboViewMode.Items.AddRange(new object[] { "Details", "LargeIcon", "SmallIcon", "Tile", "List" });
            cboViewMode.SelectedIndex = 0; // Details
            cboViewMode.SelectedIndexChanged += CboViewMode_SelectedIndexChanged;
            cboViewMode.Dock = DockStyle.Left;
            cboViewMode.Width = 150;
            rightTop.Controls.Add(cboViewMode);

            // ListView
            lsvEmployees = new ListView();
            lsvEmployees.Dock = DockStyle.Fill;
            lsvEmployees.View = View.Details;
            lsvEmployees.FullRowSelect = true;
            lsvEmployees.GridLines = true;
            lsvEmployees.SmallImageList = imgSmallIcons;
            lsvEmployees.LargeImageList = imgLargeIcons;

            // Columns: Mã NV (image), Họ Tên, Chức vụ, Ngày vào làm
            lsvEmployees.Columns.Add("Mã NV", 100, HorizontalAlignment.Left);
            lsvEmployees.Columns.Add("Họ Tên", 200, HorizontalAlignment.Left);
            lsvEmployees.Columns.Add("Chức vụ", 150, HorizontalAlignment.Left);
            lsvEmployees.Columns.Add("Ngày vào làm", 120, HorizontalAlignment.Left);

            splitContainer.Panel2.Controls.Add(lsvEmployees);
        }

        private void LoadTreeViewData()
        {
            tvDepartments.BeginUpdate();
            tvDepartments.Nodes.Clear();

            // Company
            var root = new TreeNode("Công ty ABC");
            root.ImageIndex = 0;
            root.SelectedImageIndex = 0;
            root.Name = "company_abc";
            root.Tag = new NodeTag { Id = "company_abc", Type = NodeType.Company };

            // Phòng Nhân sự
            var hr = new TreeNode("Phòng Nhân sự");
            hr.ImageIndex = 1;
            hr.SelectedImageIndex = 1;
            hr.Name = "dept_hr";
            hr.Tag = new NodeTag { Id = "dept_hr", Type = NodeType.Department };

            var hrRecruit = new TreeNode("Nhóm Tuyển dụng");
            hrRecruit.ImageIndex = 2;
            hrRecruit.SelectedImageIndex = 2;
            hrRecruit.Name = "group_hr_recruit";
            hrRecruit.Tag = new NodeTag { Id = "group_hr_recruit", Type = NodeType.Group };

            var hrTraining = new TreeNode("Nhóm Đào tạo");
            hrTraining.ImageIndex = 2;
            hrTraining.SelectedImageIndex = 2;
            hrTraining.Name = "group_hr_training";
            hrTraining.Tag = new NodeTag { Id = "group_hr_training", Type = NodeType.Group };

            hr.Nodes.Add(hrRecruit);
            hr.Nodes.Add(hrTraining);

            // Phòng Kỹ thuật
            var eng = new TreeNode("Phòng Kỹ thuật");
            eng.ImageIndex = 1;
            eng.SelectedImageIndex = 1;
            eng.Name = "dept_eng";
            eng.Tag = new NodeTag { Id = "dept_eng", Type = NodeType.Department };

            var engDev = new TreeNode("Nhóm Lập trình");
            engDev.ImageIndex = 2;
            engDev.SelectedImageIndex = 2;
            engDev.Name = "group_eng_dev";
            engDev.Tag = new NodeTag { Id = "group_eng_dev", Type = NodeType.Group };

            var engQA = new TreeNode("Nhóm QA");
            engQA.ImageIndex = 2;
            engQA.SelectedImageIndex = 2;
            engQA.Name = "group_eng_qa";
            engQA.Tag = new NodeTag { Id = "group_eng_qa", Type = NodeType.Group };

            eng.Nodes.Add(engDev);
            eng.Nodes.Add(engQA);

            root.Nodes.Add(hr);
            root.Nodes.Add(eng);

            tvDepartments.Nodes.Add(root);
            root.Expand();
            tvDepartments.EndUpdate();
        }

        private void LoadEmployeeData()
        {
            employees.Clear();

            // HR - Recruit
            employees.Add(new Employee("NV001", "Nguyễn Văn A", "Chuyên viên", new DateTime(2020, 1, 15), "group_hr_recruit"));
            employees.Add(new Employee("NV002", "Trần Thị B", "Chuyên viên", new DateTime(2021, 3, 5), "group_hr_recruit"));

            // HR - Training
            employees.Add(new Employee("NV003", "Lê Văn C", "Trưởng nhóm", new DateTime(2019, 7, 1), "group_hr_training"));

            // Engineering - Dev
            employees.Add(new Employee("NV004", "Phạm Thị D", "Lập trình viên", new DateTime(2018, 11, 20), "group_eng_dev"));
            employees.Add(new Employee("NV005", "Hoàng Văn E", "Lập trình viên", new DateTime(2022, 6, 10), "group_eng_dev"));

            // Engineering - QA
            employees.Add(new Employee("NV006", "Vũ Thị F", "Kỹ sư QA", new DateTime(2020, 9, 30), "group_eng_qa"));
        }

        private void TvDepartments_AfterSelect(object? sender, TreeViewEventArgs e)
        {
            var node = e.Node;
            if (node == null) return;

            // collect node ids (self + children) to filter employees
            var ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            CollectNodeIds(node, ids);

            var filtered = employees.Where(emp => ids.Contains(emp.DepartmentId)).ToList();

            // If selected a department node, include employees of its child group nodes as well
            // (CollectNodeIds already includes descendants)

            DisplayEmployees(filtered);
        }

        private void CollectNodeIds(TreeNode node, HashSet<string> ids)
        {
            if (node.Tag is NodeTag tag)
            {
                ids.Add(tag.Id);
            }

            foreach (TreeNode child in node.Nodes)
            {
                CollectNodeIds(child, ids);
            }
        }

        private void DisplayEmployees(List<Employee> list)
        {
            lsvEmployees.BeginUpdate();
            lsvEmployees.Items.Clear();

            foreach (var emp in list)
            {
                var item = new ListViewItem(emp.Id);
                item.ImageIndex = 0; // use first image for all
                item.SubItems.Add(emp.FullName);
                item.SubItems.Add(emp.Position);
                item.SubItems.Add(emp.HireDate.ToString("yyyy-MM-dd"));
                lsvEmployees.Items.Add(item);
            }

            lsvEmployees.EndUpdate();
        }

        private void CboViewMode_SelectedIndexChanged(object? sender, EventArgs e)
        {
            switch (cboViewMode.SelectedItem?.ToString())
            {
                case "Details":
                    lsvEmployees.View = View.Details;
                    break;
                case "LargeIcon":
                    lsvEmployees.View = View.LargeIcon;
                    break;
                case "SmallIcon":
                    lsvEmployees.View = View.SmallIcon;
                    break;
                case "Tile":
                    lsvEmployees.View = View.Tile;
                    break;
                case "List":
                    lsvEmployees.View = View.List;
                    break;
                default:
                    lsvEmployees.View = View.Details;
                    break;
            }
        }

        private Bitmap CreateIcon(Color color, int size = 16)
        {
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                using (var b = new SolidBrush(color))
                {
                    g.FillRectangle(b, 0, 0, size, size);
                }
                // draw a border
                using (var p = new Pen(Color.Black))
                {
                    g.DrawRectangle(p, 0, 0, size - 1, size - 1);
                }
            }
            return bmp;
        }

        // Simple supporting types
        private class Employee
        {
            public string Id { get; }
            public string FullName { get; }
            public string Position { get; }
            public DateTime HireDate { get; }
            public string DepartmentId { get; }

            public Employee(string id, string fullName, string position, DateTime hireDate, string departmentId)
            {
                Id = id;
                FullName = fullName;
                Position = position;
                HireDate = hireDate;
                DepartmentId = departmentId;
            }
        }

        private enum NodeType
        {
            Company,
            Department,
            Group
        }

        private class NodeTag
        {
            public string Id { get; set; } = string.Empty;
            public NodeType Type { get; set; }
        }
    }
}
