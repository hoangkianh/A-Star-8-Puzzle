/*
 * Created by SharpDevelop.
 * User: HKA_250
 * Date: 26/08/2012
 * Time: 12:08 | HKA
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace A_Star_8_Puzzle
{
	public partial class MainForm : Form
    {
        #region Thuộc tính

        private const string FINISH = "123804765";  // trạng thái kết thúc
        private const int SPEED = 800; // tốc độ dịch chuyển của ô số
        private List<Status_Node> OpenList;     // danh sách chứa các trạng thái được sinh ra
        private List<Status_Node> CloseList;    // danh sách chứa các trạng thái tìm được
        private List<Status_Node> TraceList;        // danh sách truy vết
        private int StepCount;          // số bước đi
        private Status_Node CurNode;    // node hiện tại
        private Status_Node Game;       // đề bài 
        private bool first = true; // cờ đánh dấu node là vị trí đầu tiên
        private bool reset = false; // reset lại ma trận Puzzle

        #endregion

        #region Các phương thức

        /// <summary>
        /// tính toán xem trạng thái có thể trở về trạng thái đích được không
        /// </summary>
        private bool IsCanSolve(Status_Node n)
        {
            int length = n.Code.Length;
            int value = 0;
            int[] k = new int[length];

            for (int i = 0; i < length; i++)
                int.TryParse(n.Code[i].ToString(), out k[i]);

            for (int i = 0; i < length; i++)
            {
                int t = k[i];
                if (t > 0)
                {
                    for (int j = i + 1; j < length; j++)
                    {
                        if (k[j] < t && k[j] > 0)
                        {
                            value++;
                        }
                    }
                }
            }

            if (value % 2 != 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// thêm 1 trạng thái mới vào trong list open (chứa các trạng thái con được sinh ra)
        /// sao cho danh sách được sắp xếp theo thứ tự tăng dần giá trị F
        /// - biến vào là node n
        /// </summary>
        private void AddNodeToOpenList(Status_Node n)
        {
            int i = 1;

            if (OpenList.Count == 0) // nếu list rỗng
            {
                OpenList.Add(n);
                return;
            }

            // nếu list không rỗng, tìm kiếm xem đã có chưa
            bool found = false;
            bool canadd = false;

            for (i = 0; i < OpenList.Count; i++)
            {
                // tìm thấy
                if (n.Code.Equals(OpenList[i].Code))
                {
                    found = true;
                    if (n.G < OpenList[i].G) // so sánh giá trị G, lấy Node nhỏ hơn
                    {
                        canadd = true;
                        OpenList.RemoveAt(i); // xóa phần tử thử i
                    }
                    return;
                }
            }

            // không tìm thấy hoặc có thể thêm được
            if (!found || canadd)
            {
                // duyệt list và chèn theo thứ tự tăng dần của F
                for (i = 0; i < OpenList.Count; i++)
                {
                    if (n.F < OpenList[i].F)
                    {
                        break;
                    }
                }

                if (i == OpenList.Count)
                    OpenList.Add(n);
                else
                    OpenList.Insert(i, n);
            }
        }

        /// <summary>
        /// Lấy 1 trạng thái từ List OPen chuyển sang list Close
        /// do list đã sắp xếp theo thứ tự tăng dần của F nên lấy phần tử đầu tiên (OpenList[0])
        /// - trả về node lấy được
        /// </summary>
        private Status_Node GetNodeFromOpenList()
        {
            if (OpenList.Count > 0)
            {
                Status_Node n = OpenList[0];

                OpenList.RemoveAt(0); // xóa node đầu tiên
                CloseList.Add(n); // chuyển sang CloseList
                return n;
            }
            return null;
        }

        /// <summary>
        /// Kiểm tra sự tồn tại của 1 node trong list Close
        /// nếu có -> return true, ngược lại là false
        /// </summary>
        private bool IsInCloseList(Status_Node n)
        {
            for (int i = 0; i < CloseList.Count; i++)
            {
                if (CloseList[i].Code.Equals(n.Code))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Hàm đánh giá
        /// Trả về giá trị H là số ô sai vị trí của trạng thái hiện tại so với kết quả cuối cùng
        /// </summary>
        private int CalculateH(string current)
        {
            int count = 0;
            for (int i = 0; i < current.Length; i++)
            {
                if (!current[i].Equals(FINISH[i]))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Tính toán các trạng thái con của node n
        /// </summary>
        private void ExpandNode(Status_Node n)
        {
            int index = n.Code.IndexOf('0'); // tìm vị trí '0' trong chuỗi

            switch (index)
            {
                case 0: // nếu ô 0 đang ở vị trí của ô số 1 thì có thể sang phải hoặc xuống dưới
                    {
                        // trạng thái con 1 - sang phải
                        char[] child1 = n.Code.ToCharArray();
                        // sang phải. VD: 083124765 chuyển thành 803124765
                        child1[0] = child1[1];
                        child1[1] = '0';
                        string map = new string(child1);

                        // nếu node n không có cha hoặc node con sinh ra khác n
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            // sinh node con 
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            // nếu node con chưa có trong list close                            
                            if (!IsInCloseList(child))
                            {
                                AddNodeToOpenList(child); // chèn vào list open
                            }
                        }

                        // xuống dưới
                        char[] child2 = n.Code.ToCharArray();
                        child2[0] = child2[3];
                        child2[3] = '0';
                        map = new string(child2);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                            {
                                AddNodeToOpenList(child);
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        // sang trái
                        char[] child1 = n.Code.ToCharArray();
                        child1[1] = child1[0];
                        child1[0] = '0';
                        string map = new string(child1);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }

                        // sang phải
                        char[] child2 = n.Code.ToCharArray();
                        child2[1] = child2[2];
                        child2[2] = '0';
                        map = new string(child2);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }

                        // xuống dưới
                        char[] child3 = n.Code.ToCharArray();
                        child3[1] = child3[4];
                        child3[4] = '0';
                        map = new string(child3);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }
                        break;
                    }
                case 2:
                    {
                        // sang trái
                        char[] child1 = n.Code.ToCharArray();
                        child1[2] = child1[1];
                        child1[1] = '0';
                        string map = new string(child1);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }

                        // xuống dưới
                        char[] child2 = n.Code.ToCharArray();
                        child2[2] = child2[5];
                        child2[5] = '0';
                        map = new string(child2);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }
                        break;
                    }
                case 3:
                    {
                        // lên trên
                        char[] child1 = n.Code.ToCharArray();
                        child1[3] = child1[0];
                        child1[0] = '0';
                        string map = new string(child1);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }

                        // sang phải
                        char[] child2 = n.Code.ToCharArray();
                        child2[3] = child2[4];
                        child2[4] = '0';
                        map = new string(child2);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }

                        // xuống dưới
                        char[] child3 = n.Code.ToCharArray();
                        child3[3] = child3[6];
                        child3[6] = '0';
                        map = new string(child3);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }
                        break;
                    }
                case 4:
                    {
                        // lên trên
                        char[] child1 = n.Code.ToCharArray();
                        child1[4] = child1[1];
                        child1[1] = '0';
                        string map = new string(child1);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }

                        // sang trái
                        char[] child2 = n.Code.ToCharArray();
                        child2[4] = child2[3];
                        child2[3] = '0';
                        map = new string(child2);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }

                        // sang phải
                        char[] child3 = n.Code.ToCharArray();
                        child3[4] = child3[5];
                        child3[5] = '0';
                        map = new string(child3);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }

                        // xuống dưới
                        char[] child4 = n.Code.ToCharArray();
                        child4[4] = child4[7];
                        child4[7] = '0';
                        map = new string(child4);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }
                        break;
                    }
                case 5:
                    {
                        // lên trên
                        char[] child1 = n.Code.ToCharArray();
                        child1[5] = child1[2];
                        child1[2] = '0';
                        string map = new string(child1);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }

                        // sang trái
                        char[] child2 = n.Code.ToCharArray();
                        child2[5] = child2[4];
                        child2[4] = '0';
                        map = new string(child2);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }

                        // xuống dưới
                        char[] child3 = n.Code.ToCharArray();
                        child3[5] = child3[8];
                        child3[8] = '0';
                        map = new string(child3);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }
                        break;
                    }
                case 6:
                    {
                        // lên trên
                        char[] child1 = n.Code.ToCharArray();
                        child1[6] = child1[3];
                        child1[3] = '0';
                        string map = new string(child1);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }

                        // sang phải
                        char[] child2 = n.Code.ToCharArray();
                        child2[6] = child2[7];
                        child2[7] = '0';
                        map = new string(child2);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }
                        break;
                    }
                case 7:
                    {
                        // lên trên
                        char[] child1 = n.Code.ToCharArray();
                        child1[7] = child1[4];
                        child1[4] = '0';
                        string map = new string(child1);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }

                        // sang trái
                        char[] child2 = n.Code.ToCharArray();
                        child2[7] = child2[6];
                        child2[6] = '0';
                        map = new string(child2);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }

                        // sang phải
                        char[] child3 = n.Code.ToCharArray();
                        child3[7] = child3[8];
                        child3[8] = '0';
                        map = new string(child3);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }
                        break;
                    }
                case 8:
                    {
                        // lên trên
                        char[] child1 = n.Code.ToCharArray();
                        child1[8] = child1[5];
                        child1[5] = '0';
                        string map = new string(child1);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }

                        // sang trái
                        char[] child2 = n.Code.ToCharArray();
                        child2[8] = child2[7];
                        child2[7] = '0';
                        map = new string(child2);
                        if (n.Parent == null || !n.Code.Equals(map))
                        {
                            Status_Node child = new Status_Node(map, n, n.G + 1, CalculateH(map));
                            if (!IsInCloseList(child))
                                AddNodeToOpenList(child);
                        }
                        break;
                    }
            }
        }                

        #region Thuật toán A*
        // Thuật toán A*
        private Status_Node Start(string start, string end)
        {
            Stopwatch sw = new Stopwatch();

            // khởi tạo danh sách
            OpenList.Clear();
            CloseList.Clear();
            StepCount = 0;                        

            sw.Start();

            // node đầu tiên là ma trận đề bài
            Status_Node firstNode = new Status_Node(start, null, 0, CalculateH(start));

            AddNodeToOpenList(firstNode);

            Status_Node currentNode = null;
            // tính toán bước đi, với list có số ptu lớn quá 50000 -> không tìm thấy
            while (OpenList.Count > 0 && OpenList.Count < 50000)
            {
                currentNode = GetNodeFromOpenList(); // lấy 1 node đầu tiên trong list, chuyển vào close list

                // kiểm tra xem có phải trạng thái đích không?
                if (currentNode.Code.Equals(FINISH)) // nếu đúng thì trả về trạng thái này
                {                                       
                    sw.Stop();
                    lblFinish.Text = "Thời gian: " + sw.ElapsedMilliseconds.ToString() + " mili giây";
                    lblCountStatistic.Text = "Số trạng thái tìm được: " + OpenList.Count.ToString();
                    return currentNode;
                }

                ExpandNode(currentNode);// tìm các trạng thái con nếu không thấy
            }
            return null;
        }
        #endregion

        // dừng lại nếu tìm được
        private void Stop()
        {
            if (this.CurNode.Code.Equals(FINISH))
            {
                MessageBox.Show("Đã giải xong", "Hoàn thành", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (MessageBox.Show("Bạn muốn tiếp tục không?", "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    lblFinish.Text = "";
                    lblCountStatistic.Text = "";
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        // di chuyển các ô số
        private bool MoveStep(Direction dir)
        {
            int index = -1;
            if (this.CurNode != null)
                index = this.CurNode.Code.IndexOf("0"); // tìm vị trí ô trống

            switch (dir)
            {
                case Direction.LEFT:
                    {
                        if (index % 3 == 2) return false;

                        char[,] data = this.CurNode.Map;
                        char tmp = this.CurNode.Map[index / 3, index % 3 + 1];
                        data[index / 3, index % 3] = tmp;
                        data[index / 3, index % 3 + 1] = '0';
                        this.CurNode.Map = data;
                        break;
                    }
                case Direction.RIGHT:
                    {
                        if (index % 3 == 0) return false;

                        char[,] data = this.CurNode.Map;
                        char tmp = this.CurNode.Map[index / 3, index % 3 - 1];
                        data[index / 3, index % 3] = tmp;
                        data[index / 3, index % 3 - 1] = '0';
                        this.CurNode.Map = data;
                        break;
                    }
                case Direction.UP:
                    {
                        if (index / 3 == 2) return false;

                        char[,] data = this.CurNode.Map;
                        char tmp = this.CurNode.Map[index / 3 + 1, index % 3];
                        data[index / 3, index % 3] = tmp;
                        data[index / 3 + 1, index % 3] = '0';
                        this.CurNode.Map = data;
                        break;
                    }
                case Direction.DOWN:
                    {
                        if (index / 3 == 0) return false;

                        char[,] data = this.CurNode.Map;
                        char tmp = this.CurNode.Map[index / 3 - 1, index % 3];
                        data[index / 3, index % 3] = tmp;
                        data[index / 3 - 1, index % 3] = '0';
                        this.CurNode.Map = data;
                        break;
                    }
                default:
                    break;
            }
            return true;
        }

        #endregion

        public MainForm()
		{
			InitializeComponent();
            OpenList = new List<Status_Node>();
			CloseList = new List<Status_Node>();
            TraceList = new List<Status_Node>();
            this.timerPlay.Interval = SPEED;
		}

        private void PaintBackGround(Graphics g)
        {
            g.Clear(Color.LightCyan);
            Pen p = new Pen(Color.Cyan, 1.0f);

            // vẽ lưới
            g.DrawLine(p, 0f, 96f, 288f, 96f);
            g.DrawLine(p, 0f, 192f, 288f, 192f);
            g.DrawLine(p, 96f, 0f, 96f, 288f);
            g.DrawLine(p, 192f, 0f, 192f, 288f);
        }

        // vẽ 8 puzzle
        private void pbGame_Paint(object sender, PaintEventArgs e)
        {
            PaintBackGround(e.Graphics);
            if (this.CurNode != null)
                this.CurNode.Paint(e.Graphics);
        }

        // di chuyển các ô khi click chuột
        private void pbGame_MouseClick(object sender, MouseEventArgs e)
        {
            if(this.timerPlay.Enabled)
                return;
            Point p = new Point(e.Y / 96, e.X / 96); // lấy tọa độ chuột
            bool isok = false;

            if (!isok)
            {
                Point p1 = p;
                p1.Offset(-1, 0);
                if (p1.X >= 0 && p1.Y >= 0 && p1.X < 3 && p1.Y < 3)
                {
                    if (CurNode.Map[p1.X, p1.Y] == '0')
                    {
                        isok = MoveStep(Direction.UP);// đi lên
                    }
                }
            }
            if (!isok)
            {
                Point p2 = p;
                p2.Offset(1, 0);
                if (p2.X >= 0 && p2.Y >= 0 && p2.X < 3 && p2.Y < 3)
                {
                    if (CurNode.Map[p2.X, p2.Y] == '0')
                    {
                        isok = MoveStep(Direction.DOWN);// đi xuống
                    }
                }
            }
            if (!isok)
            {
                Point p3 = p;
                p3.Offset(0, -1);
                if (p3.X >= 0 && p3.Y >= 0 && p3.X < 3 && p3.Y < 3)
                {
                    if (CurNode.Map[p3.X, p3.Y] == '0')
                    {
                        isok = MoveStep(Direction.LEFT);// sang trái
                    }
                }
            }
            if (!isok)
            {
                Point p4 = p;
                p4.Offset(0, 1);
                if (p4.X >= 0 && p4.Y >= 0 && p4.X < 3 && p4.Y < 3)
                {
                    if (CurNode.Map[p4.X, p4.Y] == '0')
                    {
                        isok = MoveStep(Direction.RIGHT);// sang phải
                    }
                }
            }

            if (isok)
            {
                this.btnResolve.Text = "Giải";
                this.pbGame.Refresh();
                this.StepCount++;
                this.lblCount.Text = this.StepCount.ToString();
                Stop();
            }
        }

        private void bntNew_Click(object sender, EventArgs e)
        {
            this.timerPlay.Enabled = false;
            this.btnResolve.Enabled = true;
            // khởi tạo mặc định
            if (this.first)
            {
                this.CurNode = new Status_Node("876042531", null, 0, CalculateH("876042531"));
                //this.CurNode = new Status_Node("684031275", null, 0, CalculateH("684031275"));
                this.first = false;
            }
            else
            {
                if (this.reset)
                {
                    this.CurNode = this.Game;
                    this.reset = false;
                }
                else
                {
                    // khởi tạo ngẫu nhiên
                    this.CurNode = new Status_Node(FINISH, null, 0, 0); // CurNode là trạng thái đích
                    Random rd = new Random();

                    for (int i = 0; i < 100; i++)
                    {
                        int j = rd.Next(1000) % 4;
                        MoveStep((Direction)j);// xáo trộn 8 puzzle theo 4 hướng
                    }
                }
            }

            this.Game = new Status_Node(this.CurNode.Code, null, 0, this.CurNode.H);// lưu lại trạng thái hiện tại cho nút reset
            this.btnResolve.Text = "Giải";
            this.StepCount = 0;
            this.lblCount.Text = this.StepCount.ToString();
            this.pbGame.Refresh();
        }

        private void btnResolve_Click(object sender, EventArgs e)
        {
            if (this.btnResolve.Text == "Giải" || this.btnResolve.Text == "Tiếp tục")
            {
                if (this.btnResolve.Text == "Giải")
                {
                    if (IsCanSolve(this.CurNode))
                    {
                        progressBar1.Style = ProgressBarStyle.Marquee;
                        progressBar1.MarqueeAnimationSpeed = 20;                       
                        Status_Node n = Start(this.CurNode.Code, FINISH);                        

                        if (n != null)// nếu giải được thì chèn vào mảng truy vết
                        {
                            TraceList.Clear();
                            while (n != null)
                            {
                                TraceList.Insert(0, n); // chèn vào TraceList
                                n = n.Parent; // truy vết
                            }

                            this.StepCount = 0;
                            MessageBox.Show("Số bước đi: " + (TraceList.Count - 1), "Số bước đi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            this.btnResolve.Enabled = true;
                            this.btnResolve.Text = "Dừng";
                            this.timerPlay.Enabled = true;
                        }
                        else
                        {
                            this.btnResolve.Enabled = false;
                            MessageBox.Show("Không tìm được lời giải, số trạng thái quá lớn");
                        }
                    }
                    else
                    {
                        this.btnResolve.Enabled = false;
                        MessageBox.Show("Không tìm được lời giải");
                        return;
                    }
                }
                else
                {
                    this.timerPlay.Enabled = true;
                    this.btnResolve.Text = "Tiếp tục";
                }
            }
            else
            {
                this.btnResolve.Text = "Tiếp tục";
                this.timerPlay.Enabled = false;
            }
        }

        // reset
        private void btnReset_Click(object sender, EventArgs e)
        {
            this.reset = true;
            bntNew_Click(null, null);
        }

        // truy vết, hiển thị bước đi
        private void timerPlay_Tick(object sender, EventArgs e)
        {
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.Maximum = this.TraceList.Count;
            progressBar1.Minimum = 0;
            progressBar1.Step = 1;

            if (this.StepCount < this.TraceList.Count)
            {
                this.CurNode = this.TraceList[this.StepCount];
                pbGame.Refresh();

                this.lblCount.Text = "" + this.StepCount++;
                progressBar1.PerformStep();

                if (this.StepCount == this.TraceList.Count)
                {
                    this.timerPlay.Enabled = false;
                    lblFinish.Text = "ĐÃ GIẢI XONG";
                    MessageBox.Show("Đã giải xong", "Hoàn thành", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (MessageBox.Show("Bạn muốn tiếp tục không?", "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        lblFinish.Text = "";
                        lblCountStatistic.Text = "";
                        progressBar1.Value = 0;
                        bntNew_Click(null, null);
                    }
                    else
                    {
                        Application.Exit();
                    }
                    GC.Collect();
                    this.btnResolve.Text = "Giải";
                }
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            bntNew_Click(null, null);
        }        
	}
}