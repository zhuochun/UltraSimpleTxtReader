using Microsoft.Win32;
using novelReader.Models;
using novelReader.Properties;
using novelReader.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace novelReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ListBoxWrapper editor;
        private ListBoxWrapper sidebar;

        private KeyHandler headerListBoxHandler;
        private KeyHandler paragraphListBoxHandler;

        private Timer autoScrollTimer = new Timer();
        private int autoScrollSpeed = 25; // char per second

        public MainWindow()
        {
            InitializeComponent();

            editor = new Reader(ParagraphListBox);
            sidebar = new Sidebar(HeaderListBox);

            headerListBoxHandler = new HeaderListBoxKeyHandler(sidebar, editor);
            paragraphListBoxHandler = new ParagraphListBoxKeyHandler(editor, sidebar);

            autoScrollTimer.AutoReset = true;
            autoScrollTimer.Interval = 1000;
            autoScrollTimer.Elapsed += autoScrollTimer_Elapsed;

            LoadLastFile();
        }

        private bool OpenAndLoadFile(string path)
        {
            if (String.IsNullOrEmpty(path))
                return false;

            App app = (App) Application.Current;
            app.Novel.Clear();
            app.Headers.Clear();

            NovelFile.Open(path, app.Novel);

            foreach (Content c in app.Novel)
            {
                if (c.IsHeader)
                {
                    app.Headers.Add(c);
                }
            }

            this.Title = "无比简易小说阅读器 - " + path;

            return true;
        }


        private void LoadLastFile()
        {
            try
            {
                if (OpenAndLoadFile(Settings.Default.LastFilePath))
                {
                    ScrollToLine(Settings.Default.LastFileLineNum, Settings.Default.LastFileChapter);
                }
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("你上次阅读的文件： " + e.FileName + " 找不到了，看个别的吧。", "读取文件错误！");

                // reset settings
                Settings.Default.LastFilePath = "";
                Settings.Default.LastFileChapter = -1;
                Settings.Default.LastFileLineNum = -1;
                Settings.Default.Save();
            }
        }

        public void LoadFile(string filename)
        {
            try
            {
                if (OpenAndLoadFile(filename))
                {
                    ScrollToLine(0, 0);

                    // save to settings
                    Settings.Default.LastFilePath = filename;
                    Settings.Default.LastFileChapter = 0;
                    Settings.Default.LastFileLineNum = 0;
                    Settings.Default.Save();
                }
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("你选择的文件： " + e.FileName + " 读取遇到了问题。", "读取文件错误！");

                // reset settings
                Settings.Default.LastFilePath = "";
                Settings.Default.LastFileChapter = -1;
                Settings.Default.LastFileLineNum = -1;
                Settings.Default.Save();
            }
        }

        private void openDialog_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.DefaultExt = "txt";
            fileDialog.Filter = "Text files (*.txt)|*.txt";
            fileDialog.ShowDialog();

            LoadFile(fileDialog.FileName);
        }

        private void ParagraphListBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // handle the first file only
                LoadFile(files[0]);
            }
        }

        private void ScrollToLine(int lineNum, int chapterNum)
        {
            sidebar.FocusOnLine(chapterNum);
            editor.FocusOnLine(lineNum);
            editor.Focus();
        }

        private void HeaderListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            editor.FocusOnLineItem(sidebar.CurrentLineItem());
            editor.Focus();

            // save settings
            Settings.Default.LastFileChapter = sidebar.CurrentLine();
            Settings.Default.LastFileLineNum = editor.CurrentLine();
            Settings.Default.Save();
        }

        private void ParagraphListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App app = (App) Application.Current;

            // empty headers
            if (app.Headers.Count < 1)
            {
                return;
            }

            // find nearest chapter
            object header = app.Headers.First();

            for (int i = editor.CurrentLine(); i >= 0; i--)
            {
                if (app.Novel[i].IsHeader)
                {
                    header = i;
                    break;
                }
            }

            // select chapter
            sidebar.FocusOnLineItem(header);

            // save settings
            Settings.Default.LastFileChapter = sidebar.CurrentLine();
            Settings.Default.LastFileLineNum = editor.CurrentLine();
            Settings.Default.Save();

            // update status
            UpdateReadingStatus();
            UpdateEstimatedTotalReadingTime();
        }

        private void UpdateReadingStatus()
        {
            int totalParagraphs = editor.LineCount();
            int currentParagraph = editor.CurrentLine() + 1;

            int totalChapters = sidebar.LineCount();
            int currentChapter = sidebar.CurrentLine() + 1;

            if (currentParagraph == totalParagraphs)
            {
                StatusTextBox.Text = "你已经看完了！";
            }
            else
            {
                double percentage = ((double) currentParagraph / totalParagraphs) * 100.0;

                StatusTextBox.Text = String.Format("你在第{2}/{3}段，{0}/{1}章，已经看了{4:F}%",
                        currentChapter, totalChapters, currentParagraph, totalParagraphs, percentage);
            }
        }

        private void HeaderListBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = headerListBoxHandler.Handle(e);
        }

        private void ParagraphListBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = paragraphListBoxHandler.Handle(e);
        }

        private void AutoScrollCheckBox_State_Changed(object sender, RoutedEventArgs e)
        {
            if (editor.LineCount() < 1)
            {
                AutoScrollCheckBox.IsChecked = false;
                return;
            }

            if (AutoScrollCheckBox.IsChecked == true)
            {
                autoScrollTimer.Interval = editor.CurrentLineItem().EstimateReadingTime(autoScrollSpeed);
                autoScrollTimer.Enabled = true; 
                autoScrollTimer.Start();
            }
            else
            {
                autoScrollTimer.Stop();
                autoScrollTimer.Enabled = false;
            }

            editor.Focus();
        }

        void autoScrollTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke((Action)(() =>
            {
                if (editor.IsAtLastLine())
                {
                    AutoScrollCheckBox.IsChecked = false;
                }
                else
                {
                    editor.FocusOnNextLine();

                    autoScrollTimer.Interval = editor.CurrentLineItem().EstimateReadingTime(autoScrollSpeed);
                    autoScrollTimer.Start();
                }
            }));
        }

        private void AutoScrollSpeedTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            autoScrollSpeed = Int32.Parse(AutoScrollSpeedTextBox.Text);
        }

        private void UpdateEstimatedTotalReadingTime()
        {
            double totalTime = ((App)Application.Current).Novel
                                   .Skip(editor.CurrentLine())
                                   .Sum(p => p.EstimateReadingTime(autoScrollSpeed)) / 1000.0;

            int hour = (int) totalTime / 3600;
            int minute = ((int)totalTime - hour * 3600) / 60;

            if (hour > 0)
            {
                TotalReadingTimeEstimate.Text = "需" + hour + "小时";
            }
            else if (minute > 1)
            {
                TotalReadingTimeEstimate.Text = "需" + minute + "分钟";
            }
            else
            {
                TotalReadingTimeEstimate.Text = "小于1分钟";
            }
        }
    }
}