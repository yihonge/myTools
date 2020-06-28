using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using APlayer3Lib;
namespace myTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetStyles();
        }
        #region 减少闪烁
        private void SetStyles()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer, true);
            //强制分配样式重新应用到控件上
            UpdateStyles();
            AutoScaleMode = AutoScaleMode.None;
        }
        #endregion
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndlnsertAfter, int X, int Y, int cx, int cy, uint Flags);

        [DllImport("user32.dll", EntryPoint = "SetParent")]
        public static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);


        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_LBUTTONDBLCLK = 0x0203;
        private const int S_OK = 0x00000000;
        private bool IsFullScreen = false;
        readonly List<LocalTVLive> liveList = new List<LocalTVLive>();
        readonly List<string> nowList = new List<string>();
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadLocalTVProgramme();
        }
        /// 加载本地电视直播节目 tv.txt
        /// </summary>
        private void LoadLocalTVProgramme()
        {
            List<string> list = new List<string>();
            list.Clear();
            using (FileStream fs = new FileStream(Application.StartupPath + "\\tv.txt", FileMode.OpenOrCreate, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        string temp = sr.ReadLine();
                        if (string.IsNullOrWhiteSpace(temp))
                        {
                            continue;
                        }
                        list.Add(temp);
                    }
                }
            }
            var node = new TreeNode();
            foreach (string item in list)
            {
                if (item.Substring(0,1) != ";")
                {
                    string[] split = item.Split(',');
                    if (split[1] == "#genre#")
                    {
                        node = treeView1.Nodes.Add(split[0]);
                    }
                    else
                    {
                        LocalTVLive live = new LocalTVLive
                        {
                            Name = split[0],
                            Path = split[1]
                        };
                        liveList.Add(live);
                        node.Nodes.Add(live.Name);
                    }
                }
            }

            return;
        }
        
        

        private void button1_Click(object sender, EventArgs e)
        {
            //string file = "";
            //OpenFileDialog fileDialog = new OpenFileDialog();
            //fileDialog.Title = "请选择要发送的文件";
            //if (DialogResult.OK == fileDialog.ShowDialog())
            //{
            //    //将选择的文件的全路径赋值给文本框
            //    file = fileDialog.FileName;
            //}
            //if (file == "") return;
            //aPlayer.Open(file);

            axPlayer1.Open("http://210.22.242.108/live-cnc-cdn.ysp.cctv.cn/ysp/2000204603.m3u8");
            //声音最大
            axPlayer1.SetVolume(100);

            //设置循环播放
            axPlayer1.SetConfig(119, "1");
            //设置图片背景透明
            axPlayer1.SetConfig(608, "0");
        }

        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (treeView1.SelectedNode != null && treeView1.SelectedNode.Parent != null)//如果选中的节点大于等于0
                {
                    nowList.Clear();
                    toolStripDropDownButton1.DropDownItems.Clear();
                    var urls = liveList.Where(x => x.Name == treeView1.SelectedNode.Text).ToList();
                    foreach (var item in urls[0].Path.Split('#'))
                    {
                        nowList.Add(item);
                        var tempItemp = toolStripDropDownButton1.DropDownItems.Add(item);
                        
                    }
                    axPlayer1.Open(nowList[0]);
                    ToolStripMenuItem tempItem = (ToolStripMenuItem)toolStripDropDownButton1.DropDownItems[0];
                    tempItem.Checked = true;

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void CheckSelected(ToolStripDropDownButton button, ToolStripItem selectedItem)
        {
            foreach (ToolStripMenuItem item in button.DropDownItems)
            {
                item.Checked = (item.Name == selectedItem.Name) ? true : false;
            }
        }
        private void toolStripDropDownButton1_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem != null)
            {
                foreach (ToolStripMenuItem tempItemp in toolStripDropDownButton1.DropDownItems)
                {
                    if (tempItemp == e.ClickedItem)
                        tempItemp.Checked = true;
                    else
                        tempItemp.Checked = false;
                }
                axPlayer1.Open(e.ClickedItem.Text);
            }
            
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void axPlayer1_OnMessage(object sender, AxAPlayer3Lib._IPlayerEvents_OnMessageEvent e)
        {
            switch (e.nMessage)
            {
                case WM_LBUTTONDOWN:
                    //MessageBox.Show("点击");
                    break;
                case WM_LBUTTONDBLCLK:
                    {
                        //MessageBox.Show("点击");
                        if (!IsFullScreen)
                        {
                            axPlayer1.Dock = DockStyle.None;
                            axPlayer1.Left = 0;
                            axPlayer1.Top = 0;
                            axPlayer1.Width = Screen.PrimaryScreen.Bounds.Width;
                            axPlayer1.Height = Screen.PrimaryScreen.Bounds.Height;
                            SetParent(axPlayer1.Handle, IntPtr.Zero);
                            IsFullScreen = true;
                        }
                        else
                        {
                            axPlayer1.Dock = DockStyle.Fill;
                            axPlayer1.Left = 0;
                            axPlayer1.Top = 0;
                            axPlayer1.Width = splitContainer1.Panel1.Width;
                            axPlayer1.Height = splitContainer1.Panel1.Height;
                            SetParent(axPlayer1.Handle, splitContainer1.Panel1.Handle);
                            IsFullScreen = false;
                        }


                    }
                    break;
            }
        }
    }
}
