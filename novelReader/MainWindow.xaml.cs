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
        private KeyHandler headerListBoxHandler;
        private KeyHandler paragraphListBoxHandler;

        public MainWindow()
        {
            InitializeComponent();
            LoadLastFile();

            headerListBoxHandler = new HeaderListBoxKeyHandler(HeaderListBox, ParagraphListBox);
            paragraphListBoxHandler = new ParagraphListBoxKeyHandler(ParagraphListBox, HeaderListBox);
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

            return true;
        }

        private void openDialog_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.DefaultExt = "txt";
            fileDialog.Filter = "Text files (*.txt)|*.txt";
            fileDialog.ShowDialog();

            if (OpenAndLoadFile(fileDialog.FileName))
            {
                ScrollToLine(0, 0);

                // save to settings
                Settings.Default.LastFilePath = fileDialog.FileName;
                Settings.Default.LastFileChapter = 0;
                Settings.Default.LastFileLineNum = 0;
                Settings.Default.Save();
            }
        }

        private void ScrollToLine(int lineNum, int chapterNum)
        {
            HeaderListBox.SelectedIndex = chapterNum;
            HeaderListBox.ScrollIntoView(HeaderListBox.SelectedItem);

            ParagraphListBox.SelectedIndex = lineNum;
            ParagraphListBox.ScrollIntoView(ParagraphListBox.SelectedItem);
            ParagraphListBox.Focus();
        }

        private void HeaderListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParagraphListBox.SelectedItem = HeaderListBox.SelectedItem;
            ParagraphListBox.ScrollIntoView(ParagraphListBox.SelectedItem);

            // save settings
            Settings.Default.LastFileChapter = HeaderListBox.SelectedIndex;
            Settings.Default.LastFileLineNum = ParagraphListBox.SelectedIndex;
            Settings.Default.Save();

            // focus content
            ParagraphListBox.Focus();
        }

        private void ParagraphListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // find chapter
            App app = (App) Application.Current;

            for (int i = ParagraphListBox.SelectedIndex; i >= 0; i--)
            {
                if (app.Novel[i].IsHeader)
                {
                    HeaderListBox.SelectedItem = app.Novel[i];
                    HeaderListBox.ScrollIntoView(HeaderListBox.SelectedItem);

                    Settings.Default.LastFileChapter = HeaderListBox.SelectedIndex;

                    break;
                }
            }

            // save settings
            Settings.Default.LastFileLineNum = ParagraphListBox.SelectedIndex;
            Settings.Default.Save();

            // update status
            UpdateReadingStatus();
        }

        private void UpdateReadingStatus()
        {
            int totalParagraphs = ParagraphListBox.Items.Count;
            int currentParagraph = ParagraphListBox.SelectedIndex + 1;

            int totalChapters = HeaderListBox.Items.Count;
            int currentChapter = HeaderListBox.SelectedIndex + 1;

            double percentage = ((double) currentParagraph / totalParagraphs) * 100.0;

            StatusTextBox.Text = String.Format("你在 {0}/{1} 章， {2}/{3} 段， 看了差不多 {4:F}%",
                    currentChapter, totalChapters,
                    currentParagraph, totalParagraphs, percentage);
        }

        private void HeaderListBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = headerListBoxHandler.Handle(e);
        }

        private void ParagraphListBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = paragraphListBoxHandler.Handle(e);
        }
    }
}